using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIMainMenu : MonoBehaviour
{


    public void StartButton(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void QuitButton(int index)
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

}
