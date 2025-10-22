using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

public class WindowAnimation : MonoBehaviour
{
    [SerializeField] GameObject toShow;
    [SerializeField] UnityEvent OpenEvent;
    [SerializeField] UnityEvent CloseEvent;

    public void Open()
    {
        toShow.SetActive(true);
        transform.DOScale(1f, 0.3f).OnComplete(() =>
        {
            OpenEvent?.Invoke();
        });
    }

    public void Close()
    {
        transform.DOScale(0f, 0.3f).OnComplete(() => toShow.SetActive(false));
        CloseEvent?.Invoke();
    }
}
