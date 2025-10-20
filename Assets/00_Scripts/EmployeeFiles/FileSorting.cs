using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;



public class FileSorting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EmployeeFile _fileToSortObj;
    private List<FileChoice> _fileChoiceList;
    [SerializeField] private FileChoice _choicePrefab;
    [SerializeField] private TMP_Text _sortText;
    [Space(5)]
    [Header("Parameters")]
    [SerializeField] private float _choiceSpawnPosOffset;
    [Space(5)]
    [SerializeField] private List<FileChoiceData> _fileChoiceDataList;

    private EmployeeFile _fileToSort;
    private int _fileIndex;

    private void Start()
    {
        NewFile(_fileIndex);
    }

    private void NewFile(int id)
    {
        MeshRenderer meshRend = _fileToSortObj.GetComponent<MeshRenderer>();
        int randInd = Random.Range(0, _fileChoiceDataList.Count);
        _fileToSortObj.Init(_fileChoiceDataList[randInd]);
        _fileToSortObj.OnDroppedEvent += OnChoose;
        _fileToSortObj.ResetFile();
        SetDrop(true);
    }


    private void SetDrop(bool active)
    {
        foreach (FileChoice file in _fileChoiceList)
        {
            file.isOpen = active;
        }

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