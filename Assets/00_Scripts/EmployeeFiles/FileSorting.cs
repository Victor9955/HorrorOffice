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

    [Space(5)]
    [Header("Parameters")]
    [Space(5)]
    [SerializeField] private List<Binder> _binderDataList;

    [Header("Events")]
    [Space(5)]
    [SerializeField] private UnityEvent<bool> OnSetOpenEvent;
    [SerializeField] private UnityEvent OnMatchCheckEvent;

    private bool _canDropFile;
    private EmployeeFile _currentFile;
    private List<FileBinder> _binderList = new();
    private Coroutine _newFileCoroutine;
    private int _fileIndex = 0;

    public event Action<Binder> OnFileDroppedEvent;

    private void Start()
    {
        _characterDisplay.OnCharacterEntered += () => _canDropFile = true;
        SetupBinders();
    }

    private void SetupBinders()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            FileBinder childBinder = transform.GetChild(i).GetComponent<FileBinder>();
            childBinder.Init(_binderDataList[i]);
            _binderList.Add(childBinder);
        }
        Debug.Log($"{_binderList.Count} binders in the scene");

    }

    private void SetBindersOpenState(bool isOpen)
    {
        foreach (FileBinder file in _binderList)
        {
            file.isOpen = isOpen;
        }
        OnSetOpenEvent.Invoke(isOpen);

        if (isOpen) _currentFile.OnDropped += OnFileDropped;
        else _currentFile.OnDropped -= OnFileDropped;

    }

    public void OnNewFile(SheetData data)
    {
        _newFileCoroutine = StartCoroutine(NewFile(data));
    }

    private IEnumerator NewFile(SheetData data)
    {
        WaitForSeconds wait = new(0.2f);
        while (!_canDropFile)
            yield return wait;
        _canDropFile = false;
        _fileIndex++;
        _currentFile = Instantiate(_fileToSortPrefab, _fileSpawnTr);
        _currentFile.Init(data, _fileIndex);
        int randInd = Random.Range(0, _binderList.Count);
        SetBindersOpenState(true);
        Singleton.Instance<GameManager>().OnFileSpawned?.Invoke();
    }

    private void OnFileDropped(Binder binderType)
    {
        SetBindersOpenState(false);
        //TODO Get Binder Dropped
        OnFileDroppedEvent?.Invoke(binderType);
    }
}