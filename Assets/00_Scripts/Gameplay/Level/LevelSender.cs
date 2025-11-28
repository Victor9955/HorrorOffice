using HuntroxGames.Utils;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSender : MonoBehaviour
{
    [SerializeField,Required] LevelCreator levelCreator;
    [SerializeField,Required] FileSorting fileSorting;
    [SerializeField] private Vector2 randomWaitTimeForCharacter;
    [SerializeField] List<DayData> days;

    int day;
    DayData current;

    public event Action<DayData> OnBeginDay;
    public event Action OnEndDay;
    public event Action OnEndGame;

    [ConsoleCommand("BeginDay", "[Integer Input]")]
    public void BeginDay(int m_day)
    {
        if(m_day > days.Count -1 || m_day < 0) return;
        if (current == null) // when current = null current level is finished
        {
            day = m_day;
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
        current = null;
        if(day == days.Count -1)
        {
            OnEndGame?.Invoke();
        }
        else
        {
            OnEndDay?.Invoke();
        }
    }
}
