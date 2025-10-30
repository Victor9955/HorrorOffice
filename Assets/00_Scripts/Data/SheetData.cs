using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Binder
{
    Fired,
    Promotion,
    RAS
}

[CreateAssetMenu(fileName = "Sheet", menuName = "Scriptable Objects/Sheet")]
public class SheetData : ScriptableObject
{
    [Header("Sheet")]
    [ShowAssetPreview]
    public Sprite spriteSheet;
    public Binder rightBinder;

    public SerializedDictionary<Binder,UnityEvent> actions;

    [Header("Database")]
    public int statA;
    public int statB;
    public int statC;
}
