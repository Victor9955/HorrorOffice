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
        MailData current = mail;
        do
        {
            if (current.hasDelay)
            {
                if (current.hasRandomDelay)
                {
                    await Awaitable.WaitForSecondsAsync(UnityEngine.Random.Range(current.delay, current.maxDelay));
                }
                else
                {
                    await Awaitable.WaitForSecondsAsync(current.delay);
                }
            }
            OnSendMail?.Invoke(current);
            current = current.nextMail;
        }
        while (current != null);
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
