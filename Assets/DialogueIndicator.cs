using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogueIndicator : MonoBehaviour
{
    [SerializeField, Required] CharacterDisplay characterCreator;
    [SerializeField] Vector2 padding;
    Transform currentCharacter;
    Image image;

    void Start()
    {
        characterCreator.OnCharcterSpawned += CharacterSpawn;
        characterCreator.OnCharacterExited += CharacterLeave;
        image = GetComponent<Image>();
    }

    private void OnDestroy()
    {
        characterCreator.OnCharcterSpawned -= CharacterSpawn;
        characterCreator.OnCharacterExited -= CharacterLeave;
    }

    private void CharacterLeave()
    {
        currentCharacter = null;
    }

    private void CharacterSpawn(GameObject m_object)
    {
        currentCharacter = m_object.transform;
    }

    void Update()
    {
        if(currentCharacter)
        {
            Vector2 screenSpacePosition = Camera.main.WorldToScreenPoint(currentCharacter.position);
            if(IsInScreen(screenSpacePosition))
            {
                transform.localScale = Vector3.zero;
            }
            else
            {
                transform.localScale = Vector3.one;
                transform.position = Vector2.Lerp(transform.position, screenSpacePosition, Time.deltaTime * 5f);
            }
        }
    }

    public bool IsInScreen(Vector2 screenPosition)
    {
        return screenPosition.x < Screen.width && screenPosition.x > 0 && screenPosition.y < Screen.height && screenPosition.y > 0;
    }
}

