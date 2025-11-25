using DG.Tweening;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DayChanger : MonoBehaviour
{
    [SerializeField] LevelSender levelSender;
    [SerializeField] Volume volume;
    [SerializeField] float vignetteTime = 0.25f;
    [SerializeField] bool beginFirstDay;

    Vignette vignette = null;

    static int dayNum;

    void Start()
    {
        volume.profile.TryGet<Vignette>(out vignette);
        levelSender.OnEndDay += OnEndDay;
        if(beginFirstDay)
        {
            dayNum = 0;
        }
        levelSender.BeginDay(dayNum);
        dayNum++;
    }

    [Button]
    private void OnEndDay()
    {
        if(vignette != null)
        {
            vignette.intensity.max = 1000f;
            DOTween.To(() => vignette.intensity.value, (i) => vignette.intensity.value = i, vignette.intensity.max, vignetteTime).OnComplete(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }
    }
}
