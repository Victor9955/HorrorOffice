using DG.Tweening;
using HuntroxGames.Utils;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private Transform _leftDoorTR;
    [SerializeField] private Transform _rightDoorTR;
    [SerializeField] private Light _light;
    [SerializeField] private Image _fadeImg;
    [Space(10)]
    [Space(10)]
    [Header("Anim Settings")]
    [SerializeField] private float _openDistance;
    [SerializeField] private float _openDuration;
    [SerializeField] private float _delay;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private Material eyes;

    [Header("Hanged Settings")]
    [Space(10)]
    [SerializeField] private GameObject _hangedObj;
    [SerializeField] private bool _isLiHere;

    private int _sceneIndex;
    private float _initLightIntens;
    private float _initAlpha;

    private void Start()
    {
        //Light setup
        if (_light != null)
        {
            _initLightIntens = _light.intensity;
            _light.intensity = 0;
        }

        // Fade Setup
        _fadeImg.gameObject.SetActive(true);
        _initAlpha = _fadeImg.color.a;
        DOVirtual.Float(_initAlpha, 0f, _fadeDuration, (a) =>
        {
            _fadeImg.SetAlpha(a);
        })
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => _fadeImg.gameObject.SetActive(false));
        eyes.SetFloat("_EyesClosed", 1f);
        eyes.SetFloat("_Smoothness", 1f);


        //Li
        if (_isLiHere && _hangedObj != null)
        {
            _hangedObj.SetActive(true);
            Hang();
        }
    }
    public void FadeToScene(int index)
    {
        _sceneIndex = index;

        //Light
        if (_light != null) DOVirtual.Float(0, _initLightIntens, _openDuration, (intens) => _light.intensity = intens);

        //Doors
        _leftDoorTR.DOMove(_leftDoorTR.position + (_leftDoorTR.up * _openDistance), _openDuration);
        _rightDoorTR.DOMove(_rightDoorTR.position + (-_rightDoorTR.up * _openDistance), _openDuration)

            // Fade
            .OnComplete(SceneFade);
    }

    private void SceneFade()
    {
        _fadeImg.gameObject.SetActive(true);
        DOVirtual.Float(0f, _initAlpha, _fadeDuration, (a) =>
        {
            _fadeImg.SetAlpha(a);
        })
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => SceneManager.LoadScene(_sceneIndex)).SetDelay(_delay);
    }


    private void Hang()
    {
        return;
    }
    public void QuitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}