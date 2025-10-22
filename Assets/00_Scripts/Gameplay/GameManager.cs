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
    public Action<int> OnNewRound;
    public Action OnStartRound;
    public Action<bool> OnStopRound;


    #endregion

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //Round Manager
        InitRoundManager();
    }

    private void InitRoundManager()
    {
        FileRoundManager roundMan = Singleton.Instance<FileRoundManager>();
        roundMan.Init();
        OnNewRound?.Invoke(roundMan.StartingRoundInd); // INVOKE ON NEW ROUND ON INIT
    }
}
