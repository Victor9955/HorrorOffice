using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Transform _leftDoorTR;
    [SerializeField] private Transform _rightDoorTR;
    [Space]
    [SerializeField] private float _openDistance;
    [SerializeField] private float _openDuration;
    private int _sceneIndex;

    public void StartButton(int index)
    {
        _sceneIndex = index;
        _leftDoorTR.DOMove(_leftDoorTR.position + (_leftDoorTR.up * _openDistance),_openDuration);
        _rightDoorTR.DOMove(_rightDoorTR.position + (-_rightDoorTR.up * _openDistance),_openDuration);
        Invoke("LaunchScene", 2);
    }

    private void LaunchScene() => SceneManager.LoadScene(_sceneIndex);
    

    public void QuitButton(int index)
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

}
