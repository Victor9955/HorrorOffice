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
    [SerializeField] private Transform _choiceTransform;
    [SerializeField] private FileChoice _choicePrefab;
    [SerializeField] private TMP_Text _sortText;
    [Space(5)]
    [Header("Parameters")]
    [SerializeField] private float _choiceSpawnPosOffset;
    [Space(5)]
    [SerializeField] private List<FileChoiceData> _fileChoiceList;
    
    private EmployeeFile _fileToSort;
    private int _fileIndex;

    private void Start()
    {
        NewFile(_fileIndex);
    }

    private void NewFile(int id)
    {
        MeshRenderer meshRend = _fileToSortObj.GetComponent<MeshRenderer>();
        int randInd = Random.Range(0, _fileChoiceList.Count);
        _fileToSortObj.Init(_fileChoiceList[randInd]);
        _fileToSortObj.OnDroppedEvent += OnChoose;
        Clear();
        _fileToSortObj.ResetFile();
        SpawnClasseur();
    }

    private void Clear()
    {
        for (int i = 0; i < _choiceTransform.childCount; i++)
        {
            Destroy(_choiceTransform.GetChild(0).gameObject);
        }
        _sortText.gameObject.SetActive(false);
            }

    private void SpawnClasseur()
    {
        for (int i = 0; i < _fileChoiceList.Count; i++)
        {
            FileChoice newChoice = Instantiate(_choicePrefab,_choiceTransform).GetComponent<FileChoice>();
            Vector3 posOffset = Vector3.right * (_choiceSpawnPosOffset * i);
            newChoice.transform.position = _choiceTransform.position + posOffset;
            newChoice.Init(_fileChoiceList[i]);
        }
    }

    private void OnChoose()
    {
        _sortText.text = "Correct Sort!";
        _sortText.color =  Color.white;
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