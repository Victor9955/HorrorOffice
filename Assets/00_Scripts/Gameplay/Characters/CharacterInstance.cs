using UnityEngine;

public class CharacterInstance : MonoBehaviour
{

    CharacterStaticInfo _characterInfo;

    private SpriteRenderer _spriteRenderer;


    public void Init(CharacterStaticInfo info)
    {
        _spriteRenderer.sprite = info.comingSprite;
    }

}
