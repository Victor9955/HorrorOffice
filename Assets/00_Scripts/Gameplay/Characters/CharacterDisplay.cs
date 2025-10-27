using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour, ISingletonMonobehavior
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
        if (_debugLoopEnterExitAnim)
        {
            DebugLoopEnter();
            return;
        }
        Singleton.Instance<GameManager>().OnNewRound += OnNewRoundEvent;
        Singleton.Instance<GameManager>().OnDialogueEnd += OnCharacterDialogueEnd;
        Singleton.Instance<GameManager>().OnCharacterExit += OnCharacterExit;

    }

    #region Event Methods

    private void OnNewRoundEvent(int ind)
    {
        SpawnCharacter();
    }
    private void OnCharacterDialogueEnd()
    {
        Action characterExitCallback = () => Singleton.Instance<GameManager>().OnCharacterExit?.Invoke();
        CharacterExit(characterExitCallback);
    }
    private void OnCharacterExit()
    {
        _currentCharacter.SetActive(false);
        _currentCharacter = null;
        //TakeOffQueue();
    }

    #endregion

    #region Chara Queue

    private void AddToQueue(GameObject character)
    {
        characterQueue.Enqueue(character);

    }

    private bool UpdateQueue()
    {
        if (_currentCharacter != null) // current characater already in use
        {
            return false;
        }
        // Start next in queue
        SpawnCharacter();
        return true;
    }
    private void TakeOffQueue()
    {
        characterQueue.Dequeue();
        UpdateQueue();
    }

    void SpawnCharacter()
    {
        //if (_currentCharacter != null)
        //{
        //    return;
        //    // add to queue
        //    AddToQueue(chara);
        //    UpdateQueue();
        //    Debug.Log("Queueing character");
        //}
        _currentCharacter = Instantiate(_characterPrefab, transform);
        _currentCharacter.SetActive(true);
        _currentCharacter.transform.position = _enterTr.position;
        // Need to spawn the sheet after entrance
        Action characterEnterCallback = () => Singleton.Instance<GameManager>().OnCharacterEnter?.Invoke();
        CharacterEnter(characterEnterCallback);
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
    #endregion

    #region Debug

    [Button]
    public void DebugSpawnCharacter()
    {
        if (_currentCharacter != null)
        {
            Debug.LogWarning("Already a character in office (IMPLEMENT QUEUE PLEASE BRO)");
            return;
        }
        SpawnCharacter();
    }
    void DebugLoopEnter()
    {
        _moveCoroutine = StartCoroutine(Move(_enterTr.position, _enterDuration, _animCurve, DebugLoopBack));
    }
    void DebugLoopBack()
    {
        _moveCoroutine = StartCoroutine(Move(_enterTr.position, _enterDuration, _animCurve, DebugLoopEnter));
    }
    #endregion Debug
}