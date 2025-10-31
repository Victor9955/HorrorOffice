using NaughtyAttributes;
using System;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField,HideInInspector] // sheet position
    CharacterStaticInfo createInfo;
    DayDialogueData dialogue;

    [HideInInspector] public bool arrived;
    [HideInInspector] public bool exited;

    [SerializeField, Required] CharacterDisplay characterDisplay;
    [SerializeField, Required] DialogueData dialogueRef;

    string toSay;

    public void CreateCharacter(CharacterStaticInfo info, DayDialogueData dialogueData)
    {
        createInfo = info;
        dialogue = dialogueData;
        characterDisplay._animCurve = info.walkCurve;

        toSay = string.Empty;
        if (dialogue.dialogs.TryGetValue(createInfo.lastBinder, out string dialogueKey))
        {
            if (dialogueRef.GetDialogue(createInfo.dialogueKey, dialogueKey, out string dialogueStr))
            {
                toSay = dialogueStr;
            }
        }
        else
        {
            if (dialogueRef.GetDialogue(createInfo.dialogueKey, dialogue.DefaultDialogueKey, out string dialogueStr))
            {
                toSay = dialogueStr;
            }
        }
        exited = false;
    }

    public void Play()
    {
        characterDisplay.SpawnCharacter(createInfo.comingSprite,toSay, () =>
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
