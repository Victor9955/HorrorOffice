using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Transform _leftDoorTR;
    [SerializeField] private Transform _rightDoorTR;
    [SerializeField] private Light _light;
    [Space]
    [SerializeField] private float _openDistance;
    [SerializeField] private float _openDuration;

    private int _sceneIndex;
    private float _initLightIntens;

    private void Start()
    {
            _initLightIntens = _light.intensity;
            _light.intensity = 0;


    }
    public void StartButton(int index)
    {
        _sceneIndex = index;
        _leftDoorTR.DOMove(_leftDoorTR.position + (_leftDoorTR.up * _openDistance), _openDuration);
        _rightDoorTR.DOMove(_rightDoorTR.position + (-_rightDoorTR.up * _openDistance), _openDuration);
            DOVirtual.Float(0, _initLightIntens, _openDuration, (intens) => _light.intensity = intens);

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
