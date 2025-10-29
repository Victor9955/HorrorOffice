using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class LevelCreator : MonoBehaviour, ISingletonMonobehavior
{
    [SerializeField, Required] CharacterCreator characterCreator;
    [SerializeField, Required] FileSorting fileSorting;
    [HideInInspector] public bool isFinished;
    [HideInInspector] public bool isCreated;
    [HideInInspector] public bool isEnded;

    LevelActionCreateInfo current;

    [SerializeField] private Sprite _debugSheetSprite;
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
        //Sheet Spawning
        fileSorting.OnNewFile(_debugSheetSprite);
    }

    public void End()
    {
        Debug.Log("<color=red> END LEVEL </color>");
        //Every thing to restart Level
        isFinished = false;
        isCreated = false;

    }


}
