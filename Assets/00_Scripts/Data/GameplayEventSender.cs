using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MailSender", menuName = "Scriptable Objects/MailSender")]
public class GameplayEventSender : ScriptableObject
{
    public Action<MailData> OnSendMail;
    public void SendMail(MailData mail) => OnSendMail?.Invoke(mail);

    [SerializeField] Vector2 mailRandomWait;

    public async void SendMailWait(MailData mail)
    {
        await Awaitable.WaitForSecondsAsync(UnityEngine.Random.Range(mailRandomWait.x, mailRandomWait.y));
        SendMail(mail);
    }
}
