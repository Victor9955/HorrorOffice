using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


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
    public UnityEvent OnBeginDay;
    public List<SheetAction> actions;
    [Required] public DialogueData dialogues;
}
