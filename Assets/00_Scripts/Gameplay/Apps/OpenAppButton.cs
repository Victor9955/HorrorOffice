using UnityEngine;

public class OpenAppButton : MonoBehaviour
{
    public IApp app;

    public void Open()
    {
        app.Open();
    }
}
