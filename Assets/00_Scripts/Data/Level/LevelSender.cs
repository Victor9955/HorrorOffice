using HuntroxGames.Utils;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSender : MonoBehaviour
{
    [SerializeField,Required] LevelCreator levelCreator;

    [SerializeField] List<LevelData> levels;

    int day;
    LevelData current;

    [ConsoleCommand("BeginDay", "[Integer Input]")]
    public void BeginDay(int m_day)
    {
        if (current == null) // when current = null current level is finished
        {
            day = m_day;
            current = levels[day];
            StartCoroutine(PlayLevel());
        }
    }

    IEnumerator PlayLevel()
    {
        foreach (var levelAction in current.level)
        {
            levelCreator.CreateLevel(levelAction);
            yield return new WaitUntil(() => levelCreator.isCreated);
            StartCoroutine(levelCreator.Play());
            yield return new WaitUntil(() => levelCreator.isFinished);
            levelCreator.End();
            yield return new WaitUntil(() => levelCreator.isEnded);

            //yield return new WaitUntil(() => DoGetNextCharacter());
        }
        current = null;
    }
    bool DoGetNextCharacter()
    {
        return false;
    }
}
