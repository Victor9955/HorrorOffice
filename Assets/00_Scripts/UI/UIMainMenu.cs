using DG.Tweening;
using FMODUnity;
using HuntroxGames.Utils;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField, UnityEngine.Range(0f, 100f)] private float _initOffPerc;
    [Space]
    [SerializeField] private float _openDistance;
    [SerializeField] private float _openDuration;
    [SerializeField] private float _delay;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private Material eyes;
    [Header("Day Transition")]
    [SerializeField] private bool _isCinematic;
    [SerializeField] private TMP_Text _nextShiftText;
    [SerializeField] private int _sceneToTransitionIndex;
    [Space]
    [SerializeField] private float _textAppeareanceDelay;
    [SerializeField] private float _fadeInDelay;
    [SerializeField] private float _textFadeOutDuration;

    [SerializeField] private EventReference _nextShiftSound;
    [SerializeField] private EventReference _doorsOpeningSound;

    private int _sceneIndex;
    private float _initLightIntens;
    private float _initOffIntens;
    private float _initAlpha;

    private void Start()
    {
        _initAlpha = _fadeImg.color.a;

        // Eyeshader setup
        eyes.SetFloat("_EyesClosed", 1f);
        eyes.SetFloat("_Smoothness", 1f);

        //Light setup
        if (_light != null)
        {
            _initLightIntens = _light.intensity;
            _initOffIntens = _light.intensity * (_initOffPerc * 0.01f);
            _light.intensity = _initOffIntens;
        }


        // DayTransition
        if (_isCinematic)
        {
            _fadeImg.gameObject.SetActive(true);
            DOVirtual.DelayedCall(_textAppeareanceDelay, () =>
            {
                _nextShiftText.gameObject.SetActive(true);
                RuntimeManager.PlayOneShot(_nextShiftSound);

            });

            DOVirtual.DelayedCall(_textAppeareanceDelay + _fadeInDelay, () =>
            {
                DOVirtual.Float(1, 0, _fadeDuration, (a) => _fadeImg.SetAlpha(a))
                    .OnComplete(() => FadeToScene(_sceneToTransitionIndex));
            });
            DOVirtual.DelayedCall(_textFadeOutDuration, () =>
            {
                // Text
                DOVirtual.Float(1f, 0f, _textFadeOutDuration, (a) => _nextShiftText.SetAlpha(a))
                        .OnComplete(() => _nextShiftText.gameObject.SetActive(false));

            });
        }

        // Fade Setup
        if (!_isCinematic)
        {
            _fadeImg.gameObject.SetActive(true);
            DOVirtual.Float(_initAlpha, 0f, _fadeDuration, (a) =>
            {
                _fadeImg.SetAlpha(a);
            })
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => _fadeImg.gameObject.SetActive(false));

        }
    }
    public void FadeToScene(int index)
    {
        DOVirtual.DelayedCall(_textFadeOutDuration, () =>
        {
            _sceneIndex = index;

            //Light
            if (_light != null) DOVirtual.Float(_initOffIntens, _initLightIntens, _openDuration, (intens) => _light.intensity = intens);


            //Doors
            _leftDoorTR.DOMove(_leftDoorTR.position + (_leftDoorTR.up * _openDistance), _openDuration);
            _rightDoorTR.DOMove(_rightDoorTR.position + (-_rightDoorTR.up * _openDistance), _openDuration)

                // Fade
                .OnComplete(SceneFade);
        });
    }

    public void ResetDayCounter()
    {
        LevelSender.day = 0;
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

    public void QuitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}