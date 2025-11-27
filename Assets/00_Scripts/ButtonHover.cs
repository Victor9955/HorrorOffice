using DG.Tweening;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] float size = 1.1f;
    [SerializeField] float time = 0.5f;

    public void EnterHover()
    {
        transform.DOScale(size, time);
    }

    public void ExitHover() 
    {
        transform.DOScale(1f,time);
    }
}
