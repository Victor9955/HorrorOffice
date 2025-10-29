using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SheetCreateInfo
{
    public string characterName;
}

[System.Serializable]
public struct LevelActionCreateInfo
{
    public CharacterData character;
    public SheetCreateInfo rightSheet;
    public SheetCreateInfo wrongSheet;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public List<LevelActionCreateInfo> level;
}
