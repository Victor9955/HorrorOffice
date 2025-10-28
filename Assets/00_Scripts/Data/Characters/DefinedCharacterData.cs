using UnityEngine;

[CreateAssetMenu(fileName = "DefinedCharacterData", menuName = "Scriptable Objects/DefinedCharacterData")]
public class DefinedCharacterData : CharacterData
{
    [SerializeField] CharacterStaticInfo characterStaticInfo;
    [SerializeField] SheetCreateInfo sheetCreateInfo;
    public override CharacterStaticInfo staticInfo { get => characterStaticInfo; }
    public override SheetCreateInfo sheetBaseInfo { get => sheetCreateInfo; }
}
