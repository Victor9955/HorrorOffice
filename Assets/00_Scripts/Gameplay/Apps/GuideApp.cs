using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GuideApp : MonoBehaviour, IApp
{
    [SerializeField] GameplayEventSender gameplayEventSender;
    [SerializeField] List<GameObject> toDesactivate;
    private void Awake()
    {
        gameplayEventSender.DesactivateGuideEvent += DesactivateGuideEvent;
    }

    private void OnDestroy()
    {

        gameplayEventSender.DesactivateGuideEvent -= DesactivateGuideEvent;
    }

    private void DesactivateGuideEvent()
    {
        toDesactivate.ForEach((g) => g.gameObject.SetActive(false));
    }

    public void Close()
    {
            
    }

    public void Open()
    {

    }
}
