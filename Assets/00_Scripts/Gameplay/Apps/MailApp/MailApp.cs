using DG.Tweening;
using HuntroxGames.Utils;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MailApp : MonoBehaviour, IApp
{
    [SerializeField] Mail mailPrefab;
    [SerializeField] MailView mailView;
    [SerializeField] RectTransform mailViewAncor;
    [SerializeField] RectTransform notification;
    [SerializeField] GameplayEventSender gameplayEvents;
    [SerializeField] FMODUnity.EventReference _mailNotificationSound;
    [SerializeField] List<LayoutGroup> layouts;

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

            foreach(var layout in layouts)
            {
                layout.CalculateLayoutInputHorizontal();
                layout.CalculateLayoutInputVertical();
            }
        }
    }

    public void Close()
    {

    }

    public void Open()
    {

    }
}
