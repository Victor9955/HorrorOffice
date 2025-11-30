using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] CharacterDisplay characterDisplay;

    [Header("Test")]
    [SerializeField] CharacterData testCharacter;
    [SerializeField] private string testKey;

    [Button]
    void Test()
    {
        if(dialogueData.GetDialogue(testKey, out string dialogue))
        {
            characterDisplay.SpawnCharacter(testCharacter.staticInfo, dialogue, null);
        }
    }
}
