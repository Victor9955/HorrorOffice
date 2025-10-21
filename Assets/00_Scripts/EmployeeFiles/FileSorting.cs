using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


[ExecuteAlways]
public class FileSorting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EmployeeFile _fileToSortObj;
    [SerializeField] private List<FileChoice> _fileChoiceList;
    [SerializeField] private FileChoice _choicePrefab;
    [SerializeField] private TMP_Text _sortText;
    [Space(5)]
    [Header("Parameters")]
    [Space(5)]
    [SerializeField] private float _binderSpacingDistance;
    [Header("Events")]
    [Space(5)]
    [SerializeField] private UnityEvent<bool> OnSetOpenEvent;
    [SerializeField] private UnityEvent OnMatchCheckEvent;
    private EmployeeFile _fileToSort;
    private int _fileIndex;

    private void Start()
    {
        NewFile(_fileIndex);
    }

    #region Binder Management Methods
    private void OnTransformChildrenChanged()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            FileChoice binder;
            if (!child.TryGetComponent<FileChoice>(out binder))
            {
                Debug.LogWarning($"{child.name} is not a Binder ({(typeof(FileChoice).ToString())}' componnent is needed)");
                child.parent = null;
            }
        }
         // sort binders if needed //SortBinders();
    }

    private void SortBinders()
    {
        foreach (FileChoice binder in _fileChoiceList)
        {
            Debug.Log("FileSorting child count changed, sorting binders");
            // sort according to order/spacing ?
        }
    }
    private void SetBindersOpenState(bool isOpen)
    {
        foreach (FileChoice file in _fileChoiceList)
        {
            file.isOpen = isOpen;
        }
        OnSetOpenEvent.Invoke(isOpen);
    }
    #endregion Binder Management Methods


    private void NewFile(int id)
    {
        MeshRenderer meshRend = _fileToSortObj.GetComponent<MeshRenderer>();
        if (_fileChoiceList.Count <= 0) Debug.LogError("Aint no damn container foo' ???");
        int randInd = Random.Range(0, _fileChoiceList.Count);
        _fileToSortObj.Init(_fileChoiceList[randInd]);
        _fileToSortObj.OnDroppedEvent += OnChoose;
        _fileToSortObj.ResetFile();
        SetBindersOpenState(true);
    }


    private void OnChoose(bool isMatched)
    {
        _sortText.text = "Correct Sort!";
        _sortText.color = Color.white;
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        _sortText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _sortText.gameObject.SetActive(false);
        _fileIndex++;
        NewFile(_fileIndex);
    }
}