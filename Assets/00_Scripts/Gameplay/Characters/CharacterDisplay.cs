using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterDisplay : MonoBehaviour, ISingletonMonobehavior
{

    [Header("Refs")]
    [SerializeField] private Transform _enterTr;
    [SerializeField] private Transform _officeTr;
    [SerializeField] private Transform _exitTr;

    [SerializeField] private GameObject _characterPrefab;

    [Header("Anim Parameters")]
    [SerializeField] private float _enterDuration, _exitAnimation;
    [SerializeField] private float _walkMagnitude = 1;
    [SerializeField] private int _walkFrequency = 1;
    [SerializeField] private AnimationCurve _animCurve;

    private GameObject _currentCharacter;
    Queue<GameObject> characterQueue;
    private Coroutine _moveCoroutine;
    private bool _moving;

    private void Awake()
    {
        characterQueue = new();
    }
    public void Init()
    {
        Singleton.Instance<GameManager>().OnNewRound += OnNewRoundEvent;
    }

    private void OnNewRoundEvent(int ind)
    {
        SpawnCharacter();
    }

    void SpawnCharacter()
    {
        if (_currentCharacter != null)
        {
            // add to queue
            Debug.Log("Queueing character");
            return;
        }
        _currentCharacter = Instantiate(_characterPrefab, transform);
        _currentCharacter.SetActive(true);
        _currentCharacter.transform.position = _enterTr.position;
        CharacterEnter();
    }

    #region CharacterMovement Methods

    void CharacterEnter()
    {
        _moveCoroutine = StartCoroutine(Move(_officeTr.position, _enterDuration, _animCurve, GoBack));
        //Sequence mySequence = DOTween.Sequence();
        //Tween slideRightTween = _currentCharacter.transform.DOShakePosition(_enterDuration, Vector3.up / 5f, randomness: 10).OnComplete(CharacterStepForward).Play();
        //Tween walkTween = _currentCharacter.transform.DOMove(_officeTr.position, _enterDuration).Play();
        //mySequence.Append(slideRightTween)
        //            .Insert(0f, walkTween);
        //mySequence.Play();
    }
    void GoBack()
    {
        _moveCoroutine = StartCoroutine(Move(_enterTr.position, _enterDuration, _animCurve, CharacterEnter));

    }

    void CharacterStepForward()
    {
        Action huh = new(() =>
        {
            Debug.Log("Huh");
        });
    }



    private IEnumerator Move(Vector3 endPos, float duration = 1f, AnimationCurve animCurve = null, Action callback = null)
    {
        float elapsed = 0f;
        Vector3 initPos = _currentCharacter.transform.position;
        _moving = true;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Interpolation du mouvement de base
            float t = Mathf.Clamp01(elapsed / duration);
            if (animCurve != null)
                t = animCurve.Evaluate(t);

            Vector3 newPos = Vector3.Lerp(initPos, endPos, t);

            // Ajout de la vague sinusoïdale sur l’axe Y
            float waveT = t * Mathf.PI * _walkFrequency;  // progression dans la sinusoïde
            Vector3 waveMov = Mathf.Abs(Mathf.Sin(waveT)) * _walkMagnitude * _currentCharacter.transform.up;
            newPos += waveMov;

            _currentCharacter.transform.position = newPos;

            yield return null;
        }
        _moving = false;
        _currentCharacter.transform.position = endPos;
        callback?.Invoke();
        yield return null;
    }
}



#endregion

