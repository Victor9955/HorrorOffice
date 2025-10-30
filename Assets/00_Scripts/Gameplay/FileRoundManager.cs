using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class Round
{
    public int RoundInd { get; private set; }
    public float Time { get; private set; }

    public Round(int roundInd, float time)
    {
        RoundInd = roundInd;
        Time = time;
    }
}

public class FileRoundManager : MonoBehaviour, ISingletonMonobehavior
{
    #region Fields

    [SerializeField] private FileSorting _fileSorter;

    [SerializeField] private int _startingRound;

    [SerializeField] private float _startingTime = 20f;

    private GameManager _gameManager;

    private float _timeRemaining;
    public float TimeRemaining
    {
        get => _timeRemaining;
        set
        {
            _timeRemaining = value;
            OnUpdateTime?.Invoke(_timeRemaining);
        }
    }

    public string Timer => MathF.Round(TimeRemaining, 2).ToString();
    private Round _currentRound;

    public int CurrentRoundInd { get; private set; }
    public int StartingRoundInd => _startingRound;

    public Round CurrentRound
    {
        get => _currentRound;
        set
        {
            _currentRound = value;
            CurrentRoundInd = value.RoundInd;
        }
    }
    public Coroutine CurrentRoundCoroutine { get; private set; }
    public bool isRoundEnding { get; set; }

    public Action<float> OnUpdateTime;

    #endregion


    public void Init()
    {
        _fileSorter.Init();
        CurrentRoundInd = _startingRound;
        TimeRemaining = _startingTime;
        CurrentRound = GetNewRound();
        Singleton.Instance<GameManager>().OnNewRound += StartRound; // REGISTER ON NEW ROUND
        
    }
    public void StartRound(int roundInt)
    {
        CurrentRoundCoroutine = StartCoroutine(RunCurrentRound());
        Singleton.Instance<GameManager>().OnStartRound?.Invoke(); // INVOKE START ROUND
    }

    private Round GetNewRound()
    {
        return new Round(CurrentRoundInd, TimeRemaining);
    }

    private IEnumerator RunCurrentRound()
    {
        Utils.BigText($"Round n°{CurrentRoundInd + 1}, {CurrentRound.Time} seconds remaining");
        while (TimeRemaining > 0)
        {
            TimeRemaining -= (Time.deltaTime * Time.timeScale);
            yield return null;
        }
        StartCoroutine(StopRound(false));
    }
    public IEnumerator StopRound(bool hasWon)
    {
        if (CurrentRoundCoroutine != null)
        {
            StopCoroutine(CurrentRoundCoroutine);
            CurrentRoundCoroutine = null;
        }
        Singleton.Instance<GameManager>().OnStopRound?.Invoke(hasWon);
        isRoundEnding = true;
        while (isRoundEnding)
        {
            yield return null;
        }
        NextRound();
    }

    public void OnMatch(bool guess)
    {
        if (CurrentRoundCoroutine == null) return;
        Utils.BigText($"Time Remaining : {Timer}");
        StartCoroutine(StopRound(guess));
    }

    private void NextRound()
    {
        CurrentRoundInd++;
        CurrentRound = GetNewRound();
        Singleton.Instance<GameManager>().OnNewRound?.Invoke(CurrentRoundInd); // NEXT ROUND
    }
}



