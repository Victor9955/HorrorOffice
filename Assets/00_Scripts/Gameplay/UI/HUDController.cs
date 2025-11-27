using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    [Header("Refs")]
    [SerializeField] GameObject charThought;
    [SerializeField] DialogueData dialogueData;

    private TMP_Text textCharThought;

    private void Awake()
    {
        if (instance == null) instance = this;
        
    }


    public void SetThoughtText(string id)
    {
        dialogueData.GetDialogue(id, out string dialogue);
    }
    public void SetThoughtActive(bool active)
    {
        if (active)
        {
            charThought.SetActive(active);
        }
    }
}
