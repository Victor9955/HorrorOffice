using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputControlScheme;
using Random = UnityEngine.Random;

public class FileSorting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EmployeeFile _fileToSortPrefab;
    [SerializeField] private TMP_Text _sortText;
    [SerializeField] private Transform _fileSpawnTr;
    [Space(5)]
    [Header("Parameters")]
    [Space(5)]
    [SerializeField] private List<BinderData> _binderDataList;
    //[SerializeField] private float _binderSpacingDistance;
    [Header("Events")]
    [Space(5)]
    [SerializeField] private UnityEvent<bool> OnSetOpenEvent;
    [SerializeField] private UnityEvent OnMatchCheckEvent;

    private EmployeeFile _currentFile;
    private List<FileBinder> _binderList;
    private Coroutine _textCoroutine;
    private int _fileIndex;

    private void Awake()
    {
        _binderList = new List<FileBinder>();
    }

    public void Init()
    {
        _fileIndex = 0;
        SetupBinders();
        Singleton.Instance<GameManager>().OnStartRound += OnNewFileRound; // REGISTER ON START ROUND
    }


    #region Binder Management Methods
    private void SetupBinders()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            FileBinder childBinder = transform.GetChild(i).GetComponent<FileBinder>();
            childBinder.Init(i, _binderDataList[i]);
            _binderList.Add(childBinder);
        }
        Debug.Log($"{_binderList.Count} binders in the scene");

    }
    private void SortBindersEditor()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            FileBinder potentialBinder;
            if (!child.TryGetComponent(out potentialBinder))
            {
                Debug.LogWarning($"{child.name} is not a Binder ({(typeof(FileBinder).ToString())}' componnent is needed)");
                foreach (FileBinder binder in _binderList)
                {
                    // sort binders if needed
                }
                child.parent = null;
            }
        }
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
    #endregion Binder Management Methods

    void OnNewFileRound()
    {
        _sortText.text = $"file n°{_fileIndex + 1}";
        _sortText.color = Color.white;
        ShowText(false);
        NewFile();
    }
    private void NewFile()
    {
        _fileIndex++;
        _currentFile = Instantiate(_fileToSortPrefab, _fileSpawnTr);
        _currentFile.name = $"DragFileToSort_{_fileIndex}";
        if (_binderList.Count <= 0) Debug.LogError("Aint no damn container foo' ???");
        int randInd = Random.Range(0, _binderList.Count);
        _currentFile.Init(_binderList[randInd]);
        _currentFile.ResetFile();
        SetBindersOpenState(true);
    }


    private void OnFileDropped(bool isMatched)
    {
        string matchResult = isMatched ? "Correct Sort!" : "Wrong Sort...";
        SetBindersOpenState(false);

        //show match on text
        _sortText.text = $"file n°{_fileIndex + 1} : {matchResult}";
        _sortText.color = Color.white;
        ShowText(true);
        StartCoroutine(Singleton.Instance<FileRoundManager>().StopRound(isMatched));
    }

    private void ShowText(bool endRound)
    {
        if (_textCoroutine != null) StopCoroutine(_textCoroutine);
        _textCoroutine = StartCoroutine(ShowTextRoutine(endRound));
    }
    private IEnumerator ShowTextRoutine(bool endRound)
    {

        _sortText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _sortText.gameObject.SetActive(false);
        if (endRound) Singleton.Instance<FileRoundManager>().isRoundEnding = false;
    }
}