using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class MailApp : MonoBehaviour, IApp, ISingletonMonobehavior
{
    [SerializeField] Mail mailPrefab;
    [SerializeField] RectTransform contentAncor;
    [SerializeField] RectTransform mailViewAncor;
    [SerializeField] WindowAnimation mailWindow;
    [HideInInspector] public List<Mail> bin = new();

    MailView current;
    bool toBeDestroyed;

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
        ReiceiveMail(0);
    }

    public void ReiceiveMail(int id)
    {
        Mail cash = Instantiate(mailPrefab, contentAncor.transform);
        cash.mailId = id;
    }

    public void OpenMail(MailView mail)
    {
        GameObject cash = null;
        if (current != null)
        {
            cash = current.gameObject;
        }
        current = Instantiate(mail, mailViewAncor);
        current.myWindow = mailWindow;
        mailWindow.Open();
        if (toBeDestroyed && cash != null)
        {
            Destroy(cash);
            toBeDestroyed = false;
        }
    }

    public void Close()
    {

    }

    public void CloseCurrentMail()
    {
        toBeDestroyed = true;
    }
}
