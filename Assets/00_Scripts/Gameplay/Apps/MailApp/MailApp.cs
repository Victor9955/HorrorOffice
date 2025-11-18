using DG.Tweening;
using HuntroxGames.Utils;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MailApp : MonoBehaviour, IApp, ISingletonMonobehavior
{
    [SerializeField] Mail mailPrefab;
    [SerializeField] MailView mailView;
    [SerializeField] RectTransform mailViewAncor;
    [SerializeField] Image notifiaction;
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
        notifiaction.enabled = true;
    }

    public void OpenMail(Mail mail)
    {
        if(reiceivedMail.TryGetValue(mail, out MailData mailCash))
        {
            int seenMail = reiceivedMail.Keys.Where((m) => m.wasOpened).Count();
            if(seenMail > 0)
            {
                notifiaction.enabled = false;
            }
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
