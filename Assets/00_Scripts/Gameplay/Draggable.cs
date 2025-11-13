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
            Vector3 mousePos = Mouse.current.position.value;
            mousePos.z = _distance;
            return _cam.ScreenToWorldPoint(mousePos);
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
        _dragCoroutine = StartCoroutine(DragReturn());
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
            _targetDI = ComputePickedUpDrag();
            
            DragTick();
            yield return new WaitForSeconds(_draggingTick);
        }
        _dragCoroutine = StartCoroutine(DragReturn());
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
        transform.LookAt(_cam.transform, Vector3.up);
        Quaternion rot = Quaternion.LookRotation((_cam.transform.position - transform.position), Vector3.up);
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

        DragStateInfo currentDI = new
            (
            DragLerp(transform.position, targetPos), // Lerp Pos
            DragLerp(transform.rotation.x, targetRot.x), // Lerp Yaw
            DragLerp(transform.rotation.y, targetRot.y), // Lerp Pitch
            DragLerp(transform.rotation.z, targetRot.z) // Lerp Roll
            );
        transform.position = currentDI.Pos;
        transform.rotation = currentDI.Rot;
        //if (isDragging)
        //{
        //    transform.rotation = Quaternion.LookRotation(-(_cam.transform.position - transform.position), transform.up);
        //}
        //else transform.rotation = currentDI.Rot;

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
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z
            );
        setInfo = info;
        Utils.BigText("Pickup State saved", "white", 15);
    }

    protected virtual void ResetDrag()
    {
        transform.position = _initDI.Pos;
        transform.rotation = _initDI.Rot;
        gameObject.SetActive(true);
        _canBeDragged = _draggableOnReset;
    }
}
