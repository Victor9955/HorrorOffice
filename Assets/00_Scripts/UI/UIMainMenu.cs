using DG.Tweening;
using HuntroxGames.Utils;
using System;
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
    [Space]
    [Header("Anim Settings")]
    [SerializeField,Range(0f,1f)] private float _offIntensityCoef = 1f;
    [SerializeField] private float _openDistance;
    [SerializeField] private float _openDuration;
    [SerializeField] private float _fadeDuration;

    private int _sceneIndex;
    private float _initLightIntens;
    private float _initAlpha;

    private void Start()
    {
        //Light setup
        if (_light != null)
        {
            _initLightIntens = _light.intensity;
            _light.intensity *= _offIntensityCoef;
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

    }
    public void FadeToScene(int index)
    {
        _sceneIndex = index;

        //Light
        float currentIntens = _light.intensity;
        if (_light != null) DOVirtual.Float(currentIntens, _initLightIntens, _openDuration, (intens) => _light.intensity = intens);

        //Doors
        _leftDoorTR.DOMove(_leftDoorTR.position + (_leftDoorTR.up * _openDistance), _openDuration);
        _rightDoorTR.DOMove(_rightDoorTR.position + (-_rightDoorTR.up * _openDistance), _openDuration)
            
            // Fade
            .OnComplete(FadeToScene);
    }

    private void FadeToScene()
    {
        _fadeImg.gameObject.SetActive(true);
        DOVirtual.Float(0f, _initAlpha, _fadeDuration, (a) =>
        {
            _fadeImg.SetAlpha(a);
        })
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => SceneManager.LoadScene(_sceneIndex));
    }

    public void QuitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}