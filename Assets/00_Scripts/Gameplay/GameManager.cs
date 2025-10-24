using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour, ISingletonMonobehavior
{

    #region GameEvents
    //Rounds events
    public Action<int> OnNewRound;
    public Action OnStartRound;
    public Action<bool> OnStopRound;

    //Character enter & File
    public Action OnCharacterEnter;
    public Action OnFileSpawned;
    public Action OnDialogueEnd;
    public Action OnCharacterExit;

    #endregion

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //Round Manager
        FileRoundManager roundMan = Singleton.Instance<FileRoundManager>();
        Singleton.Instance<FileRoundManager>().Init();

        //Character Displayer
        Singleton.Instance<CharacterDisplay>().Init();

        OnNewRound?.Invoke(roundMan.StartingRoundInd); // INVOKE ON NEW ROUND ON INIT
    }

    private void InitRoundManager()
    {
    }
}
