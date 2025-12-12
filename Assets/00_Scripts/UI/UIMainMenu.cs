using DG.Tweening;
using HuntroxGames.Utils;
using System.Collections;
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
    [SerializeField] private TMP_Text _nextShiftText;
    [Space(10)]
    [Space(10)]
    [Header("Anim Settings")]
    [SerializeField] private bool _isCinematic;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _openDistance;
    [SerializeField, Range(0, 100f)] private float _offLightIntensPerc;
    [SerializeField] private float _openDuration;
    [SerializeField] private float _delay;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private Material eyes;
    [Space]
    [Header("Flicker Settings")]
    [SerializeField] private float _flickerDelay;
    [SerializeField, Range(0, 100)] private int _flickerTurnBackOnChance;
    [SerializeField] private int _flickerRerollBuff;

    [Header("Hanged Settings")]
    [Space(10)]
    [SerializeField] private GameObject _hangedObj;
    [SerializeField] private bool _isLiHere;

    private int _sceneIndex;
    private float _initLightIntens;
    private float _initAlpha;
    private float _offLightIntens;
    private int _rerollCount;
    private void Start()
    {
        //Light setup
        if (_light != null)
        {
            _initLightIntens = _light.intensity;
            float offLightIntens = _initLightIntens * (_offLightIntensPerc / 100f);
            _light.intensity = _offLightIntens;

        }

        // Fade Setup
        _fadeImg.gameObject.SetActive(true);
        _initAlpha = _fadeImg.color.a;
        DOVirtual.Float(_initAlpha, 0f, _fadeDuration, (a) =>
        {
            _fadeImg.SetAlpha(a);
        })
        .SetEase(Ease.InOutQuad)
        .OnComplete(() =>
        {
            _fadeImg.gameObject.SetActive(false);
            if (_isCinematic)
            {
                Debug.Log("Cinematic : start day in " + _startDelay);
                DOVirtual.DelayedCall(_startDelay, () => FadeToScene(_sceneIndex));
            }
        });
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
        if (_light != null) DOVirtual.Float(_offLightIntens, _initLightIntens, _openDuration, (intens) => _light.intensity = intens);

        //Text fade
        DOVirtual.Float(1f, 0f, _openDuration, (a) => _nextShiftText.SetAlpha(a));

        // Light Flicker
        //StartCoroutine(FlickerRoutine(0));

        //Doors
        _leftDoorTR.DOMove(_leftDoorTR.position + (_leftDoorTR.up * _openDistance), _openDuration);
        _rightDoorTR.DOMove(_rightDoorTR.position + (-_rightDoorTR.up * _openDistance), _openDuration)
        // Fade
        .OnComplete(SceneFade);

    }
    private IEnumerator FlickerRoutine(int luckBonus)
    {
        int rerollLuck = _rerollCount == 0 ? 0 : luckBonus;
        _nextShiftText.SetAlpha(0f);
        yield return new WaitForSeconds(0.5f);
        bool doesTheLightTurnsOnAgain = Random.Range(0, 1f) < (_offLightIntensPerc + rerollLuck) * 0.1f;
        Debug.Log($"Turning off, ({(_offLightIntensPerc + rerollLuck)}% chance to turn on again)");
        if (doesTheLightTurnsOnAgain)
        {
            _rerollCount++;
            Debug.Log("Turning back on again");
            rerollLuck += _flickerRerollBuff;
            _nextShiftText.SetAlpha(1f);
            StartCoroutine(FlickerRoutine(rerollLuck));
        }
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