using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MailSender", menuName = "Scriptable Objects/MailSender")]
public class GameplayEventSender : ScriptableObject
{
    public event Action<MailData> OnSendMail;
    public event Action<int> AddHelpfulnessEvent;
    public event Action<int> AddTrustworthinessEvent;
    public event Action<int> AddAuthenticityEvent;
    public event Action<int> AddEmpathyEvent;
    public event Action DesactivateDataBaseEvent;
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

    public void AddHelpfulness(int amount) => AddHelpfulnessEvent?.Invoke(amount);

    public void LoadScene(int index) => SceneManager.LoadScene(index);

    public void AddTrustworthiness(int amount) => AddTrustworthinessEvent?.Invoke(amount);
    public void AddAuthenticity(int amount) => AddAuthenticityEvent?.Invoke(amount);
    public void AddEmpathy(int amount) => AddEmpathyEvent?.Invoke(amount);

    public void DesactivateDataBase() => DesactivateDataBaseEvent?.Invoke();
}
