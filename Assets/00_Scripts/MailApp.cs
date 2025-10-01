using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MailApp : MonoBehaviour, IApp
{
    [SerializeField] GameObject toShow;
    [SerializeField] RectTransform parent;
    [SerializeField] Mail mailPrefab;
    [SerializeField] RectTransform contentAncor;


    private void Start()
    {
        if (parent == null)
        {
            parent = GetComponent<RectTransform>();
        }
    }

    public void Open()
    {
        parent.DOScale(1f, 0.3f);
        toShow.SetActive(true);
    }
    public void Close()
    {
        parent.DOScale(0f, 0.3f).OnComplete(() => toShow.SetActive(false));
    }

    [Button]
    void TestReceiveMail()
    {

    }
}
