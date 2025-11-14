using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MailApp : MonoBehaviour, IApp, ISingletonMonobehavior
{
    [SerializeField] Mail mailPrefab;
    [SerializeField] MailView mailView;
    [SerializeField] RectTransform mailViewAncor;
    [SerializeField] GameplayEventSender gameplayEvents;

    Dictionary<Mail, MailData> reiceivedMail = new();


    private void Start()
    {
        gameplayEvents.OnSendMail += ReiceiveMail;
    }

    private void OnDestroy()
    {
        gameplayEvents.OnSendMail -= ReiceiveMail;
    }

    void ReiceiveMail(MailData mailData)
    {
        Mail mailCash = Instantiate(mailPrefab,mailViewAncor);
        reiceivedMail.Add(mailCash,mailData);
        mailCash.mailData = mailData;
        mailCash.mailAppRef = this;
    }

    public void OpenMail(Mail mail)
    {
        if(reiceivedMail.TryGetValue(mail, out MailData mailCash))
        {
            mailView.Show(mailCash);
        }
    }

    public void Close()
    {

    }

    public void Open()
    {

    }
}
