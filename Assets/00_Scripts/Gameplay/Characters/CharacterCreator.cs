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

    public void CreateCharacter(CharacterStaticInfo info, DayDialogueData dialogueData)
    {
        createInfo = info;
        dialogue = dialogueData;
        characterDisplay._animCurve = info.walkCurve;
        exited = false;
    }

    public void Play()
    {
        if(dialogue.dialogs.TryGetValue(createInfo.lastBinder, out string dialogueKey))
        {
            Debug.Log(dialogueKey);
        }
        else
        {
            Debug.Log(dialogue.DefaultDialogueKey);
        }
        
        characterDisplay.SpawnCharacter(createInfo.comingSprite, () =>
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
