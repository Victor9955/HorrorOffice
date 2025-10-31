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
    [SerializeField] private float _enterDuration;
    [SerializeField] private float _exitDuration;
    [SerializeField] private float _walkMagnitude = 1;
    [SerializeField] private int _walkFrequency = 1;
    [SerializeField] public AnimationCurve _animCurve;

    private GameObject _currentCharacter;
    private Coroutine _moveCoroutine;

    public Action OnCharcterSpawned;
    public Action OnCharacterEntered;
    public Action OnCharacterExited;

    public void CharacterLeave(Action onEnd)
    {
        _moveCoroutine = StartCoroutine(Move(_exitTr.position, _exitDuration, _animCurve, () =>
        {
            _currentCharacter.SetActive(false);
            Destroy(_currentCharacter);
            OnCharacterExited?.Invoke();
            onEnd?.Invoke();
        }));
    }

    public void SpawnCharacter(Sprite character, Action onArrived)
    {
        OnCharcterSpawned?.Invoke();
        _currentCharacter = Instantiate(_characterPrefab, transform);
        _currentCharacter.GetComponent<SpriteRenderer>().sprite = character;
        _currentCharacter.SetActive(true);
        _currentCharacter.transform.position = _enterTr.position;

        _moveCoroutine = StartCoroutine(Move(_officeTr.position, _enterDuration, _animCurve,() =>
        {
            OnCharacterEntered?.Invoke();
            onArrived?.Invoke();
        }));
    }

    private IEnumerator Move(Vector3 endPos, float duration = 1f, AnimationCurve animCurve = null, Action callback = null)
    {
        if(_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

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
        _moveCoroutine = null;
        callback?.Invoke();
    }
}