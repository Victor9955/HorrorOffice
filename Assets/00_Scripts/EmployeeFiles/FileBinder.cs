using DG.Tweening;
using System;
using System.Collections;
using Unity.Properties;
using UnityEngine;


public class FileBinder : MonoBehaviour, IDropContainer
{

    public Binder BinderType => _binderType;
    public bool canReceive;
    private MeshRenderer _meshRend;
    private Binder _binderType;

    private float _openAnimDistance;
    private float _openAnimDuration;

    private bool _hasDropAnimEnded;
    private Vector3 _initPos;

    public MeshRenderer MeshRend
    {
        get
        {
            if (_meshRend == null)
            {
                _meshRend = GetComponent<MeshRenderer>();

            }
            return _meshRend;
        }
    }

    private void Start()
    {
        _initPos = transform.position;
    }
    public void Init(Binder bindertype, float distance, float duration)
    {
        _binderType = bindertype;
        _openAnimDistance = distance;
        _openAnimDuration = duration;
    }

    private void OnMouseEnter()
    {
        UpdateBinderState(true);
    }

    private void OnMouseExit()
    {
        UpdateBinderState(false);
    }

    public bool Drop<T>(T dropped) where T : Draggable
    {
        EmployeeFile file = dropped as EmployeeFile;
        UpdateBinderState(false);
        if (canReceive)
            if (file == null) throw new Exception("Bruh that aint no File");
        return true;
    }
    public bool CanReceive()
    {
        return canReceive;
    }

    private void UpdateBinderState(bool isOpening)
    {
        string state = isOpening ? "Opening" : "Closing";
        Vector3 targetPos = isOpening ? _initPos + Vector3.back * _openAnimDistance : _initPos;
        transform.DOMove(targetPos, _openAnimDuration, false);
    }



    #region Debug


    private void OnMouseDown()
    {
        Debug.Log($"{name}'s type is {BinderType}");
    }
    #endregion
}
