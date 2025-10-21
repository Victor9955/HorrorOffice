using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class NoteApp : MonoBehaviour, IApp
{
    [SerializeField] Scrollbar scrollbar;
    public void Close()
    {
            
    }

    public void Open()
    {
        scrollbar.value = 1;
    }
}
