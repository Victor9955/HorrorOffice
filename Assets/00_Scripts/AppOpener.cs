using AYellowpaper;
using DG.Tweening;
using System;
using UnityEngine;

public class AppOpener : MonoBehaviour
{
    [SerializeField] GameObject toShow;
    [SerializeField] RectTransform parent;
    [SerializeField] InterfaceReference<IApp> app;

    public void Open()
    {
        toShow.SetActive(true);
        parent.DOScale(1f, 0.3f);
        app.Value.Open();
    }

    public void Close()
    {
        parent.DOScale(0f, 0.3f).OnComplete(() => toShow.SetActive(false));
        app.Value.Close();
    }
}
