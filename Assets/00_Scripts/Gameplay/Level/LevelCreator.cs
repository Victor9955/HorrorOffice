using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelCreator : MonoBehaviour
{
    [SerializeField,Required] CharacterCreator characterCreator;
    [SerializeField,Required] FileSorting fileSorting;
    [HideInInspector] public bool isFinished;
    [HideInInspector] public bool isCreated;
    [HideInInspector] public bool isEnded;
    [SerializeField] FileSender fileSender;
    SheetAction current;
    private Dictionary<CharacterData, Binder> CharacterSheetDict = new();

    private void Start()
    {
        fileSorting.OnFileDroppedEvent += ReceiveBinder;
        fileSender.OnGiveFile += () =>
        {
            if (current.sheets.Count > 0)
            {
                foreach (SheetData sheet in current.sheets)
                {
                    fileSorting.OnNewFile(sheet);
                }
            }
        };
    }

    private void OnDestroy()
    {
        fileSorting.OnFileDroppedEvent -= ReceiveBinder;
    }

    void ReceiveBinder(Binder binder, SheetData sheetData)
    {
        if(sheetData.character != null)
        {
            CharacterStaticInfo info = sheetData.character.staticInfo;
            info.lastBinder = binder;
            current.character.staticInfo = info;
        }
        if(sheetData.actions.TryGetValue(binder, out UnityEvent actionEvent))
        {
            actionEvent?.Invoke();
        }
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
        yield return new WaitUntil(() => DialoguePlayer.IsTalking);
        yield return new WaitUntil(() => !DialoguePlayer.IsTalking);

        isFinished = true;
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
