using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;


public struct DragStateInfo
{
    private Vector3 _posOffset;
    private float _yaw;
    private float _pitch;
    private float _roll;

    public Quaternion Rot => Quaternion.Euler(_pitch, _yaw, _roll);
    public Vector3 Pos => _posOffset;


    #region Constructors
    public DragStateInfo(Vector3 posOffset, float yaw, float pitch, float roll)
    {
        _posOffset = posOffset;
        _yaw = yaw;
        _pitch = pitch;
        _roll = roll;
    }

    public DragStateInfo(DragStateInfo original)
    {
        _posOffset = original._posOffset;
        _yaw = original._yaw;
        _pitch = original._pitch;
        _roll = original._roll;
    }

    public DragStateInfo(Vector3 posOffset, Quaternion rot)
    {
        _posOffset = posOffset;
        Vector3 euleurRot = rot.eulerAngles;
        _yaw = euleurRot.x;
        _pitch = euleurRot.y;
        _roll = euleurRot.z;
    }

    #endregion
}

public class Draggable : MonoBehaviour
{
    #region Parameters
    [SerializeField, Range(0.3f, 1f), Tooltip("how much the obj is near the camera's eye (1 = furthest)")] protected float _distance;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected bool _getsConsumedOnCorrectDrop = true;
    [SerializeField] protected float _dragPosSpeed;
    [SerializeField] protected float _dragRotSpeed;
    [SerializeField] protected float _draggingTick = 0.2f;
    [SerializeField] protected float _dragReturnDuration = 0.4f;
    #endregion

    //Refs
    protected Camera _cam;
    public Vector3 CamToWorldPos
    {
        get
        {
            //Vector3 mousePos = _cam.ScreenPointToRay(Mouse.current.position.value).GetPoint(_distance);
            return _cam.ScreenPointToRay(Mouse.current.position.value).GetPoint(_distance);
        }
    }

    //Info
    [SerializeField] protected bool _draggableOnReset = true;
    protected bool _canBeDragged;
    protected bool _isPickedUp;
    protected DragStateInfo _initDI;
    protected DragStateInfo _targetDI;
    protected DragStateInfo _pickedUpDI;

    Coroutine _dragCoroutine;
    public Coroutine DragCoroutine
    {
        get => _dragCoroutine;
        private set
        {
            if (_dragCoroutine != null)
                StopCoroutine(_dragCoroutine);
            _dragCoroutine = value;
        }
    }
    private void Start()
    {
        _cam = Camera.main;
    }
    #region Inputs

    private void OnMouseDown()
    {
        if (!_canBeDragged)
        {
            Debug.Log($"Cant pickup {name} rn");
        }
        else DragCoroutine = StartCoroutine(Drag());
    }

    private void OnMouseUp()
    {
        Drop();
    }

    #endregion
    public virtual void Drop()
    {
        _isPickedUp = false;
        // Usual drop stuff
    }
    protected virtual IEnumerator Drag()
    {

        Debug.Log("Start dragging");
        _isPickedUp = true;
        while (_isPickedUp)
        {
            //_targetDI = ComputePickedUpDrag();
            DragTick();
            //transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
            yield return new WaitForSeconds(_draggingTick);
        }

        //_dragCoroutine = StartCoroutine(DragReturn());
        Debug.Log("Stop dragging");
    }

    protected virtual void DragTick()
    {
        // do the tick stuff idk
        ApplyDrag(_targetDI);
    }

    private IEnumerator DragReturn()
    {
        float elapsed = 0;
        //transform.DORotate(_initDI.Rot.eulerAngles, 0.4f);
        while (elapsed < _dragReturnDuration)
        {
            elapsed += Time.deltaTime;
            ApplyDrag(_initDI, false);
            yield return new WaitForSeconds(_draggingTick);
        }
        Debug.Log("Returned");

    }

    private DragStateInfo ComputePickedUpDrag()
    {
        //transform.LookAt(_cam.transform, Vector3.up);
        Quaternion rot = Quaternion.LookRotation((_cam.transform.position - transform.position).normalized, Vector3.up);

        return new DragStateInfo
            (
            CamToWorldPos + _pickedUpDI.Pos,
            rot.x,
            rot.y,
            rot.z
            );
    }

    protected void ApplyDrag(DragStateInfo dragInfo, bool isDragging = true)
    {
        Quaternion targetRot = dragInfo.Rot;
        Vector3 targetPos = dragInfo.Pos;

        DragStateInfo newDI = new
            (
            DragLerp(transform.position, targetPos), // Lerp Pos
            DragLerp(transform.rotation, targetRot) // Lerp Yaw
            );

        transform.position = newDI.Pos;
        transform.rotation = newDI.Rot;

    }

    #region Lerps
    public float DragLerp(float P, float T)
    {
        if (_dragPosSpeed * Time.deltaTime > 1) return T;
        float result = P + (T - P) * _dragPosSpeed * Time.deltaTime;
        return result;
    }

    public Vector3 DragLerp(Vector3 P, Vector3 T)
    {
        if (_dragPosSpeed * Time.deltaTime > 1) return T;
        Vector3 result = P + (T - P) * _dragPosSpeed * Time.deltaTime;
        return result;
    }

    public Quaternion DragLerp(Quaternion P, Quaternion T)
    {
        if (_dragPosSpeed * Time.deltaTime > 1) return T;
        /*
        Vector3 PVec = P.eulerAngles;
        Vector3 TVec = T.eulerAngles;
        Quaternion resultVec = Quaternion.Euler(PVec + (TVec - PVec) * _dragPosSpeed * Time.deltaTime);*/
        return Quaternion.Lerp(P,T,1f);
    }


    #endregion

    protected void SetState(ref DragStateInfo setInfo, DragStateInfo targetInfo)
    {
        setInfo = targetInfo;
        Utils.BigText("Pickup State saved", "white", 15);

    }
    protected void SetState(ref DragStateInfo setInfo)
    {
        DragStateInfo info = new
            (
            transform.position,
            transform.rotation
            );
        setInfo = info;
        Utils.BigText("Pickup State saved", "white", 15);
    }


}
