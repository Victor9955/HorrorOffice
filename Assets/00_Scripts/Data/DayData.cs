using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEditor;
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
    public List<SheetData> sheets;
    public DayDialogueData dayDialogue;
    public UnityEvent finishedEvent;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class DayData : ScriptableObject
{
    public UnityEvent initEvent;
    public UnityEvent startEvent;
    public UnityEvent startFileSortingEvent;

    public List<Binder> binders;
    public List<SheetAction> actions;

    public UnityEvent endEvent;
    public int endDayScene;

    public string Liveliness;
    public string Helpfulness;
    public string Empathy;
    public string Authenticity;
    public string Trustworthiness;
    public string Hopefulness;
}
