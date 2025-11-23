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

[System.Serializable]
public class SheetDataJSON
{
    public string version;
    public string reqId;
    public string status;
    public string sig;
    public SheetTable table;
}

[System.Serializable]
public class SheetTable
{
    public SheetColumn[] cols;
    public SheetRow[] rows;
}

[System.Serializable]
public class SheetColumn
{
    public string id;
    public string label;
    public string type;
}

[System.Serializable]
public class SheetRow
{
    public SheetCell[] c;
}

[System.Serializable]
public class SheetCell
{
    public string v;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] string url;
    [SerializeField] string comasReplacement;
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

    public bool GetDialogue(string characterID, string dialogueKey, out string dialogue)
    {
        if (GetDialogue(characterID + "_" + dialogueKey, out dialogue))
        {
            return true;
        }
        if (GetDialogue(dialogueKey, out dialogue))
        {
            return true;
        }
        Debug.Log("No Dialogue " + dialogueKey);
        return false;
    }

    [Button]
    private async Task SyncDataAsync()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        Debug.Log("<color=yellow> Starting Google Sheets synchronisation... </color>");

        var operation = webRequest.SendWebRequest();
        while (!operation.isDone)
            await Task.Delay(100);

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            ParseGoogleSheetsJSON(webRequest.downloadHandler.text);
            Debug.Log("<color=green> Google Sheets synchronisation successful! </color>");
        }
        else
        {
            Debug.LogError($"Web request failed: {webRequest.error}");
        }
    }

    private void ParseGoogleSheetsJSON(string jsonString)
    {
        dialogues = new SerializedDictionary<string, List<string>>();

        // Clean up the Google Sheets JSON response
        jsonString = CleanGoogleSheetsJSON(jsonString);

        // Parse the JSON
        SheetDataJSON sheetData = JsonUtility.FromJson<SheetDataJSON>(jsonString);

        if (sheetData?.table?.rows == null)
        {
            Debug.LogError("Failed to parse Google Sheets data");
            return;
        }

        // Skip header row (index 0) and process data rows
        for (int i = 1; i < sheetData.table.rows.Length; i++)
        {
            SheetRow row = sheetData.table.rows[i];

            // Extract values from cells
            string characterId = GetCellValue(row, 0);
            string dialogueId = GetCellValue(row, 1);
            string key = GetCellValue(row, 2);
            string english = GetCellValue(row, 3);
            string french = GetCellValue(row, 4);

            // Skip rows without a key
            if (string.IsNullOrEmpty(key))
                continue;

            // Clean up the text
            english = CleanupText(english);
            french = CleanupText(french);

            // Add to dictionary
            dialogues[key] = new List<string> { english, french };
        }

        Debug.Log($"Parsed {dialogues.Count} dialogue entries from Google Sheets");
    }

    private string CleanGoogleSheetsJSON(string jsonString)
    {
        // Remove the Google Visualization API wrapper
        if (jsonString.StartsWith("/*O_o*/"))
        {
            jsonString = jsonString.Substring(7);
        }

        if (jsonString.Contains("google.visualization.Query.setResponse("))
        {
            jsonString = jsonString.Replace("google.visualization.Query.setResponse(", "");
            jsonString = jsonString.TrimEnd(';');
            jsonString = jsonString.TrimEnd(')');
        }

        return jsonString;
    }

    private string GetCellValue(SheetRow row, int columnIndex)
    {
        if (row.c != null && columnIndex < row.c.Length && row.c[columnIndex] != null)
        {
            return row.c[columnIndex].v ?? "";
        }
        return "";
    }

    private string CleanupText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        // Replace Unicode escapes and other special characters
        string cleanedText = text
            .Replace("\\u0027", "'")
            .Replace("\\u2019", "'")
            .Replace("\\u2026", "...")
            .Replace("\\u2018", "'")
            .Replace("\\n", "\n")
            .Replace("\\\"", "\"");

        // Replace custom comas replacement if specified
        if (!string.IsNullOrEmpty(comasReplacement) && comasReplacement.Length > 0)
        {
            cleanedText = cleanedText.Replace(comasReplacement[0], ',');
        }

        return cleanedText;
    }
}