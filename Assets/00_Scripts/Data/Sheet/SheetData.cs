using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public enum Binder
{
    Fired,
    Promotion,
    RAS
}

public enum LevelAction
{
    SendEmail
}

[CreateAssetMenu(fileName = "Sheet", menuName = "Scriptable Objects/Sheet")]
public class SheetData : ScriptableObject
{

    [Header("Sheet")]
    [ShowAssetPreview]
    public Sprite spriteSheet;
    public Binder rightBinder;

    [Header("Fired Binder")]
    public GameObject firedMail;

    [Header("RAS Binder")]
    public GameObject rasMail;

    [Header("Promotion Binder")]
    public GameObject promotionMail;


    [Header("Database")]
    public int statA;
    public int statB;
    public int statC;
}
