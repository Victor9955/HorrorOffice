using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MailApp : MonoBehaviour, IApp
{
    [SerializeField] Mail mailPrefab;
    [SerializeField] RectTransform contentAncor;
    [HideInInspector] public List<Mail> bin = new();

    public void Open()
    {
        while (bin.Count > 0)
        {
            bin[0].UnCheck();
            bin.RemoveAt(0);
        }
    }

    public void Delete()
    {
        while (bin.Count > 0)
        {
            bin[0].Delete();
            bin.RemoveAt(0);
        }
    }

    [Button]
    void TestReceiveMail()
    {
        Mail cash = Instantiate(mailPrefab, contentAncor.transform);
        cash.appRef = this;
    }

    public void Close()
    {

    }
}
