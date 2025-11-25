using DG.Tweening;
using FMODUnity;
using System;
using System.Collections;
using TMPro;
using Unity.Properties;
using UnityEngine;


public class FileBinder : MonoBehaviour, IDropContainer
{
    [SerializeField] private Transform _childContainerTR;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private EventReference _hoverSound;
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

    [SerializeField] Vector3 _filePosOffset;
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

    public void Init(Binder bindertype, float distance, float duration)
    {
        _initPos = transform.position;

        gameObject.SetActive(true);
        _binderType = bindertype;
        _openAnimDistance = distance;
        _openAnimDuration = duration;
        _text.text = _binderType.ToString();
    }

    public Vector3 GetFilePosition()
    {
        return transform.position + _filePosOffset;
    }

    private void OnMouseEnter()
    {
        UpdateOpenState(true, true);
    }

    private void OnMouseExit()
    {
        UpdateOpenState(false, true);
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
        Vector3 targetPos = isOpening ? _initPos - transform.forward * _openAnimDistance : _initPos;
        animIsOpen = isOpening;
        _childContainerTR.DOMove(targetPos, _openAnimDuration).SetEase(Ease.InOutSine);
        if (isHovered )
        {
            if (_animRoutine != null) StopCoroutine(_animRoutine);
            _animRoutine = StartCoroutine(OpenCoroutine());
            if (!_hoverSound.IsNull)
            {
                RuntimeManager.PlayOneShot(_hoverSound, transform.position);
            }
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
