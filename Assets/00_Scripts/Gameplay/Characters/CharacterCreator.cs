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

    private void Start()
    {
        Singleton.Instance<GameManager>().OnCharacterEnter += () => arrived = true;
    }

    public void CreateCharacter(CharacterStaticInfo info)
    {
        //TODO Setup Sprites walkCurve etc..
        createInfo = info;
        characterDisplay._animCurve = info.walkCurve;
        arrived = false;
        exited = false;
    }

    public void Play()
    {
        //TODO Play Coming Animations
        characterDisplay.SpawnCharacter(createInfo.comingSprite);
    }

    public void Back()
    {
        //TODO Play Character go away
        characterDisplay.OnCharacterDialogueEnd();
        arrived = false;
    }
}
