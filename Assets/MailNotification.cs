using DG.Tweening;
using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine;

public class MailNotification : MonoBehaviour
{
    [SerializeField] GameplayEventSender gameplayEvents;
    [SerializeField] TMPInput nameTMP;
    [SerializeField] RectTransform ancor;
    void Start()
    {
        ancor.gameObject.SetActive(false);
        gameplayEvents.OnSendMail += ReceiveMail;
    }

    private void OnDestroy()
    {
        gameplayEvents.OnSendMail -= ReceiveMail;
    }
    private void ReceiveMail(MailData data)
    {
        ancor.gameObject.SetActive(true);
        ancor.anchoredPosition = new Vector2(150, ancor.anchoredPosition.y);
        ancor.DOAnchorPosX(-100, 2f).SetEase(Ease.InExpo);
        nameTMP.input.text = nameTMP.before + data.character.staticInfo.name;
    }
}
