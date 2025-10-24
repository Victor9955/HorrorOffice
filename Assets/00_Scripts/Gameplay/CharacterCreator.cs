using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField] // sheet position
    CharacterStaticInfo createInfo;
    public void CreateCharacter(CharacterStaticInfo info)
    {
        //TODO Setup Sprites walkCurve etc..
        createInfo = info;
    }

    public void Play()
    {
        //TODO Play Coming Animations
    }

    public void Back()
    {
        //TODO Play Character go away
    }
}
