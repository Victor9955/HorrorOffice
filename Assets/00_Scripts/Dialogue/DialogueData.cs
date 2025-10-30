using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DayDialogueData
{
    public string DefaultDialogueKey;
    public SerializedDictionary<Binder, string> dialogs;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    public SerializedDictionary<CharacterData,DayDialogueData> dialogues;
}
