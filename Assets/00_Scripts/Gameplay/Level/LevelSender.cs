using DG.Tweening;
using HuntroxGames.Utils;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LevelSender : MonoBehaviour
{
    [SerializeField,Required] LevelCreator levelCreator;
    [SerializeField,Required] FileSorting fileSorting;
    [SerializeField] private Vector2 randomWaitTimeForCharacter;
    [SerializeField] List<DayData> days;

    [Header("We are in the End game now")]
    [SerializeField] bool beginFirstDay;
    [SerializeField] SceneAsset endGameScene;
    [SerializeField] Volume volume;
    [SerializeField] float vignetteTime = 0.25f;

    static int day;
    DayData current;
    Vignette vignette = null;

    public event Action<DayData> OnBeginDay;
    public event Action OnEndDay;
    public event Action OnEndGame;

    private void Start()
    {
        if(beginFirstDay)
        {
            day = 0;
        }
        volume.profile.TryGet<Vignette>(out vignette);
        BeginDay();
    }

    [ConsoleCommand("BeginDay")]
    public void BeginDay()
    {
        if (current == null) // when current = null current level is finished
        {
            current = days[day];
            current.startEvent?.Invoke();
            fileSorting._binderDataList = current.binders;
            fileSorting.SetupBinders();
        }
    }

    public void StartSheetSorting()
    {
        StartCoroutine(PlayLevel());
    }

    IEnumerator PlayLevel()
    {
        OnBeginDay?.Invoke(current);
        foreach (var levelAction in current.actions)
        {
            //yield return new WaitUntil(() => levelAction.beginCondition);
            levelCreator.CreateLevel(levelAction);
            yield return new WaitUntil(() => levelCreator.isCreated);
            StartCoroutine(levelCreator.Play());
            yield return new WaitUntil(() => levelCreator.isFinished);
            StartCoroutine(levelCreator.End());
            yield return new WaitUntil(() => levelCreator.isEnded);
            yield return new WaitForSeconds(UnityEngine.Random.Range(randomWaitTimeForCharacter.x, randomWaitTimeForCharacter.y));
        }
        day = Mathf.Clamp(day + 1, 0, days.Count - 1);
        if (day == days.Count -1)
        {
            OnEndGame?.Invoke();
            EndDay();
        }
        else
        {
            OnEndDay?.Invoke();
            SceneManager.LoadScene(endGameScene.name);
        }
        current = null;
    }

    [Button("End Day Test")]
    void EndDay()
    {
        vignette.intensity.max = 1000f;
        DOTween.To(() => vignette.intensity.value, (i) => vignette.intensity.value = i, vignette.intensity.max, vignetteTime).OnComplete(() =>
        {
            SceneManager.LoadScene(current.endDayScene.name);
        });
    }
}
