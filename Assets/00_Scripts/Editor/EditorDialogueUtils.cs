using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public static class DialogueEditorUtils
{
    //const string CSV_URL = "https://docs.google.com/spreadsheets/d/1VIUEIFc0PBlgMFKcpnok2PT3joCAU0wYwM3H5UpBdo4/edit?gid=0#gid=0";
    const string CSV_URL = "https://docs.google.com/spreadsheets/d/1VIUEIFc0PBlgMFKcpnok2PT3joCAU0wYwM3H5UpBdo4/edit?usp=sharing";

   static string CSV_PATH = "Assets/06_CSV/Dialogue_Horror_Office_test1.csv";
    private readonly static string _fileName;
    private static UnityWebRequest _webRequest = null;



    public static async Task ImportFromGoogleDrive(TextAsset csvEditor)
    {
        if (null != _webRequest) return;
        _webRequest = UnityWebRequest.Get(CSV_URL);
        Debug.Log("<color=white>Starting CSV synchronisation...</color>");
        _webRequest.SendWebRequest();
        while (!_webRequest.isDone)
        {
            await Task.Delay(500);
            if (_webRequest.result == UnityWebRequest.Result.ConnectionError) Debug.LogError("Web request failed : connection error");
        }
        CSV_PATH = AssetDatabase.GetAssetPath(csvEditor.GetInstanceID());
        Utils.BigText(CSV_PATH);
        Utils.BigText(CSV_URL);
        EditorUpdate();
    }

    private static void EditorUpdate()
    {
        if (_webRequest == null)
        {
            Debug.LogError($"Web Request Error : <color=white>{_webRequest.error}</color>");
        }
        else
        {
            //Write Into TextAsset
            TextAsset csvAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(CSV_PATH);
            
            File.WriteAllText(AssetDatabase.GetAssetPath(csvAsset), _webRequest.downloadHandler.text);
            EditorUtility.SetDirty(csvAsset);
            AssetDatabase.Refresh();
            
            Debug.Log($"<size=15><color=white>Import CSV From GoogleDrive Complete !</color></size>");
        }
        EditorApplication.update -= EditorUpdate;
        _webRequest = null;
    }
}