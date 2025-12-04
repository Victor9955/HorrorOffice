using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MailSender", menuName = "Scriptable Objects/MailSender")]
public class GameplayEventSender : ScriptableObject
{
    public Action<MailData> OnSendMail;
    public Action AddHelpFullness;
    public async void SendMail(MailData mail)
    {
        if(mail.hasDelay)
        {
            if(mail.hasRandomDelay)
            {
                await Awaitable.WaitForSecondsAsync(UnityEngine.Random.Range(mail.delay, mail.maxDelay));
            }
            else
            {
                await Awaitable.WaitForSecondsAsync(mail.delay);
            }
        }
        OnSendMail?.Invoke(mail);
    }

    public void AddHelpfullness()
    {
        AddHelpFullness?.Invoke();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
