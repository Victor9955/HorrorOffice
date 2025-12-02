using DG.Tweening;
using HuntroxGames.Utils;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSender : MonoBehaviour
{
    [SerializeField,Required] LevelCreator levelCreator;
    [SerializeField,Required] FileSorting fileSorting;
    [SerializeField] private Vector2 randomWaitTimeForCharacter;
    [SerializeField] List<DayData> days;

    [Header("End Shift Button")]
    [SerializeField] TextMeshProUGUI endShiftText;
    [SerializeField] Image endShiftImage;
    [SerializeField] Button endShiftButton;

    [Header("End")]
    [SerializeField] bool beginFirstDay;
    [SerializeField] int endGameScene;
    [SerializeField] Material fullscreenVignette;
    [SerializeField] float vignetteTime = 0.25f;
    [SerializeField] WindowAnimation evaluationRerport;

    static int day;
    DayData current;

    public event Action<DayData> OnBeginDay;
    public event Action<DayData> OnInitDatabase;
    public event Action OnEndDay;
    public event Action OnEndGame;

    int endDayIndex = 0;
    private void Start()
    {
        if(beginFirstDay)
        {
            day = 0;
        }
        BeginDay();
    }

    public void BeginDay()
    {
        if (current == null) // when current = null current level is finished
        {
            current = days[day];
            current.startEvent?.Invoke();
            fileSorting._binderDataList = current.binders;
            fileSorting.SetupBinders();
            endDayIndex = current.endDayScene;
            OnInitDatabase?.Invoke(current);
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
        endShiftButton.interactable = true;
        endShiftImage.color = Color.red;
        current = null;
    }

    private void OnDestroy()
    {
        fullscreenVignette.SetFloat("_EyesClosed", 1f);
        fullscreenVignette.SetFloat("_Smoothness", 0f);
    }

    public void EndShiftButtonCallback()
    {
        if(current == null)
        {
            if (day == days.Count - 1)
            {
                DOVirtual.Float(1f, 0f, vignetteTime, (eye) =>
                {
                    fullscreenVignette.SetFloat("_EyesClosed", eye);
                    fullscreenVignette.SetFloat("_Smoothness", eye);
                }).OnComplete(() =>
                {
                    day = Mathf.Clamp(day + 1, 0, days.Count);
                    SceneManager.LoadScene(endGameScene);
                });
            }
            else
            {
                DOVirtual.Float(1f, 0f, vignetteTime, (eye) =>
                {
                    fullscreenVignette.SetFloat("_EyesClosed", eye);
                    fullscreenVignette.SetFloat("_Smoothness", eye);
                }).OnComplete(() =>
                {
                    day = Mathf.Clamp(day + 1, 0, days.Count);
                    SceneManager.LoadScene(endDayIndex);
                });
            }
        }
        else
        {
            StartSheetSorting();
            endShiftButton.interactable = false;
            endShiftText.text = "End Shift";
            endShiftImage.color = Color.gray;
        }
    }
}
