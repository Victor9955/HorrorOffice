using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStaticInfo
{
    public string name;
    public Sprite comingSprite;
    public Sprite givePaperSprite;
    public AnimationCurve walkCurve;
    public int happyAmount;
    public Vector2 lookOffset;
    public Binder lastBinder;
}

public class CharacterData : ScriptableObject
{
    public virtual CharacterStaticInfo staticInfo { get; }
}
