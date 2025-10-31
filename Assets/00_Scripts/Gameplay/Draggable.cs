using NaughtyAttributes;
using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public struct DragStateInfo
{
    private Vector3 _posOffset;
    private float _yaw;
    private float _pitch;
    private float _roll;

    public Quaternion Rot => Quaternion.Euler(_pitch, _yaw, _roll);
    public Vector3 Pos => _posOffset;

    public DragStateInfo(Vector3 posOffset, float yaw, float pitch, float roll)
    {
        _posOffset = posOffset;
        _yaw = yaw;
        _pitch = pitch;
        _roll = roll;
    }
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
        _initDI = SetState(_initDI);
        _cam = Camera.main;
        _dragCoroutine = StartCoroutine(DragReturn());
    }
    #region Inputs

    private void OnMouseDown()
    {
        DragCoroutine = StartCoroutine(Drag());
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
            _targetDI = ComputeTargetDrag();
            ApplyDrag(_targetDI);
            yield return new WaitForSeconds(_draggingTick);
        }
        _dragCoroutine = StartCoroutine(DragReturn());
        Debug.Log("Stop dragging");
    }

    private IEnumerator DragReturn()
    {
        float elapsed = 0;
        while (elapsed < _dragReturnDuration)
        {
            elapsed += Time.deltaTime;
            ApplyDrag(_initDI,false);
            yield return new WaitForSeconds(_draggingTick);
        }
        Debug.Log("Returned");

    }

    private DragStateInfo ComputeTargetDrag()
    {
        transform.LookAt(_cam.transform, Vector3.up);

        return new DragStateInfo
            (
            CamToWorldPos + _pickedUpDI.Pos,
            transform.rotation.x,
            transform.rotation.y,
            transform.rotation.z
            );
    }

    private void ApplyDrag(DragStateInfo dragInfo, bool isDragging = true)
    {
        Quaternion targetRot = dragInfo.Rot;
        Vector3 targetPos = dragInfo.Pos;

        DragStateInfo _currentDI = new
           (
           DragLerp(transform.position, targetPos), // Lerp Pos
           DragLerp(transform.rotation.x, targetRot.x), // Lerp Yaw
           DragLerp(transform.rotation.y, targetRot.y), // Lerp Pitch
           DragLerp(transform.rotation.z, targetRot.z) // Lerp Roll
           );

        transform.position = _currentDI.Pos;
        if (isDragging)
        {
            transform.LookAt(_cam.transform, transform.up);
        }
        else transform.rotation = _currentDI.Rot;

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

    [Button] 
        protected DragStateInfo SetState(DragStateInfo info)
    {
        info = new DragStateInfo
            (
            transform.position,

            transform.rotation.x,
            transform.rotation.y,
            transform.rotation.z
            );

        Utils.BigText("Pickup State saved", "white", 15);
        return info;
    }
}
