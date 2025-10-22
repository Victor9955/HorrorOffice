using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class FileSorting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EmployeeFile _fileToSortObj;
    [SerializeField] private TMP_Text _sortText;
    [Space(5)]
    [Header("Parameters")]
    [Space(5)]
    [SerializeField] private List<BinderData> _binderDataList;
    //[SerializeField] private float _binderSpacingDistance;
    [Header("Events")]
    [Space(5)]
    [SerializeField] private UnityEvent<bool> OnSetOpenEvent;
    [SerializeField] private UnityEvent OnMatchCheckEvent;

    private List<FileBinder> _binderList;
    private int _fileIndex;

    private void Awake()
    {
        _binderList = new List<FileBinder>();
    }

    private void Start()
    {
        _fileIndex = 0;
        SetupBinders();
        NewFile();
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
    private void SortBindert()
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

    private void SortBinders()
    {

    }
    private void SetBindersOpenState(bool isOpen)
    {
        foreach (FileBinder file in _binderList)
        {
            file.isOpen = isOpen;
        }
        OnSetOpenEvent.Invoke(isOpen);

        if(isOpen) _fileToSortObj.OnDropped += OnFileDropped;
        else _fileToSortObj.OnDropped -= OnFileDropped;

    }
    #endregion Binder Management Methods


    private void NewFile()
    {
        MeshRenderer meshRend = _fileToSortObj.GetComponent<MeshRenderer>();
        if (_binderList.Count <= 0) Debug.LogError("Aint no damn container foo' ???");
        int randInd = Random.Range(0, _binderList.Count);
        _fileToSortObj.Init(_binderList[randInd]);
        _fileToSortObj.ResetFile();
        SetBindersOpenState(true);
    }


    private void OnFileDropped(bool isMatched)
    {
        string matchResult = isMatched ? "Correct Sort!" : "Wrong Sort...";
        SetBindersOpenState(false);
        _sortText.text = $"file n°{_fileIndex} : {matchResult}";
        _sortText.color = Color.white;
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        _fileIndex++;
        _sortText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _sortText.gameObject.SetActive(false);
        NewFile();
    }
}