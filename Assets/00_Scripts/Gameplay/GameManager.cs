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
    public Action OnFileSpawned;
    public Action OnDialogueEnd;
    public Action OnCharacterExit;

    #endregion
}
