using HuntroxGames.Utils;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSender : MonoBehaviour
{
    [SerializeField,Required] LevelCreator levelCreator;

    [SerializeField] List<DayData> days;

    int day;
    DayData current;

    [ConsoleCommand("BeginDay", "[Integer Input]")]
    public void BeginDay(int m_day)
    {
        if (current == null) // when current = null current level is finished
        {
            day = m_day;
            current = days[day];
            StartCoroutine(PlayLevel());
        }
    }

    IEnumerator PlayLevel()
    {
        foreach (var levelAction in current.actions)
        {
            yield return new WaitUntil(() => levelAction.beginCondition);
            levelCreator.CreateLevel(levelAction);
            yield return new WaitUntil(() => levelCreator.isCreated);
            StartCoroutine(levelCreator.Play());
            yield return new WaitUntil(() => levelCreator.isFinished);
            StartCoroutine(levelCreator.End());
            yield return new WaitUntil(() => levelCreator.isEnded);
        }
        current = null;
    }
}
