using NaughtyAttributes;
using System;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField,HideInInspector] // sheet position
    CharacterStaticInfo characterInfo;
    DayDialogueData dialogue;

    [HideInInspector] public bool arrived;
    [HideInInspector] public bool exited;

    [SerializeField, Required] CharacterDisplay characterDisplay;
    [SerializeField, Required] DialogueData dialogueData;

    string toSay;

    public void CreateCharacter(CharacterStaticInfo info, DayDialogueData dialogueData, Sprite ovverideSprite = null)
    {
        characterInfo = info;
        dialogue = dialogueData;
        exited = false;
        if(ovverideSprite != null)
        {
            characterInfo.comingSprite = ovverideSprite;
        }
    }

    public void Play()
    {
        toSay = string.Empty;
        if (dialogue.dialogs.TryGetValue(characterInfo.lastBinder, out string dialogueKey))
        {
            dialogueData.GetDialogue(characterInfo.dialogueKey, dialogueKey, out toSay);
        }
        else
        {
            dialogueData.GetDialogue(characterInfo.dialogueKey, dialogue.DefaultDialogueKey, out toSay);
        }

        characterDisplay.SpawnCharacter(characterInfo, toSay, () =>
        {
            arrived = true;
        });
    }

    public void Back()
    {
        characterDisplay.CharacterLeave(() =>
        {
            exited = true;
        });
        arrived = false;
    }
}
