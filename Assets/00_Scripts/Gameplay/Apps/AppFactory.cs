using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class AppFactory : MonoBehaviour, ISingletonMonobehavior
{
    [SerializeField] List<InterfaceReference<IApp>> apps = new();
    [SerializeField] Canvas cavna;
    Dictionary<Type, IApp> appsByType = new();

    private void Awake()
    {
        foreach (var app in apps)
        {
            appsByType.Add(app.Value.GetType(), app.Value);
        }
    }

    private void Start()
    {
        foreach (var app in apps)
        {
            app.Value.notificationEvent += Value_notificationEvent;
        }

    }

    private void Value_notificationEvent(int obj)
    {

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }
    }
}
