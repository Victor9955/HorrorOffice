using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class FileSorting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EmployeeFile _fileToSortPrefab;
    [SerializeField] private Transform _fileSpawnTr;
    [SerializeField, Required] private CharacterDisplay _characterDisplay;

    [Header("Parameters")]
    [Space(5)]
    [SerializeField] private float _stackingDistance;
    [SerializeField] private float _openAnimDistance;
    [SerializeField] private float _openAnimDuration;

    public List<Binder> _binderDataList;

    [Header("Events")]
    [Space(5)]
    [SerializeField] private UnityEvent<bool> OnSetLockEvent;
    [SerializeField] private UnityEvent OnMatchCheckEvent;

    private bool _canDropFile;
    private EmployeeFile _currentFile;
    private List<FileBinder> _binderList = new();
    private Coroutine _newFileCoroutine;
    private int _fileIndex = 0;

    public event Action<Binder> OnFileDroppedEvent;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _characterDisplay.OnCharacterEntered += () => _canDropFile = true;
        _characterDisplay.OnCharacterExited += () => SetBindersLockState(true);
    }

    [Button]
    public void SetupBinders()
    {
        _binderList.Clear();
        if (_binderDataList.Count <= 0) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < _binderDataList.Count; i++)
        {
            FileBinder childBinder = transform.GetChild(i).GetComponent<FileBinder>();
            childBinder.Init(_binderDataList[i], _openAnimDistance, _openAnimDuration);
            childBinder.transform.position = transform.position + (Vector3.up * (i * _stackingDistance / 10));
            _binderList.Add(childBinder);
        }
        Debug.Log($"{_binderList.Count} binders in the scene");
    }


    private void SetBindersLockState(bool isUnlocked)
    {
        foreach (FileBinder file in _binderList)
        {
            file.isUnlocked = isUnlocked;
        }
        OnSetLockEvent.Invoke(isUnlocked);

        if (isUnlocked) _currentFile.OnFileDroppedInSorter += OnFileDropped;
        else _currentFile.OnFileDroppedInSorter -= OnFileDropped;

    }

    public void OnNewFile(SheetData data)
    {
        _canDropFile = false;
        _fileIndex++;
        _currentFile = Instantiate(_fileToSortPrefab, _fileSpawnTr);
        _currentFile.Init(data, _fileIndex);
        int randInd = Random.Range(0, _binderList.Count);
        SetBindersLockState(true);
        Singleton.Instance<GameManager>().OnFileSpawned?.Invoke();
    }


    private void OnFileDropped(Binder binderType)
    {
        SetBindersLockState(false);
        //TODO Get Binder Dropped
        OnFileDroppedEvent?.Invoke(binderType);
    }
}