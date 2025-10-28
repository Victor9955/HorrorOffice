using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

public class WindowAnimation : MonoBehaviour
{
    [SerializeField] GameObject toShow;
    public UnityEvent OpenEvent;
    public UnityEvent CloseEvent;
    [HideInInspector] public bool isClosed = false;

    public void Open()
    {
        toShow.SetActive(true);
        transform.DOScale(1f, 0.3f).OnComplete(() =>
        {
            OpenEvent?.Invoke();
            isClosed = false;
        });
    }

    public void Close()
    {
        transform.DOScale(0f, 0.3f).OnComplete(() =>
        {
            CloseEvent?.Invoke();
            toShow.SetActive(false);
            isClosed = true;
        });
    }
}
