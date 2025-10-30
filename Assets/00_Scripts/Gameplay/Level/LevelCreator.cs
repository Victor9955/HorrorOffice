using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField,Required] CharacterCreator characterCreator;
    [SerializeField,Required] FileSorting fileSorting;
    [HideInInspector] public bool isFinished;
    [HideInInspector] public bool isCreated;
    [HideInInspector] public bool isEnded;

    SheetAction current;

    private void Start()
    {
        fileSorting.OnFileDroppedEvent += () => isFinished = true;
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

        //Sheet Spawning
        fileSorting.OnNewFileRound();
    }

    public IEnumerator End()
    {
        Debug.Log("<color=red> END LEVEL </color>");
        //Every thing to restart Level
        characterCreator.Back();
        yield return new WaitUntil(() => characterCreator.exited);
        isFinished = false;
        isCreated = false;
        isEnded = true;
    }
}
