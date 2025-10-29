using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SheetAction
{
    [Required]
    public CharacterData character;

    public List<SheetData> sheets;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public List<SheetAction> actions;
}
