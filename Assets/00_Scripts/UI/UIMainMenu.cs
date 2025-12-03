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
    [SerializeField] private List<Light> _lights = new();
    [Space]
    [SerializeField] private float _openDistance;
    [SerializeField] private float _openDuration;

    private int _sceneIndex;
    private List<float> _initLightIntens = new();

    private void Start()
    {
        for (int i = 0; i < _lights.Count; i++)
        {
            _initLightIntens.Add(_lights[i].intensity);
            _lights[i].intensity = 0;
        }


    }
    public void StartButton(int index)
    {
        _sceneIndex = index;
        _leftDoorTR.DOMove(_leftDoorTR.position + (_leftDoorTR.up * _openDistance), _openDuration);
        _rightDoorTR.DOMove(_rightDoorTR.position + (-_rightDoorTR.up * _openDistance), _openDuration);
            DOVirtual.Float(0, _initLightIntens[0], _openDuration, (intens) => _lights[0].intensity = intens);
            DOVirtual.Float(0, _initLightIntens[0], _openDuration, (intens) => _lights[1].intensity = intens);

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
