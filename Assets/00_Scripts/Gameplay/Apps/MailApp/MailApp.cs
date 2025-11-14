using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MailApp : MonoBehaviour, IApp, ISingletonMonobehavior
{
    [SerializeField] Mail mailPrefab;
    [SerializeField] RectTransform contentAncor;
    [SerializeField] RectTransform mailViewAncor;
    [SerializeField] WindowAnimation mailWindow;
    [SerializeField] GameplayEventSender gameplayEvents;
    [HideInInspector] public List<Mail> bin = new();

    MailView current;
    bool toBeDestroyed;

    private void Start()
    {
        gameplayEvents.OnSendMail += ReiceiveMail;
    }

    public void ReiceiveMail(MailData mail)
    {

    }

    public void OpenMail(MailData mail)
    {
        GameObject cash = null;
        if (current != null)
        {
            cash = current.gameObject;
        }
        //current = Instantiate(mail, mailViewAncor);
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

    public void Open()
    {

    }
}
