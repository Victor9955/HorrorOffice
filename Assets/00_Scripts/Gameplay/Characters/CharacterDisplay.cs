using FMODUnity;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPEffects.CharacterData.CharData;

public class CharacterDisplay : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform _enterTr;
    [SerializeField] private Transform _officeTr;
    [SerializeField] private Transform _exitTr;

    [SerializeField] private GameObject _characterPrefab;

    [SerializeField] StudioEventEmitter eventEmitter;
    [SerializeField] List<EventReference> events = new();


    private GameObject _currentCharacterObj;
    private CharacterStaticInfo _currentCharacterInfo;
    private Coroutine _moveCoroutine;

    public Action<GameObject> OnCharcterSpawned;
    public Action OnCharacterEntered;
    public Action OnCharacterExited;

    public void CharacterLeave(Action onEnd)
    {
        _currentCharacterObj.GetComponent<DialoguePlayer>().Hide();
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
        eventEmitter.EventReference = events[UnityEngine.Random.Range(0, events.Count)];
        SetCharacterObj(info);
        _currentCharacterObj.GetComponent<DialoguePlayer>().SetDialogue(info, dialogue);
        _currentCharacterInfo = info;
        OnCharcterSpawned?.Invoke(_currentCharacterObj);
        _moveCoroutine = StartCoroutine(Move(_officeTr.position, info._enterDuration, info._animCurve, () =>
        {
            OnCharacterEntered?.Invoke();
            onArrived?.Invoke();
        }));
    }

    private void SetCharacterObj(CharacterStaticInfo info)
    {
        _currentCharacterObj = Instantiate(_characterPrefab, transform);
        _currentCharacterObj.GetComponentInChildren<SpriteRenderer>().sprite = info.comingSprite;
        _currentCharacterObj.SetActive(true);
        _currentCharacterObj.transform.position = _enterTr.position;
    }

    private IEnumerator Move(Vector3 endPos, float duration = 1f, AnimationCurve animCurve = null, Action callback = null)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        float elapsed = 0f;
        Vector3 initPos = _currentCharacterObj.transform.position;
        float oldT = animCurve.Evaluate(0f);
        bool Up = false;
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


            if (Up)
            {
                if (oldT >= Mathf.Abs(Mathf.Sin(waveT)))
                {
                    Up = false;
                    oldT = Mathf.Abs(Mathf.Sin(waveT));
                    eventEmitter.Play();
                }
            }
            else
            {
                if (oldT <= Mathf.Abs(Mathf.Sin(waveT)))
                {
                    Up = true;
                    oldT = Mathf.Abs(Mathf.Sin(waveT));
                }
            }
            newPos += waveMov;

            _currentCharacterObj.transform.position = newPos;

            yield return null;
        }
        _currentCharacterObj.transform.position = endPos;
        _moveCoroutine = null;
        _currentCharacterObj.GetComponent<DialoguePlayer>().Say();
        callback?.Invoke();
    }
}