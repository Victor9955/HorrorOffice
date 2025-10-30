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

    public void CreateCharacter(CharacterStaticInfo info)
    {
        createInfo = info;
        characterDisplay._animCurve = info.walkCurve;
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
        characterDisplay.CharacterLeave(() =>
        {
            exited = true;
        });
        arrived = false;
    }
}
