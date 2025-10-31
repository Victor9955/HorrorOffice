using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public enum Language
{
    English,
    French
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] string url;
    public Language language;
    public SerializedDictionary<string, List<string>> dialogues;
    
    public bool GetDialogue(string key, out string dialogue)
    {
        dialogue = string.Empty;
        if (dialogues.ContainsKey(key))
        {
            dialogue = dialogues[key][(int)language];
            return true;
        }
        return false;
    }

    public bool GetDialogue(string characterID,string dialogueKey, out string dialogue)
    {
        return GetDialogue(characterID + "_" + dialogueKey, out dialogue);
    }

    [Button]
    private async Task SyncDataAsync()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        Debug.Log("<color=yellow> Starting CSV synchronisation... </color>");

        var operation = webRequest.SendWebRequest();
        while (!operation.isDone)
            await Task.Delay(100);

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            ParseCSV(webRequest.downloadHandler.text);
            Debug.Log("<color=green> CSV synchronisation successful! </color>");
        }
        else
        {
            Debug.LogError($"Web request failed: {webRequest.error}");
        }
    }

    private void ParseCSV(string csvText)
    {
        dialogues = new();

        string[] lines = csvText.Split('\n');

        for (int i = 1; i < lines.Length; i++) // Start at 1 to skip header
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] fields = lines[i].Split(',');

            // Ensure we have enough columns (C, D, E at minimum)
            if (fields.Length >= 5)
            {
                string key = fields[2].Trim(); // Column C
                string english = fields[3].Trim(); // Column D
                string french = fields[4].Trim(); // Column E

                if (!string.IsNullOrEmpty(key))
                {
                    dialogues[key] = new List<string> { english, french };
                }
            }
        }

        Debug.Log($"Parsed {dialogues.Count} dialogue entries");
    }
}
