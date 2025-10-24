using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSender : MonoBehaviour, ISingletonMonobehavior
{
    [SerializeField,Required] LevelCreator levelCreator;

    [SerializeField] List<LevelData> levels;

    int day;
    [SerializeField] int testDay = 0;

    LevelData current;

    [Button]
    void TestDay() => SetDay(testDay);

    public void SetDay(int m_day)
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
        foreach (var level in current.level)
        {
            levelCreator.CreateLevel(level);
            yield return new WaitUntil(() => levelCreator.isCreated);
            levelCreator.Play();
            yield return new WaitUntil(() => levelCreator.isFinished);
            levelCreator.End();
            yield return new WaitUntil(() => levelCreator.isEnded);

            // TODO Rajouter conditons pour nouveau perso
        }
        current = null;
    }
}
