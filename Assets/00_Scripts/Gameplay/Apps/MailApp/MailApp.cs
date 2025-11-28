using DG.Tweening;
using HuntroxGames.Utils;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MailApp : MonoBehaviour, IApp
{
    [SerializeField] Mail mailPrefab;
    [SerializeField] MailView mailView;
    [SerializeField] RectTransform mailViewRect;
    [SerializeField] RectTransform mailViewAncor;
    [SerializeField] RectTransform notification;
    [SerializeField] GameplayEventSender gameplayEvents;
    [SerializeField] FMODUnity.EventReference _mailNotificationSound;

    Dictionary<Mail, MailData> reiceivedMail = new();

    Tween notifTween;
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
        if(notifTween == null)
        {
            notifTween = notification.DOShakeRotation(0.25f, Vector3.forward * 20f);
            notifTween.SetLoops(-1);
        }
        if(!_mailNotificationSound.IsNull)
        {
            FMODUnity.RuntimeManager.PlayOneShot(_mailNotificationSound, transform.position);
        }
    }

    public void OpenMail(Mail mail)
    {
        if(reiceivedMail.TryGetValue(mail, out MailData mailCash))
        {
            int seenMail = reiceivedMail.Keys.Where((m) => m.wasOpened).Count();
            if(seenMail == 0)
            {
                notifTween.Complete();
                notifTween.Kill();
            }
            mailView.Show(mailCash);
            RecalculateSize();
        }
    }

    void RecalculateSize()
    {
        float size = 0f;

        foreach(RectTransform rectT in mailViewRect)
        {
            size += rectT.sizeDelta.y;
        }
        Vector2 rec = mailViewRect.sizeDelta;
        rec.y = size;
        mailViewRect.sizeDelta = rec;
    }

    public void Close()
    {

    }

    public void Open()
    {

    }
}
