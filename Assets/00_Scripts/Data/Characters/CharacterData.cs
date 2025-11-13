using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStaticInfo
{
    public string name;
    public string dialogueKey;
    public Sprite comingSprite;
    public Sprite givePaperSprite;
    public Vector2 lookOffset;
   [HideInInspector] public Binder lastBinder;


    [Header("Anim Parameters")]
    public float _enterDuration;
    public float _exitDuration;
    public  float _walkMagnitude;
    public int _walkFrequency;
     public AnimationCurve _animCurve;
}

public class CharacterData : ScriptableObject
{
    public virtual CharacterStaticInfo staticInfo { get; }
}
