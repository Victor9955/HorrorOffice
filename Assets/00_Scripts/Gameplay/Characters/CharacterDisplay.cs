using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{

    [Header("Refs")]
    [SerializeField] private Transform _enterTr;
    [SerializeField] private Transform _officeTr;
    [SerializeField] private Transform _exitTr;

    [SerializeField] private GameObject _characterPrefab;

    [Header("Anim Parameters")]
    [SerializeField] private bool _debugLoopEnterExitAnim;
    [SerializeField] private float _enterDuration;
    [SerializeField] private float _exitDuration;
    [SerializeField] private float _walkMagnitude = 1;
    [SerializeField] private int _walkFrequency = 1;
    [SerializeField] public AnimationCurve _animCurve;

    private GameObject _currentCharacter;
    Queue<GameObject> characterQueue;
    private Coroutine _moveCoroutine;

    private void Awake()
    {
        characterQueue = new();
    }

    #region Event Methods
    public void OnCharacterDialogueEnd()
    {
        CharacterExit(Singleton.Instance<GameManager>().OnCharacterExit);
    }
    private void OnCharacterExit()
    {
        _currentCharacter.SetActive(false);
        _currentCharacter = null;
    }

    #endregion

    #region Chara Queue

    public void SpawnCharacter(Sprite character)
    {
        _currentCharacter = Instantiate(_characterPrefab, transform);
        _currentCharacter.GetComponent<SpriteRenderer>().sprite = character;
        _currentCharacter.SetActive(true);
        _currentCharacter.transform.position = _enterTr.position;

        CharacterEnter(Singleton.Instance<GameManager>().OnCharacterEnter);
    }



    #endregion

    #region CharacterMovement Methods

    void CharacterEnter(Action callback)
    {
        _moveCoroutine = StartCoroutine(Move(_officeTr.position, _enterDuration, _animCurve, callback));
    }
    void CharacterGoBackToSpawn(Action callback)
    {
        _moveCoroutine = StartCoroutine(Move(_enterTr.position, _enterDuration, _animCurve, callback));

    }
    void CharacterExit(Action callback)
    {
        _moveCoroutine = StartCoroutine(Move(_exitTr.position, _exitDuration, _animCurve, callback));
    }

    void CharacterStepForward()
    {
        Action huh = new(() =>
        {
            Debug.Log("Huh");
        });

        huh.Invoke();
    }

    private IEnumerator Move(Vector3 endPos, float duration = 1f, AnimationCurve animCurve = null, Action callback = null)
    {
        float elapsed = 0f;
        Vector3 initPos = _currentCharacter.transform.position;
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
        _currentCharacter.transform.position = endPos;
        callback?.Invoke();
    }
    #endregion
}