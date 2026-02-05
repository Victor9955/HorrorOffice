using DG.Tweening;
using HuntroxGames.Utils;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ReadOnlyAttribute = HuntroxGames.Utils.ReadOnlyAttribute;

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

    public static int day;
    DayData current;

    public event Action<DayData> OnBeginDay;
    public event Action<DayData> OnInitDatabase;
    public event Action OnEndDay;
    public event Action OnEndGame;

    int endDayIndex = 0;
    int fileNum = 0;

    private void Start()
    {
        if(beginFirstDay)
        {
            day = 0;
        }
        BeginDay();
        fullscreenVignette.SetFloat("_EyesClosed", 1f);
        fullscreenVignette.SetFloat("_Smoothness", 1f);
        Singleton.Instance<GameManager>().OnFileSpawned += Add;
        fileSorting.OnFileDroppedEvent += Sub;
    }

    private void OnDestroy()
    {
        Singleton.Instance<GameManager>().OnFileSpawned -= Add;
        fileSorting.OnFileDroppedEvent -= Sub;

        fullscreenVignette.SetFloat("_EyesClosed", 1f);
        fullscreenVignette.SetFloat("_Smoothness", 1f);
    }

    void Add()
    {
        fileNum++;
    }

    void Sub(Binder binder,SheetData sheetData)
    {
        fileNum--;
    }

    public void BeginDay()
    {
        if (current == null) // when current = null current level is finished
        {
            current = days[day];
            fileSorting._binderDataList = current.binders;
            fileSorting.SetupBinders();
            endDayIndex = current.endDayScene;
            OnInitDatabase?.Invoke(current);
            current.initEvent?.Invoke();
        }
    }

    public void OnPCLogIn()
    {
        if(current != null)
        {
            current.startEvent?.Invoke();
        }
    }

    public void StartSheetSorting()
    {
        StartCoroutine(PlayLevel());
    }

    IEnumerator PlayLevel()
    {
        OnBeginDay?.Invoke(current);
        current.startFileSortingEvent?.Invoke();
        foreach (var levelAction in current.actions)
        {
            //yield return new WaitUntil(() => levelAction.beginCondition);
            levelCreator.CreateLevel(levelAction);
            yield return new WaitUntil(() => levelCreator.isCreated);
            StartCoroutine(levelCreator.Play());
            yield return new WaitUntil(() => levelCreator.isFinished);
            StartCoroutine(levelCreator.End());
            yield return new WaitUntil(() => levelCreator.isEnded);
            levelAction.finishedEvent?.Invoke();
            yield return new WaitForSeconds(UnityEngine.Random.Range(randomWaitTimeForCharacter.x, randomWaitTimeForCharacter.y));
        }

        yield return new WaitUntil(() => fileNum == 0);
        endShiftButton.interactable = true;
        endShiftImage.color = Color.red;
        current = null;
        OnEndDay?.Invoke();
    }

    public void EndShiftButtonCallback()
    {
        if(current == null)
        {
            endShiftButton.interactable = false;
            DOVirtual.Float(1f, 0f, vignetteTime, (eye) =>
            {
                fullscreenVignette.SetFloat("_EyesClosed", eye);
                fullscreenVignette.SetFloat("_Smoothness", eye);
            }).OnComplete(() =>
            {
                if(day == days.Count - 1)
                {
                    SceneManager.LoadScene(endGameScene);
                }
                else
                {
                    day = Mathf.Clamp(day + 1, 0, days.Count - 1);
                    SceneManager.LoadScene(endDayIndex);
                }
            });
        }
        else
        {
            StartSheetSorting();
            endShiftButton.interactable = false;
            endShiftText.text = "End Shift";
            endShiftImage.color = Color.gray;
        }
    }

    [ConsoleCommand("Force")]
    void ForceChangeDay()
    {
        day = Mathf.Clamp(day + 1, 0, days.Count);
        SceneManager.LoadScene(endDayIndex);
    }
}
