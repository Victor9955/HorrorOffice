using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class FileSorting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EmployeeFile _fileToSortPrefab;
    [SerializeField] private Transform _filePoolTr;
    [SerializeField, Required] private CharacterDisplay _characterDisplay;
    [SerializeField] private Transform _initialFilesTR;

    [Header("Binder Parameters")]
    [Space(5)]
    [SerializeField] private float _binderStackingDistance;
    [SerializeField] private float _openAnimDistance;
    [SerializeField] private float _openAnimDuration;
    public List<Binder> _binderDataList;

    [Header("File Stack Parameters")]
    [SerializeField] private float _fileStackingDistance = 2;
    [SerializeField] private Vector2 _fileStackRotOffset;

    //[Header("Events")]
    //[Space(5)]
    //[SerializeField] private UnityEvent<bool> OnSetLockEvent;
    //[SerializeField] private UnityEvent OnMatchCheckEvent;

    private bool _canDropFile;
    private EmployeeFile _currentFile;
    private List<FileBinder> _binderList = new();
    private List<EmployeeFile> _fileList = new();
    private Coroutine _newFileCoroutine;
    private int _fileIndex = 0;

    public event Action<Binder> OnFileDroppedEvent;

    private void Awake()
    {
        _binderList.Clear();
        _fileList.Clear();
    }
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _characterDisplay.OnCharacterEntered += () => _canDropFile = true;
        _characterDisplay.OnCharacterExited += () => SetBindersLockState(true);
        SetupBinders();
        _binderList.Clear();
        FileStackUpdate();
    }
    #region Binder Methods

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
            childBinder.transform.position = transform.position + (Vector3.up * (i * _binderStackingDistance / 10));
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
        //OnSetLockEvent.Invoke(isUnlocked);

        if (isUnlocked) _currentFile.OnFileDroppedInSorter += OnFileDropped;
        else _currentFile.OnFileDroppedInSorter -= OnFileDropped;

    }

    #endregion
    public void OnNewFile(SheetData data)
    {
        _fileIndex++;
        _currentFile = Instantiate(_fileToSortPrefab);

        //Addfile to stack
        FileStackAdd(_currentFile);
        
        _currentFile.Init(data, _fileIndex);

        FileStackUpdate(); // set lil rot offset 
        int randInd = Random.Range(0, _binderList.Count);
        SetBindersLockState(true);
        Singleton.Instance<GameManager>().OnFileSpawned?.Invoke();
    }

    private void FileStackAdd(EmployeeFile file)
    {
        file.transform.parent = transform;
        FileStackUpdate();
    }

    private void FileStackUpdate()
    {
        foreach (EmployeeFile file in _fileList)
        {
            Debug.Log(file.name);
            file.transform.position = _filePoolTr.position + transform.up * (_binderStackingDistance /10);
            Debug.Log($"{name} position = {file.transform.position}");
            file.transform.Rotate(Vector3.up * Random.Range(_fileStackRotOffset.x,_fileStackRotOffset.y));
        }
    }

    private void OnFileDropped(Binder binderType)
    {
        SetBindersLockState(false);
        //TODO Get Binder Dropped
        OnFileDroppedEvent?.Invoke(binderType);
    }
}