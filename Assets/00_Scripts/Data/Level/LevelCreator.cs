using NaughtyAttributes;
using UnityEngine;

public class LevelCreator : MonoBehaviour, ISingletonMonobehavior
{
    [SerializeField,Required] CharacterCreator characterCreator;
    [SerializeField] public bool isFinished;
    [HideInInspector] public bool isCreated;
    [HideInInspector] public bool isEnded;

    LevelActionCreateInfo current;
    public void CreateLevel(LevelActionCreateInfo createInfo)
    {
        Debug.Log("<color=green> CREATE LEVEL </color>");
        isFinished = false;
        isCreated = false;
        current = createInfo;

        characterCreator.CreateCharacter(current.character.staticInfo);
        // can awate other events

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

        isCreated = true;
    }

    public void Play()
    {
        Debug.Log("<color=red> BEGIN LEVEL </color>");
        characterCreator.Play(/*Sheet To Place*/);
    }

    public void End()
    {

        Debug.Log("<color=red> END LEVEL </color>");
        characterCreator.Back();
        //Every thing to restart Level
        isEnded = true;
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
