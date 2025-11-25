using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private TextMeshProUGUI tmp;


    [SerializeField] private string testKey;

    [Button]
    void Test()
    {
        if(dialogueData.GetDialogue(testKey, out string dialogue))
        {
            tmp.text = dialogue;
        }
    }
}
