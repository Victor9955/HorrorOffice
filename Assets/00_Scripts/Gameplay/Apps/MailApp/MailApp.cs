using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MailApp : MonoBehaviour, IApp, ISingletonMonobehavior
{
    [SerializeField] Mail mailPrefab;
    [SerializeField] MailRef mails;
    [SerializeField] RectTransform contentAncor;
    [SerializeField] RectTransform bg;
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
        ReiceiveMail(1);
    }

    public void ReiceiveMail(int id)
    {
        Mail cash = Instantiate(mailPrefab, contentAncor.transform);
        cash.mailId = id;
    }

    public void OpenMail(int id)
    {
        // Look for Mail in Scriptable object with all mails
        if(id < 0 && id > mails.mailsPrefab.Count)
        {
            Debug.LogAssertion("Wrong Mail ID Sended");
        }
        else
        {
            Instantiate(mails.mailsPrefab[id], bg);
        }
    }

    public void Close()
    {

    }
}
