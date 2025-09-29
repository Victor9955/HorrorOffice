using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class File
{
    [SerializeField] private int _id;
    public int FileID => _id;
    [SerializeField] private Color _color = Color.white;
    public Color FileColor => _color;
}

public class FileSorting : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject _fileToSortObj;
    [SerializeField] private Transform _choiceTransform;
    [SerializeField] private GameObject _choicePrefab;
    [SerializeField] private TMP_Text _sortText;
    [Space(5)]
    [Header("Parameters")]
    [SerializeField] private float _choiceOffset;
    [Space(5)]
    [SerializeField] private List<File> _fileList;
    
    private File _fileToSort;
    private int _fileIndex;

    private void Start()
    {
        NewFile(_fileIndex);
    }

    private void NewFile(int id)
    {
        MeshRenderer meshRend = _fileToSortObj.GetComponent<MeshRenderer>();
        int randInd = Random.Range(0, _fileList.Count);
        _fileToSort = _fileList[randInd];
        meshRend.material.color = _fileToSort.FileColor;
        ClearButtons();
        SpawnButtons();
    }

    private void ClearButtons()
    {
        for (int i = 0; i < _choiceTransform.childCount; i++)
        {
            Destroy(_choiceTransform.GetChild(0).gameObject);
        }
        _sortText.gameObject.SetActive(false);
    }

    private void SpawnButtons()
    {
        for (int i = 0; i < _fileList.Count; i++)
        {
            FileChoice newChoice = Instantiate(_choicePrefab,_choiceTransform).GetComponent<FileChoice>();
            Vector3 posOffset = Vector3.right * (_choiceOffset * i);
            newChoice.transform.position = _choiceTransform.position + posOffset;
            newChoice.Init(_fileList[i]);
            newChoice.OnChooseEvent += OnChoose;
        }
    }

    private void OnChoose(int choiceID)
    {
        bool isSortedCorrectly = (choiceID == _fileToSort.FileID);
        _sortText.text = isSortedCorrectly ? "Correct Sort!": "Wrong Sort...";
        _sortText.color = isSortedCorrectly ? Color.green : Color.red;
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