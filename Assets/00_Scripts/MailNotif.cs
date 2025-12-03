using System;
using UnityEngine;

public class MailNotif : MonoBehaviour
{
    [SerializeField] TMPInput input;
    [SerializeField] GameplayEventSender gameplayEventSender;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameplayEventSender.OnSendMail += ReceiveMail;
    }

    private void OnDestroy()
    {
        gameplayEventSender.OnSendMail -= ReceiveMail;
    }

    private void ReceiveMail(MailData data)
    {
        input.input.text = input.before + data.character.staticInfo.name;
    }
}
