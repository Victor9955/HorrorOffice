using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Binder
{
    Rehabilitation,
    Growth,
    Adequate,
    Warning,
}


[CreateAssetMenu(fileName = "Sheet", menuName = "Scriptable Objects/Sheet")]
public class SheetData : ScriptableObject
{
    [Header("Sheet")]
    [ShowAssetPreview]
    public Sprite sprite;
    public Binder rightBinder;

    public SerializedDictionary<Binder,UnityEvent> actions;

    [Header("Database")]
    public string charachterId;
    public string characterName;
    public int pausesNumber;
    public string beginTime;
    public string breakfastTime;
    [TextArea(1,20)] public string medicineTime;
    public string HGrade;
    public string EGrade;
    public string AGrade;
    public string LGrade;
    public string TGrade;
    public string HGradeTwo;
}
