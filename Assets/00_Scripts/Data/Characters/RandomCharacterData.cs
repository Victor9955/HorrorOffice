using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomCharacterData", menuName = "Scriptable Objects/RandomCharacterData")]
public class RandomCharacterData : CharacterData
{
    public override CharacterStaticInfo staticInfo => randoms[Random.Range(0,randoms.Count)];

    [SerializeField] List<CharacterStaticInfo> randoms = new();
}
