using NaughtyAttributes;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelCreator : MonoBehaviour
{
    [SerializeField,Required] CharacterCreator characterCreator;
    [SerializeField,Required] FileSorting fileSorting;
    [SerializeField,Required] DialogueData dialogueData;
    [HideInInspector] public bool isFinished;
    [HideInInspector] public bool isCreated;
    [HideInInspector] public bool isEnded;

    SheetAction current;

    private void Start()
    {
        fileSorting.OnFileDroppedEvent += (binder) =>
        {
            isFinished = true;
            foreach (var sheet in current.sheets)
            {
                if(sheet.actions.TryGetValue(binder,out UnityEvent cash))
                {
                    cash?.Invoke();
                }
            }
        };
    }

    public void CreateLevel(SheetAction createInfo)
    {
        Debug.Log("<color=green> CREATE LEVEL </color>");

        current = createInfo;
        characterCreator.CreateCharacter(createInfo.character.staticInfo, createInfo.dayDialogue);
        isCreated = true;
        isEnded = false;
    }

    public IEnumerator Play()
    {
        Debug.Log("<color=red> BEGIN LEVEL </color>");
        characterCreator.Play();

        yield return new WaitUntil(() => characterCreator.arrived);

        if(dialogueData.GetDialogue(current.character.staticInfo.dialogueKey, current.dayDialogue.DefaultDialogueKey, out string dialogue))
        {
            Debug.Log(dialogue);
        }
        
        foreach (SheetData sheetAction in current.sheets)
        {
            //fileSorting.OnNewFileRound();
            //Sheet Spawning
        }
        fileSorting.OnNewFileRound(/*Sheet Data*/);
    }

    public IEnumerator End()
    {
        Debug.Log("<color=red> END LEVEL </color>");
        //Every thing to restart Leve
        characterCreator.Back();
        yield return new WaitUntil(() => characterCreator.exited);
        isFinished = false;
        isCreated = false;
        isEnded = true;
    }
}
