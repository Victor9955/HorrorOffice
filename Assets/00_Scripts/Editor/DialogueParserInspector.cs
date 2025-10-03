using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueParser))]
class DialogueParserInspector : Editor
{
    private TextAsset _csvEditor;


    public async override void OnInspectorGUI()
    {
        _csvEditor = (target as DialogueParser).CSVFile;
        //GUILayout.TextField("FileToLoad");
        if(GUILayout.Button("Sync CSV"))
        {
            await DialogueEditorUtils.ImportFromGoogleDrive(_csvEditor);
        }
        base.OnInspectorGUI();
    }
}
