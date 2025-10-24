using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStaticInfo
{
    public string name;
    public Sprite commingSprite;
    public Sprite givePaperSprite;
    public Animation walkCurve;
    public int happyAmount;
}

public class CharacterData : ScriptableObject
{
    public virtual CharacterStaticInfo staticInfo { get; }
    public virtual SheetCreateInfo sheetBaseInfo { get; }
}
