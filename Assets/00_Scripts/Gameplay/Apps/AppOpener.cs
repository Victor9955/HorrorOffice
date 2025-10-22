using DG.Tweening;
using System;
using UnityEngine;

public class AppOpener : MonoBehaviour
{
    [SerializeField] GameObject toShow;
    [SerializeField] InterfaceReference<IApp> app;

    public void Open()
    {
        toShow.SetActive(true);
        transform.DOScale(1f, 0.3f);
        app.Value.Open();
    }

    public void Close()
    {
        transform.DOScale(0f, 0.3f).OnComplete(() => toShow.SetActive(false));
        app.Value.Close();
    }
}
