using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField, Required] CharacterCreator characterCreator;
    [SerializeField, Required] FileSorting fileSorting;
    [HideInInspector] public bool isFinished;
    [HideInInspector] public bool isCreated;
    [HideInInspector] public bool isEnded;
    [SerializeField] public bool debugStartFirstDay;
    SheetAction current;
    private Dictionary<CharacterData, Binder> CharacterSheetDict = new();

    private void Start()
    {
        fileSorting.OnFileDroppedEvent += (binder) =>
        {
            isFinished = true;
            foreach (var sheet in current.sheets)
            {
                sheet.actions.Where((action) => action.binder == binder).First().binderEvent?.Invoke();
            }
        };
    }

    public void CreateLevel(SheetAction createInfo)
    {
        Debug.Log("<color=green> CREATE LEVEL </color>");

        current = createInfo;
        characterCreator.CreateCharacter(current.character.staticInfo);
        isCreated = true;
        isEnded = false;
    }

    public IEnumerator Play()
    {
        Debug.Log("<color=red> BEGIN LEVEL </color>");
        characterCreator.Play();

        yield return new WaitUntil(() => characterCreator.arrived);

        foreach (SheetData sheet in current.sheets)
            fileSorting.OnNewFile(sheet);
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
