using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SheetAction
{
    [Required]
    public CharacterData character;
    public bool beginCondition;
    public List<SheetData> sheets;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class DayData : ScriptableObject
{
    public List<SheetAction> actions;
}
