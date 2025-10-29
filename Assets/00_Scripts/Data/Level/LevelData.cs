using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;



public enum SheetAction
{
    BloodTest,
    Name,
}

public enum SheetValue
{
    A, B, C, D, E, F,//Value as Letters
    RandomAge, // RandomAge between 0 and 100
    ScrambleString,
    ScrambleNum,
    BaseName
}

[System.Serializable]
public struct Sheet
{
    public SheetAction Action;
    public SheetValue Value;
    public string stringValue;
    public int numValue;
}

[System.Serializable]
public struct SheetCreateInfo
{
    public List<Sheet> modifiers;
}

[System.Serializable]
public struct LevelActionCreateInfo
{
    public CharacterData character;
    public SheetCreateInfo sheetCreateInfo;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public List<LevelActionCreateInfo> level;
}
