using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class Page
{
    public string pageName;
    public Transform pageTransform;
}

public class SearchingApp : MonoBehaviour, IApp
{
    [SerializeField] List<Transform> toShow;
    [SerializeField] List<Page> values;
    RectTransform rec;
    private void Start()
    {
        rec = GetComponent<RectTransform>();

    }


    public void Open()
    {
        rec.DOScale(1f, 0.3f);
        toShow.ForEach(show => show.gameObject.SetActive(true));
    }

    public void Close()
    {
        rec.DOScale(0f, 0.3f).OnComplete(() => toShow.ForEach(show => show.gameObject.SetActive(false)));
    }
}
