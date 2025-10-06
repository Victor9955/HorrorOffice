using AYellowpaper.SerializedCollections;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "DataGetter", menuName = "Scriptables/DataGetter")]
public class DataGetter : ScriptableObject
{
    [SerializedDictionary("Character ID", "CSV")]
    public SerializedDictionary<string,TextAsset> _csv = new();

    /// <summary>
    ///              ID Perso          ID Value  Value
    /// </summary>  
    [HideInInspector] public Dictionary<string,Dictionary<string, object>> _persoValue = new();

    private void Start()
    {
        //string[] lines = _csv.text.Split('\n');
        //for (int i = 1; i < lines.Length; i++)
        //{
        //    if (!string.IsNullOrWhiteSpace(lines[i]))
        //    {
        //        string[] dataValues = lines[i].Split(new[] { '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        //    }
        //}
    }

    void Converter(string key, string value)
    {
        switch (key)
        {
            case "STRING":
                //_keyValueCSV.Add(key, value);
                break;
            case "INT":
                //_keyValueCSV.Add(key, int.Parse(value));
                break;
            case "FLOAT":
                //_keyValueCSV.Add(key, float.Parse(value));
                break;
            case "IMAGE":
                //Get Sprite from ScriptableObject using key
                break;
        }
    }
}

public static class DicExtension
{
    public static T GetValueCast<T>(this Dictionary<string,object> collection, string key)
    {
        return (T)collection[key];
    }
}
