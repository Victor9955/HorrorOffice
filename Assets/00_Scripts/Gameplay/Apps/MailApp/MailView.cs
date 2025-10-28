using System.Collections;
using UnityEngine;

public class MailView : MonoBehaviour
{
    [HideInInspector] public WindowAnimation myWindow;

    public void Close()
    {
        myWindow.Close();
    }
}
