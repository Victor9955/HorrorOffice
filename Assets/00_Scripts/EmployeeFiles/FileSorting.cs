using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FileSorting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EmployeeFile _fileToSortPrefab;
    [SerializeField] private Transform _filePoolTr;
    [SerializeField, Required] private CharacterDisplay _characterDisplay;

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
    private List<EmployeeFile> _activeFileList = new();
    private Coroutine _newFileCoroutine;
    private int _fileIndex = 0;

    public event Action<Binder,SheetData> OnFileDroppedEvent;

    private void Awake()
    {
        _binderList.Clear();
        _activeFileList.Clear();
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
        _currentFile = Instantiate(_fileToSortPrefab, _filePoolTr);

        //Addfile to stack
        FileStackAdd(_currentFile);

        _currentFile.Init(data, _fileIndex);

        FileStackUpdate(); // set files in a stack and applies lil rot offset 
        SetBindersLockState(true);
        Singleton.Instance<GameManager>().OnFileSpawned?.Invoke();
    }

    private void FileStackAdd(EmployeeFile file)
    {
        _activeFileList.Add(file);
        file.transform.rotation = _filePoolTr.rotation;
        file.OnPickup += FileStackRemoveTopFile;
        FileStackUpdate();

    }

    private void FileStackRemoveTopFile()
    {
        EmployeeFile file = _activeFileList.Last();
        _activeFileList.Remove(file);
        Debug.Log("now count " + _activeFileList.Count);
        file.OnPickup -= FileStackRemoveTopFile;
        FileStackUpdate();
    }

    private void FileStackUpdate()
    {
        if (_activeFileList.Count <= 0) return;
        for (int i = 0; i < _activeFileList.Count; i++)
        {
            EmployeeFile file = _activeFileList[i];
            file.transform.position = _filePoolTr.position + (Vector3.up * _fileStackingDistance * i);
            file.IsDraggable = false;
        }
        _activeFileList.Last().IsDraggable = true;
    }

    private void OnFileDropped(Binder binderType, SheetData sheetData)
    {
        SetBindersLockState(false);
        OnFileDroppedEvent?.Invoke(binderType, sheetData);
    }
}