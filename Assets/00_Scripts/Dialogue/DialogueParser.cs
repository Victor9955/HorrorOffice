using System;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    [SerializeField] private TextAsset _csvFile;

    public TextAsset CSVFile => _csvFile;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        ParseDialogueCSV();
    }

    private void ParseDialogueCSV()
    {
        string[] lines = _csvFile.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                string[] dataValues = lines[i].Split(new[] { '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                //throw new NotImplementedException();
                // NEED TO CONVERT IT INTO A 'DIALOGUE' CLASS VRO
            }
        }
    }
    public static bool IsPunctuation(char _char) => (_char == ',') || (_char == '.') || (_char == '?') || (_char == '!') || (_char == ';') || (_char == ':');

}

