using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class ScriptableObjectViewerWithMerge : EditorWindow
{
    private ScriptableObject sourceObject;
    private ScriptableObject targetObject;
    private Editor sourceEditor;
    private Editor targetEditor;
    private Dictionary<string, bool> fieldCheckboxStates = new Dictionary<string, bool>();
    private Dictionary<string, bool> listHeaderCheckboxStates = new Dictionary<string, bool>();
    private Dictionary<string, Dictionary<int, bool>> listElementStates = new Dictionary<string, Dictionary<int, bool>>();
    private Vector2 scrollPosition;
    private bool showSource = true;
    private bool showTarget = true;

    [MenuItem("Tools/ScriptableObject Viewer With Merge")]
    public static void ShowWindow()
    {
        GetWindow<ScriptableObjectViewerWithMerge>("SO Viewer With Merge");
    }

    private void OnEnable()
    {
        ClearCachedEditors();
        fieldCheckboxStates.Clear();
        listHeaderCheckboxStates.Clear();
        listElementStates.Clear();
    }

    private void OnGUI()
    {
        DrawObjectFields();

        if (sourceObject == null || targetObject == null)
        {
            EditorGUILayout.HelpBox("Both Source and Target ScriptableObjects must be assigned.", MessageType.Info);
            return;
        }

        if (sourceObject.GetType() != targetObject.GetType())
        {
            EditorGUILayout.HelpBox("Source and Target must be of the same ScriptableObject type.", MessageType.Error);
            return;
        }

        DrawMergeButton();
        DrawInspectorsWithCheckboxes();
    }

    private void DrawObjectFields()
    {
        EditorGUI.BeginChangeCheck();

        sourceObject = (ScriptableObject)EditorGUILayout.ObjectField(
            "Source ScriptableObject",
            sourceObject,
            typeof(ScriptableObject),
            false
        );

        targetObject = (ScriptableObject)EditorGUILayout.ObjectField(
            "Target ScriptableObject",
            targetObject,
            typeof(ScriptableObject),
            false
        );

        if (EditorGUI.EndChangeCheck())
        {
            ClearCachedEditors();
            fieldCheckboxStates.Clear();
            listHeaderCheckboxStates.Clear();
            listElementStates.Clear();
        }
    }

    private void DrawMergeButton()
    {
        EditorGUILayout.Space();
        if (GUILayout.Button("Merge Checked Fields from Source to Target", GUILayout.Height(30)))
        {
            MergeScriptableObjects();
        }
        EditorGUILayout.Space();
    }

    private void DrawInspectorsWithCheckboxes()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Source Object
        showSource = EditorGUILayout.Foldout(showSource, $"Source: {sourceObject.name}", true);
        if (showSource)
        {
            EnsureCachedEditor(ref sourceEditor, sourceObject);
            DrawObjectInspectorWithCheckboxes(sourceObject, sourceEditor, false);
        }

        EditorGUILayout.Space();

        // Target Object
        showTarget = EditorGUILayout.Foldout(showTarget, $"Target: {targetObject.name}", true);
        if (showTarget)
        {
            EnsureCachedEditor(ref targetEditor, targetObject);
            DrawObjectInspectorWithCheckboxes(targetObject, targetEditor, true);
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawObjectInspectorWithCheckboxes(ScriptableObject obj, Editor editor, bool isTarget)
    {
        if (editor == null) return;

        EditorGUILayout.BeginVertical("box");

        SerializedObject serializedObject = new SerializedObject(obj);
        SerializedProperty property = serializedObject.GetIterator();

        bool enterChildren = true;
        while (property.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (property.name == "m_Script") // Skip the script reference
                continue;

            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                DrawListWithElementSelection(property, isTarget);
            }
            else
            {
                DrawFieldWithCheckbox(property, isTarget);
            }
        }

        if (isTarget)
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawFieldWithCheckbox(SerializedProperty property, bool isTarget)
    {
        EditorGUILayout.BeginHorizontal();

        // Checkbox (only for source object fields)
        if (!isTarget)
        {
            string key = property.propertyPath;
            if (!fieldCheckboxStates.ContainsKey(key))
            {
                fieldCheckboxStates[key] = false;
            }

            // Create a separate area for the checkbox to ensure it's clickable
            Rect checkboxRect = GUILayoutUtility.GetRect(20, 20, GUILayout.Width(20));
            fieldCheckboxStates[key] = EditorGUI.Toggle(checkboxRect, fieldCheckboxStates[key]);
        }
        else
        {
            GUILayout.Space(24); // Maintain alignment
        }

        // Property field - make it read-only for source
        EditorGUI.BeginDisabledGroup(!isTarget);
        EditorGUILayout.PropertyField(property, true);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();

        // Visual feedback for checked state (source only)
        if (!isTarget && fieldCheckboxStates.ContainsKey(property.propertyPath) && fieldCheckboxStates[property.propertyPath])
        {
            DrawCheckedStateHighlight();
        }
    }

    private void DrawListWithElementSelection(SerializedProperty listProperty, bool isTarget)
    {
        string listKey = listProperty.propertyPath;

        EditorGUILayout.BeginVertical("box");

        // List header with checkbox for entire list replacement
        EditorGUILayout.BeginHorizontal();

        if (!isTarget)
        {
            // Initialize list header and element states if needed
            if (!listHeaderCheckboxStates.ContainsKey(listKey))
            {
                listHeaderCheckboxStates[listKey] = false;
            }
            if (!listElementStates.ContainsKey(listKey))
            {
                listElementStates[listKey] = new Dictionary<int, bool>();
            }

            // List header checkbox - for replacing entire list
            Rect headerCheckboxRect = GUILayoutUtility.GetRect(20, 20, GUILayout.Width(20));
            listHeaderCheckboxStates[listKey] = EditorGUI.Toggle(headerCheckboxRect, listHeaderCheckboxStates[listKey]);

            // Show info about what this checkbox does
            EditorGUILayout.LabelField($"{listProperty.displayName} (Replace entire list)", EditorStyles.boldLabel);
        }
        else
        {
            GUILayout.Space(24);
            EditorGUILayout.LabelField(listProperty.displayName, EditorStyles.boldLabel);
        }
        EditorGUILayout.EndHorizontal();

        // Draw the array size field for target (editable)
        if (isTarget)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(24); // Align with elements
            EditorGUILayout.PropertyField(listProperty.FindPropertyRelative("Array.size"));
            EditorGUILayout.EndHorizontal();
        }

        // List elements with individual checkboxes (only if list header is not checked)
        if (!isTarget && !listHeaderCheckboxStates[listKey])
        {
            EditorGUILayout.LabelField("Select individual elements to append:", EditorStyles.miniLabel);

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty element = listProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();

                // Element checkbox
                if (!listElementStates[listKey].ContainsKey(i))
                {
                    listElementStates[listKey][i] = false;
                }

                Rect elementCheckboxRect = GUILayoutUtility.GetRect(20, 20, GUILayout.Width(20));
                listElementStates[listKey][i] = EditorGUI.Toggle(elementCheckboxRect, listElementStates[listKey][i]);

                // Element property field - read-only for source
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(element, new GUIContent($"Element {i}"), true);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndHorizontal();
            }
        }
        else if (!isTarget)
        {
            EditorGUILayout.HelpBox($"Entire {listProperty.displayName} list will be replaced with source list.", MessageType.Info);
        }
        else
        {
            // For target, just show the elements normally
            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty element = listProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(24);
                EditorGUILayout.PropertyField(element, new GUIContent($"Element {i}"), true);
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawCheckedStateHighlight()
    {
        Rect lastRect = GUILayoutUtility.GetLastRect();
        lastRect.xMin += 25;
        EditorGUI.DrawRect(lastRect, new Color(0.2f, 0.8f, 0.2f, 0.1f));
    }

    private void MergeScriptableObjects()
    {
        if (sourceObject == null || targetObject == null)
        {
            Debug.LogError("Cannot merge: Source or Target is null.");
            return;
        }

        if (sourceObject.GetType() != targetObject.GetType())
        {
            Debug.LogError("Cannot merge: Source and Target are not of the same type.");
            return;
        }

        Undo.RecordObject(targetObject, "Merge ScriptableObjects");
        SerializedObject sourceSerialized = new SerializedObject(sourceObject);
        SerializedObject targetSerialized = new SerializedObject(targetObject);

        bool mergedAny = false;

        // Merge regular fields
        SerializedProperty sourceProp = sourceSerialized.GetIterator();
        bool enterChildren = true;
        while (sourceProp.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (sourceProp.name == "m_Script") continue;

            string key = sourceProp.propertyPath;

            // Check if this field is checked for merging
            if (fieldCheckboxStates.ContainsKey(key) && fieldCheckboxStates[key])
            {
                SerializedProperty targetProp = targetSerialized.FindProperty(key);
                if (targetProp != null && targetProp.propertyType == sourceProp.propertyType)
                {
                    CopySerializedProperty(sourceProp, targetProp);
                    mergedAny = true;
                }
            }

            // Merge lists
            if (sourceProp.isArray && sourceProp.propertyType != SerializedPropertyType.String)
            {
                SerializedProperty targetList = targetSerialized.FindProperty(key);
                if (targetList != null)
                {
                    if (listHeaderCheckboxStates.ContainsKey(key) && listHeaderCheckboxStates[key])
                    {
                        // Replace entire list
                        ReplaceEntireList(sourceProp, targetList);
                        mergedAny = true;
                    }
                    else if (listElementStates.ContainsKey(key))
                    {
                        // Append selected elements
                        AppendSelectedListElements(sourceProp, targetList, key);
                        mergedAny = true;
                    }
                }
            }
        }

        if (mergedAny)
        {
            targetSerialized.ApplyModifiedProperties();
            EditorUtility.SetDirty(targetObject);
            AssetDatabase.SaveAssets();

            // Show summary
            ShowMergeSummary();
        }
        else
        {
            Debug.LogWarning("No fields were selected for merging.");
        }

        // Refresh the target editor
        if (targetEditor != null)
        {
            DestroyImmediate(targetEditor);
            targetEditor = Editor.CreateEditor(targetObject);
        }
    }

    private void ReplaceEntireList(SerializedProperty sourceList, SerializedProperty targetList)
    {
        if (sourceList == null || targetList == null || !sourceList.isArray) return;

        // Clear the target list
        targetList.arraySize = 0;

        // Copy all elements from source to target
        for (int i = 0; i < sourceList.arraySize; i++)
        {
            targetList.arraySize++;
            SerializedProperty sourceElement = sourceList.GetArrayElementAtIndex(i);
            SerializedProperty targetElement = targetList.GetArrayElementAtIndex(i);
            CopySerializedProperty(sourceElement, targetElement);
        }
    }

    private void AppendSelectedListElements(SerializedProperty sourceList, SerializedProperty targetList, string listKey)
    {
        if (sourceList == null || targetList == null || !sourceList.isArray) return;
        if (!listElementStates.ContainsKey(listKey)) return;

        int appendedCount = 0;
        for (int i = 0; i < sourceList.arraySize; i++)
        {
            if (listElementStates[listKey].ContainsKey(i) && listElementStates[listKey][i])
            {
                // Add the element to target list
                targetList.arraySize++;
                SerializedProperty sourceElement = sourceList.GetArrayElementAtIndex(i);
                SerializedProperty targetElement = targetList.GetArrayElementAtIndex(targetList.arraySize - 1);
                CopySerializedProperty(sourceElement, targetElement);
                appendedCount++;
            }
        }
    }

    private void ShowMergeSummary()
    {
        int fieldCount = 0;
        int listReplaceCount = 0;
        int listAppendCount = 0;

        foreach (var kvp in fieldCheckboxStates)
        {
            if (kvp.Value) fieldCount++;
        }

        foreach (var kvp in listHeaderCheckboxStates)
        {
            if (kvp.Value) listReplaceCount++;
        }

        foreach (var kvp in listElementStates)
        {
            if (listHeaderCheckboxStates.ContainsKey(kvp.Key) && listHeaderCheckboxStates[kvp.Key])
                continue; // Skip if entire list is being replaced

            foreach (var element in kvp.Value)
            {
                if (element.Value) listAppendCount++;
            }
        }

        Debug.Log($"Merge Summary: {fieldCount} fields, {listReplaceCount} lists replaced, {listAppendCount} list elements appended");
    }

    private void CopySerializedProperty(SerializedProperty source, SerializedProperty target)
    {
        if (source.propertyType != target.propertyType) return;

        switch (source.propertyType)
        {
            case SerializedPropertyType.Integer:
                target.intValue = source.intValue;
                break;
            case SerializedPropertyType.Boolean:
                target.boolValue = source.boolValue;
                break;
            case SerializedPropertyType.Float:
                target.floatValue = source.floatValue;
                break;
            case SerializedPropertyType.String:
                target.stringValue = source.stringValue;
                break;
            case SerializedPropertyType.Color:
                target.colorValue = source.colorValue;
                break;
            case SerializedPropertyType.ObjectReference:
                target.objectReferenceValue = source.objectReferenceValue;
                break;
            case SerializedPropertyType.LayerMask:
                target.intValue = source.intValue;
                break;
            case SerializedPropertyType.Enum:
                target.enumValueIndex = source.enumValueIndex;
                break;
            case SerializedPropertyType.Vector2:
                target.vector2Value = source.vector2Value;
                break;
            case SerializedPropertyType.Vector3:
                target.vector3Value = source.vector3Value;
                break;
            case SerializedPropertyType.Vector4:
                target.vector4Value = source.vector4Value;
                break;
            case SerializedPropertyType.Rect:
                target.rectValue = source.rectValue;
                break;
            case SerializedPropertyType.AnimationCurve:
                target.animationCurveValue = source.animationCurveValue;
                break;
            case SerializedPropertyType.Bounds:
                target.boundsValue = source.boundsValue;
                break;
            case SerializedPropertyType.Quaternion:
                target.quaternionValue = source.quaternionValue;
                break;
            case SerializedPropertyType.Generic:
                // For complex types, handle arrays and nested properties
                if (source.isArray && target.isArray)
                {
                    target.arraySize = source.arraySize;
                    for (int i = 0; i < source.arraySize; i++)
                    {
                        CopySerializedProperty(source.GetArrayElementAtIndex(i), target.GetArrayElementAtIndex(i));
                    }
                }
                else
                {
                    // For nested types, copy all child properties
                    var sourceIterator = source.Copy();
                    var targetIterator = target.Copy();
                    bool enterChildren = true;
                    while (sourceIterator.NextVisible(enterChildren) && targetIterator.NextVisible(enterChildren))
                    {
                        enterChildren = false;
                        CopySerializedProperty(sourceIterator, targetIterator);
                    }
                }
                break;
        }
    }

    private void EnsureCachedEditor(ref Editor editor, ScriptableObject targetObj)
    {
        if (editor == null || editor.target != targetObj)
        {
            if (editor != null)
            {
                DestroyImmediate(editor);
            }
            editor = Editor.CreateEditor(targetObj);
        }
    }

    private void ClearCachedEditors()
    {
        if (sourceEditor != null)
        {
            DestroyImmediate(sourceEditor);
            sourceEditor = null;
        }
        if (targetEditor != null)
        {
            DestroyImmediate(targetEditor);
            targetEditor = null;
        }
    }

    private void OnDisable()
    {
        ClearCachedEditors();
        fieldCheckboxStates.Clear();
        listHeaderCheckboxStates.Clear();
        listElementStates.Clear();
    }

    // Utility methods
    public List<string> GetCheckedFields()
    {
        List<string> checkedFields = new List<string>();
        foreach (var kvp in fieldCheckboxStates)
        {
            if (kvp.Value)
            {
                checkedFields.Add(kvp.Key);
            }
        }
        return checkedFields;
    }

    public List<string> GetListsToReplace()
    {
        List<string> listsToReplace = new List<string>();
        foreach (var kvp in listHeaderCheckboxStates)
        {
            if (kvp.Value)
            {
                listsToReplace.Add(kvp.Key);
            }
        }
        return listsToReplace;
    }

    public List<string> GetListElementsToAppend()
    {
        List<string> elementsToAppend = new List<string>();
        foreach (var listKvp in listElementStates)
        {
            // Skip if entire list is being replaced
            if (listHeaderCheckboxStates.ContainsKey(listKvp.Key) && listHeaderCheckboxStates[listKvp.Key])
                continue;

            foreach (var elementKvp in listKvp.Value)
            {
                if (elementKvp.Value)
                {
                    elementsToAppend.Add($"{listKvp.Key}[{elementKvp.Key}]");
                }
            }
        }
        return elementsToAppend;
    }
}