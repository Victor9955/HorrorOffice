using DG.Tweening;
using System;
using System.Collections;
using Unity.Properties;
using UnityEngine;


public class FileBinder : MonoBehaviour, IDropContainer
{

    public Binder BinderType => _binderType;
    public bool animIsOpen;
    public bool isUnlocked;
    private MeshRenderer _meshRend;
    private Binder _binderType;

    private float _openAnimDistance;
    private float _openAnimDuration;

    private bool _hasDropAnimEnded;
    private Vector3 _initPos;
    private Coroutine _animRoutine;
    private Transform _childContainerMesh;
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


    private void Awake()
    {
        _childContainerMesh = GetComponentInChildren<Transform>();
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
        UpdateOpenState(true);
    }

    private void OnMouseExit()
    {
        UpdateOpenState(false);
    }

    public bool Drop<T>(T dropped) where T : Draggable
    {
        EmployeeFile file = dropped as EmployeeFile;
        if (file == null) throw new Exception("Bruh that aint no File");

        if (isUnlocked)
        {
            if(_animRoutine != null) StopCoroutine(_animRoutine);
            UpdateOpenState(false);
        }
        return isUnlocked;
    }
    public bool IsUnlocked()
    {
        return isUnlocked;
    }


    public void UpdateOpenState(bool isOpening, bool isHovered = false)
    {
        if (isOpening == animIsOpen) return;
        Vector3 targetPos = isOpening ? _initPos + Vector3.back * _openAnimDistance : _initPos;
        animIsOpen = isOpening;
        transform.DOMove(targetPos, _openAnimDuration).SetEase(Ease.InOutSine);
        if (isHovered )
        {
            if (_animRoutine != null) StopCoroutine(_animRoutine);
            _animRoutine = StartCoroutine(OpenCoroutine());
        }

    }

    private IEnumerator OpenCoroutine()
    {
        while (animIsOpen)
        {
            yield return new WaitForSeconds(0.2f);
        }
        UpdateOpenState(false);
    }



    #region Debug

    private void OnMouseDown()
    {
        Debug.Log($"{name}'s type is {BinderType}");
    }
    #endregion
}
