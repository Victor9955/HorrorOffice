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
    [SerializeField] private Transform _fileSpawnTr;
    [Space(5)]
    [Header("Parameters")]
    [Space(5)]
    [SerializeField] private List<BinderData> _binderDataList;
    //[SerializeField] private float _binderSpacingDistance;
    [Header("Text UI")]
    [Space(5)]
    //[SerializeField] private TMP_Text _sortText;
    [SerializeField] private Transform _sortUITr;
    [SerializeField] private float _textDelay;
    [Header("Events")]
    [Space(5)]
    [SerializeField] private UnityEvent<bool> OnSetOpenEvent;
    [SerializeField] private UnityEvent OnMatchCheckEvent;

    private bool _canDropFile;
    private EmployeeFile _currentFile;
    private List<FileBinder> _binderList;
    private Coroutine _textCoroutine;
    private Coroutine _newFileCoroutine;
    private int _fileIndex;
    private WaitForSeconds _textWaitForSeconds;

    public event Action OnFileDroppedEvent;

    private void Awake()
    {
        _binderList = new();
    }

    private void Start()
    {
        _textWaitForSeconds = new WaitForSeconds(_textDelay);
        Init();
    }
    public void Init()
    {
        _fileIndex = 0;
        SetupBinders();
        Singleton.Instance<GameManager>().OnStartRound += OnNewFileRound; // REGISTER ON START ROUND
        Singleton.Instance<GameManager>().OnCharacterEnter += OnEnterAnimEnd; // REGISTER ON START ROUND
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

    public void OnNewFileRound()
    {
        //if(_sortText != null)
        //{
        //    _sortText.text = $"file n°{_fileIndex + 1}";
        //    _sortText.color = Color.white;
        //    ShowText(false);
        //}
        _newFileCoroutine = StartCoroutine(NewFile());
    }

    private void OnEnterAnimEnd()
    {
        _canDropFile = true;
    }
    private IEnumerator NewFile()
    {
        WaitForSeconds wait = new(0.2f);
        while (!_canDropFile)
            yield return wait;
        _canDropFile = false;
        _fileIndex++;
        _currentFile = Instantiate(_fileToSortPrefab, _fileSpawnTr);

        if (_binderList.Count <= 0) Debug.LogError("Aint no damn container foo' ???");
        int randInd = Random.Range(0, _binderList.Count);
        _currentFile.Init(_binderList[randInd], _fileIndex);
  
        _currentFile.ResetFile();
        SetBindersOpenState(true);
        Singleton.Instance<GameManager>().OnFileSpawned?.Invoke();
        //// Play Dialogue etc
        //yield return new WaitForSeconds(Random.Range(0f, 2.5f));
        //Singleton.Instance<GameManager>().OnDialogueEnd?.Invoke();
    }


    private void OnFileDropped(bool isMatched)
    {
        string matchResult = isMatched ? "Correct Sort!" : "Wrong Sort...";
        SetBindersOpenState(false);

        OnFileDroppedEvent?.Invoke();

        //show match on text
        //_sortText.text = $"file n°{_fileIndex + 1} : {matchResult}";
        //_sortText.color = Color.white;
        //ShowText(true);
        //StartCoroutine(Singleton.Instance<FileRoundManager>().StopRound(isMatched));
    }


    #region Text Methods
    private void ShowText(bool endRound)
    {
        if (_textCoroutine != null) StopCoroutine(_textCoroutine);
        //_textCoroutine = StartCoroutine(ShowTextRoutine(endRound));
    }
    /*
    private IEnumerator ShowTextRoutine(bool endRound)
    {
        //_sortUITr.gameObject.SetActive(true);
        //yield return _textWaitForSeconds;
        //_sortUITr.gameObject.SetActive(false);
        //if (endRound) Singleton.Instance<FileRoundManager>().isRoundEnding = false;
    }*/
    #endregion
}