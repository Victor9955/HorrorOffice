using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct DayDialogueData
{
    public string DefaultDialogueKey;
    public SerializedDictionary<Binder, string> dialogs;
}

[System.Serializable]
public struct SheetAction
{
    public CharacterData character;
    public bool beginCondition;
    public List<SheetData> sheets;
    public DayDialogueData dayDialogue;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class DayData : ScriptableObject
{
    public UnityEvent OnBeginDay;
    public List<SheetAction> actions;
}
