using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStaticInfo
{
    public string name;
    public string dialogueKey;
    public Sprite comingSprite;
    public Sprite givePaperSprite;
    public AnimationCurve walkCurve;
    public int happyAmount;
    public Vector2 lookOffset;
}

public class CharacterData : ScriptableObject
{
    public virtual CharacterStaticInfo staticInfo { get; }
}
