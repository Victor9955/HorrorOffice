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
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.UI.Image;


public struct DragInfo
{
    private Vector3 _posOffset;
    private float _yaw;
    private float _pitch;
    private float _roll;

    public Quaternion Rot;
    public Vector3 Pos => _posOffset;


    #region Constructors
    public DragInfo(Vector3 posOffset, float yaw, float pitch, float roll)
    {
        _posOffset = posOffset;
        Rot = Quaternion.Euler(yaw, pitch, roll);
        _yaw = yaw;
        _pitch = pitch;
        _roll = roll;
    }

    public DragInfo(DragInfo original)
    {
        _posOffset = original._posOffset;
        Rot = original.Rot;
        _yaw = original._yaw;
        _pitch = original._pitch;
        _roll = original._roll;
    }

    public DragInfo(Vector3 posOffset, Quaternion rot)
    {
        _posOffset = posOffset;
        Rot = rot;
        Vector3 euleurRot = rot.eulerAngles;
        _yaw = euleurRot.x;
        _pitch = euleurRot.y;
        _roll = euleurRot.z;
    }

    public DragInfo(Vector3 posOffset, Vector3 eulerAngles)
    {
        _posOffset = posOffset;
        Rot = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        _yaw = eulerAngles.x;
        _pitch = eulerAngles.y;
        _roll = eulerAngles.z;
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
    protected Camera _cam => Camera.main;
    public Vector3 CamToWorldPos => _cam.ScreenPointToRay(Mouse.current.position.value).GetPoint(_distance);

    //Info
    [SerializeField] protected bool _draggableOnInit = true;
    protected bool _isDraggable;
    protected bool _isPickedUp;
    // Drag Infos
    protected DragInfo _initDI;
    protected DragInfo _targetDI;
    protected DragInfo _pickedUpDI;

    [HideInInspector] public Action OnPickup;


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

    #region Inputs
    private void OnMouseDown()
    {
        if (!_isDraggable)
        {
            Debug.Log($"Cant pickup {name} rn");
        }
        else DragCoroutine = StartCoroutine(Drag());
    }

    private void OnMouseUp()
    {
        if (_isPickedUp) Drop();
    }

    #endregion
    public virtual void Drop()
    {
        _isPickedUp = false;
        // Usual drop stuff
    }
    protected virtual IEnumerator Drag()
    {
        _isPickedUp = true;
        OnPickup?.Invoke();
        while (_isPickedUp)
        {
            DragTick();
            yield return new WaitForSecondsRealtime(_draggingTick);
        }
    }

    protected virtual void DragTick()
    {
        // do the tick stuff idk
        ApplyDrag(_targetDI);
    }

    private DragInfo ComputePickedUpDrag()
    {
        //transform.LookAt(_cam.transform, Vector3.up);
        Quaternion rot = Quaternion.LookRotation((_cam.transform.position - transform.position).normalized, Vector3.up);

        return new DragInfo
            (
            CamToWorldPos + _pickedUpDI.Pos,
            rot.x,
            rot.y,
            rot.z
            );
    }

    protected void ApplyDrag(DragInfo dragInfo, bool isDragging = true)
    {
        Quaternion targetRot = dragInfo.Rot;
        Vector3 targetPos = dragInfo.Pos;

        DragInfo newDI = new
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
        return Mathf.Lerp(P,T,Time.deltaTime * _dragPosSpeed);
    }

    public Vector3 DragLerp(Vector3 P, Vector3 T)
    {
        if (_dragPosSpeed * Time.deltaTime > 1) return T;
        return Vector3.Lerp(P,T, _dragPosSpeed * Time.deltaTime);
    }

    public Quaternion DragLerp(Quaternion P, Quaternion T)
    {
        if (_dragPosSpeed * Time.deltaTime > 1) return T;
        return Quaternion.Lerp(P, T, _dragPosSpeed * Time.deltaTime);
    }


    #endregion

    protected void SetState(ref DragInfo setInfo, DragInfo targetInfo)
    {
        setInfo = targetInfo;
        Utils.BigText("Pickup State saved", "white", 15);

    }
    protected void SetState(ref DragInfo setInfo)
    {
        DragInfo info = new
            (
            transform.position,
            transform.rotation
            );
        setInfo = info;
        Utils.BigText("Pickup State saved", "white", 15);
    }


}
