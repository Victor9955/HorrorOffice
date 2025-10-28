using UnityEngine;

[CreateAssetMenu(fileName = "RandomCharacterData", menuName = "Scriptable Objects/RandomCharacterData")]
public class RandomCharacterData : CharacterData
{
    public override CharacterStaticInfo staticInfo => CreateRandomCharacterStatic();

    CharacterStaticInfo CreateRandomCharacterStatic()
    {
        CharacterStaticInfo createInfo = new CharacterStaticInfo();

        //TODO Set Random Values

        return createInfo;
    }
}
