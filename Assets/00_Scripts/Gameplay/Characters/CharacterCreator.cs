using NaughtyAttributes;
using System;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField,HideInInspector] // sheet position
    CharacterStaticInfo createInfo;

    [HideInInspector] public bool arrived;
    [HideInInspector] public bool exited;

    [SerializeField, Required] CharacterDisplay characterDisplay;
    [SerializeField, Required] DialogueData dialogueRef;

    string toSay;

    private void Start()
    {
        Singleton.Instance<GameManager>().OnCharacterEnter += () => arrived = true;
    }

    public void CreateCharacter(CharacterStaticInfo info)
    {
        //TODO Setup Sprites walkCurve etc..
        createInfo = info;
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
        characterDisplay.SpawnCharacter(createInfo.comingSprite, () =>
        {
            arrived = true;
        });
    }

    public void Back()
    {
        //TODO Play Character go away
        characterDisplay.OnCharacterDialogueEnd();
        arrived = false;
    }
}
