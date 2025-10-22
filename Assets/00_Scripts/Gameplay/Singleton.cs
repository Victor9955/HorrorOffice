using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISingletonMonobehavior
{

}

public class Singleton : MonoBehaviour
{
    static Dictionary<Type,ISingletonMonobehavior> singletons = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        foreach (var singleton in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            ISingletonMonobehavior cash = singleton as ISingletonMonobehavior;
            if (cash != null)
            {
                if(singletons.ContainsKey(singleton.GetType()))
                {
                    Debug.LogAssertion("Singleton " + singleton.GetType().Name + " is no alone please kill the other one");
                    continue;
                }
                singletons.Add(singleton.GetType(), cash);
                DontDestroyOnLoad(singleton);
            }
        }
        SceneManager.sceneLoaded += SceneLoaded;
        Application.quitting += OnQuit;
    }

    private static void OnQuit()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        Application.quitting -= OnQuit;
    }

    private static void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        foreach (var singleton in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            ISingletonMonobehavior cash = singleton as ISingletonMonobehavior;
            if (cash != null)
            {
                if (singletons.ContainsKey(singleton.GetType()))
                {
                    Debug.LogAssertion("Singleton " + singleton.GetType().Name + " is initiated in other Scene please kill the other one");
                    continue;
                }
                singletons.Add(singleton.GetType(), cash);
                DontDestroyOnLoad(singleton);
            }
        }
    }

    public static T Instance<T>() where T : ISingletonMonobehavior
    {
        if(singletons.TryGetValue(typeof(T), out ISingletonMonobehavior value))
        {
            return (T)singletons[typeof(T)];
        }
        Debug.LogAssertion("No Singleton on Type " + typeof(T).Name);

        return default(T);
    }
}
