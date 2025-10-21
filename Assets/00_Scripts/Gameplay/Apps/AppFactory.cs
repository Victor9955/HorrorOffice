using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class AppFactory : MonoBehaviour, ISingletonMonobehavior
{
    [SerializeField] List<InterfaceReference<IApp>> apps = new();
    Dictionary<Type, IApp> appsByType = new();
    Dictionary<IApp, int> notificationNum = new();

    private void Awake()
    {
        foreach (var app in apps)
        {
            if(app.UnderlyingValue == null) continue;
            appsByType.Add(app.Value.GetType(), app.Value);
            notificationNum.Add(app.Value, 0);
        }
    }

    public T GetApp<T>() where T : IApp
    {
        bool doGetValue = appsByType.TryGetValue(typeof(T), out IApp value);
        Debug.Assert(doGetValue);
        if (doGetValue)
        {
            return (T) value;
        }
        else
        {
            return default(T);
        }
    }

    public void Notification<T>() where T : IApp
    {
        if (appsByType.TryGetValue(typeof(T), out IApp value))
        {
            if (notificationNum.ContainsKey(value))
            {
                notificationNum[value]++;
                value.Notification();
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Singleton.Instance<AppFactory>().Notification<SearchingApp>();
        }
    }
}
