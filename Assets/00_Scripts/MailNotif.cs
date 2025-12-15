using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailNotif : MonoBehaviour
{
    [SerializeField] TMPInput input;
    [SerializeField] float notificationTime;
    [SerializeField] GameplayEventSender gameplayEventSender;
    [SerializeField] RectTransform ancor;
    [SerializeField] MailApp mailApp;
    [SerializeField] WindowAnimation windowMail;
    [SerializeField] List<WindowAnimation> appToClose;
    
    float ancorPosX;
    MailData current;

    Queue<MailData> queuedMail = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameplayEventSender.OnSendMail += ReceiveMail;
        ancorPosX = ancor.anchoredPosition.x;
    }

    private void OnDestroy()
    {
        gameplayEventSender.OnSendMail -= ReceiveMail;
    }

    private void ReceiveMail(MailData data)
    {
        queuedMail.Enqueue(data);
        if (!isShowing)
        {
            ShowMail(queuedMail.Dequeue());
        }
    }

    bool isShowing = false;
    void ShowMail(MailData mail)
    {
        current = mail;
        isShowing = true;
        ancor.gameObject.SetActive(true);
        ancor.anchoredPosition = new Vector2(ancorPosX + 250, ancor.anchoredPosition.y);
        ancor.DOAnchorPosX(ancorPosX, 0.5f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            DOVirtual.DelayedCall(notificationTime, () =>
            {
                ancor.DOAnchorPosX(ancorPosX + 250, 0.5f).SetEase(Ease.InExpo).OnComplete(() =>
                {
                    ancor.gameObject.SetActive(false);
                    if (queuedMail.Count > 0)
                    {
                        ShowMail(queuedMail.Dequeue());
                    }
                    else
                    {
                        isShowing = false;
                    }
                });
            });
        });
        input.input.text = input.before + mail.character.staticInfo.name;
    }

    public void OpenMail()
    {
        windowMail.Open();
        appToClose.ForEach((window) => window.Close());
        mailApp.OpenMail(current);
    }
}
