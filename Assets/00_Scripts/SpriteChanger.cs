using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DialoguePlayer dialoguePlayer;

    public void ChangeSprite(string key)
    {
        if (dialoguePlayer.current.sprites.TryGetValue(key, out Sprite sprite))
        {
            spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogError("No Sprite " + key + " on " + dialoguePlayer.current.name);
        }
    }
}
