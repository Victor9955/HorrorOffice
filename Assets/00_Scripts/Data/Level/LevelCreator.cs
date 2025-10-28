using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class LevelCreator : MonoBehaviour, ISingletonMonobehavior
{
    [SerializeField,Required] CharacterCreator characterCreator;
    [SerializeField,Required] FileSorting fileSorting;
    [HideInInspector] public bool isFinished;
    [HideInInspector] public bool isCreated;
    [HideInInspector] public bool isEnded;

    LevelActionCreateInfo current;

    private void Start()
    {
        Singleton.Instance<GameManager>().OnCharacterExit += () => isEnded = true;
    }
    public void CreateLevel(LevelActionCreateInfo createInfo)
    {
        Debug.Log("<color=green> CREATE LEVEL </color>");

        current = createInfo;

        characterCreator.CreateCharacter(current.character.staticInfo);
        fileSorting.OnFileDroppedEvent += FileDropped;
        // can awate other events
        isCreated = true;
        isEnded = false;
    }

    private void FileDropped()
    {
        characterCreator.Back();
        isFinished = true;
    }

    public IEnumerator Play()
    {
        Debug.Log("<color=red> BEGIN LEVEL </color>");
        characterCreator.Play();

        yield return new WaitUntil(() => characterCreator.arrived);
        
        foreach (var item in current.character.sheetBaseInfo.modifiers)
        {
            Init(item.Action, item.Value);
        }

        //Sheet Changing Data
        foreach (var item in current.sheetCreateInfo.modifiers)
        {
            Modifie(item.Action, item.Value);
        }

        //Sheet Spawning
        fileSorting.OnNewFileRound();
    }

    public void End()
    {
        Debug.Log("<color=red> END LEVEL </color>");
        //Every thing to restart Level
        isFinished = false;
        isCreated = false;

    }

    void Init(SheetAction action , SheetValue sheetValue)
    {
        Debug.Log("<color=yellow> Action From Base " + action.ToString());
        switch (action)
        {   
            case SheetAction.BloodTest:
                break;
            case SheetAction.Name:
                break;
        }
    }

    void Modifie(SheetAction action, SheetValue sheetValue)
    {
        Debug.Log("<color=yellow> Action From Sheet " + action.ToString());
        switch (action)
        {
            case SheetAction.BloodTest:
                break;
            case SheetAction.Name:
                Debug.Log(current.character.staticInfo.name);
                break;
        }
    }
}
