using DG.Tweening;
using HuntroxGames.Utils;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISceneTransitionTwoIThink : MonoBehaviour
{
    [SerializeField] private Image _fadeImg;
    [Space(10)]
    [Header("Settings")]
    [SerializeField] private float _fadeDuration;

    private float _initAlpha;
    private void Start()
    {
        _fadeImg.gameObject.SetActive(true);
        _initAlpha = _fadeImg.color.a;
        DOVirtual.Float(_initAlpha, 0f, _fadeDuration, (a) =>
        {
            _fadeImg.SetAlpha(a);
        })
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => _fadeImg.gameObject.SetActive(false));
    }

    public void SceneTransition(int index)
    {
        _fadeImg.gameObject.SetActive(true);
        DOVirtual.Float(0f, _initAlpha, _fadeDuration, (a) =>
        {
            _fadeImg.SetAlpha(a);
        })
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => SceneManager.LoadScene(index));
    }
    public void QuitButton(int index)
    {
        Application.Quit();
#if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
#endif
    }

}
