using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{

    [Header("Refs")]
    [SerializeField] private Transform _enterTr;
    [SerializeField] private Transform _officeTr;
    [SerializeField] private Transform _exitTr;

    [SerializeField] private GameObject _characterPrefab;


    private GameObject _currentCharacterObj;
    private CharacterStaticInfo _currentCharacterInfo;
    private Coroutine _moveCoroutine;

    public Action OnCharcterSpawned;
    public Action OnCharacterEntered;
    public Action OnCharacterExited;

    public void CharacterLeave(Action onEnd)
    {
        _moveCoroutine = StartCoroutine(Move(_exitTr.position, _currentCharacterInfo._exitDuration, _currentCharacterInfo._animCurve, () =>
        {
            _currentCharacterObj.SetActive(false);
            Destroy(_currentCharacterObj);
            OnCharacterExited?.Invoke();
            onEnd?.Invoke();
        }));
    }

    public void SpawnCharacter(CharacterStaticInfo info,string dialogue, Action onArrived)
    {
        OnCharcterSpawned?.Invoke();
        _currentCharacter.GetComponentInChildren<TextMeshProUGUI>().text = dialogue;
        SetCharacterObj(info);
        _currentCharacterInfo = info;
        _moveCoroutine = StartCoroutine(Move(_officeTr.position, info._enterDuration, info._animCurve, () =>
        {
            OnCharacterEntered?.Invoke();
            onArrived?.Invoke();
        }));
    }

    private void SetCharacterObj(CharacterStaticInfo info)
    {
        _currentCharacterObj = Instantiate(_characterPrefab, transform);
        _currentCharacterObj.GetComponent<SpriteRenderer>().sprite = info.comingSprite;
        _currentCharacterObj.SetActive(true);
        _currentCharacterObj.transform.position = _enterTr.position;
    }

    private IEnumerator Move(Vector3 endPos, float duration = 1f, AnimationCurve animCurve = null, Action callback = null)
    {
        if(_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        float elapsed = 0f;
        Vector3 initPos = _currentCharacterObj.transform.position;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Interpolation du mouvement de base
            float t = Mathf.Clamp01(elapsed / duration);
            if (animCurve != null)
                t = animCurve.Evaluate(t);

            Vector3 newPos = Vector3.Lerp(initPos, endPos, t);

            // Ajout de la vague sinuso�dale sur l�axe Y
            float waveT = t * Mathf.PI * _currentCharacterInfo._walkFrequency;  // progression dans la sinuso�de
            Vector3 waveMov = Mathf.Abs(Mathf.Sin(waveT)) * _currentCharacterInfo._walkMagnitude * _currentCharacterObj.transform.up;
            newPos += waveMov;

            _currentCharacterObj.transform.position = newPos;

            yield return null;
        }
        _currentCharacterObj.transform.position = endPos;
        _moveCoroutine = null;
        callback?.Invoke();
    }
}