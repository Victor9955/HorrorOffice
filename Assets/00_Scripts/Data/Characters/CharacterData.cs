using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct CharacterStaticInfo
{
    public string name;
    public string dialogueKey;

    public SerializedDictionary<string, Sprite> sprites;

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

    [Header("Dialogue Parameters")]
    public float saySpeed;
    public float timeBetweenPhrases;
    public float waitTime;
    public TMP_FontAsset font;

    [Header("Mail")]
    public Sprite mailPP;
}

public class CharacterData : ScriptableObject
{
    public virtual CharacterStaticInfo staticInfo { get; set; }
}
