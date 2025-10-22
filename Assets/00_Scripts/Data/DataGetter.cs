using NaughtyAttributes;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "DataGetter", menuName = "Scriptable Objects/DataGetter")]
public class DataGetter : ScriptableObject
{
    public List<string> links;

    [Button]
    public async void SyncDataAsync()
    {
        foreach (var link in links)
        {
            UnityWebRequest _webRequest = UnityWebRequest.Get(link);
            await _webRequest.SendWebRequest();
            while (!_webRequest.isDone)
            {
                await Task.Delay(500);
                if (_webRequest.result == UnityWebRequest.Result.ConnectionError) Debug.LogError("Web request failed : connection error");
            }
            string[,] data = CSVToMatrix(_webRequest.downloadHandler.text);
            ParseData(data);
        }
    }

    void ParseData(string[,] data)
    {

    }

    string[,] CSVToMatrix(string csvData)
    {
        if (string.IsNullOrEmpty(csvData))
        {
            Debug.LogError("CSV data is null or empty!");
            return null;
        }

        string[] lines = csvData.Split('\n');
        List<string> validLines = new List<string>();

        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
                validLines.Add(line.Trim());
        }

        if (validLines.Count == 0)
            return null;

        string[] firstLineColumns = validLines[0].Split(',');
        int rows = validLines.Count;
        int cols = firstLineColumns.Length;
        string[,] matrix = new string[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            string[] columns = validLines[i].Split(',');

            for (int j = 0; j < cols; j++)
            {
                if (j < columns.Length)
                    matrix[i, j] = columns[j].Trim();
                else
                    matrix[i, j] = "";
            }
        }

        return matrix;
    }

}
