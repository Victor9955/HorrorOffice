using UnityEngine;

[CreateAssetMenu(fileName = "DefinedCharacterData", menuName = "Scriptable Objects/DefinedCharacterData")]
public class DefinedCharacterData : CharacterData
{
    [SerializeField] CharacterStaticInfo characterStaticInfo;
    public override CharacterStaticInfo staticInfo { get => characterStaticInfo; }
}
