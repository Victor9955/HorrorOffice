using NaughtyAttributes;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Draggable : MonoBehaviour
{
    [SerializeField,Range(0.3f,1f),Tooltip("how much the obj is near the camera's eye (1 = furthest)")] protected float _distance;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected bool _getsConsumedOnCorrectDrop = true;
    [SerializeField] protected float _draggingTick = 0.2f;
    //Refs
    protected Camera _cam;
    //Info
    protected bool _isDragging;
    protected (Vector3 pos, Quaternion rot) pickupStateTr;
    protected (Vector3 pos, Quaternion rot) _initialStateTr;

    private float _dragTick;
    private void Start()
    {
        _cam = Camera.main;
        _dragTick = _draggingTick;
        _initialStateTr = (transform.position, transform.rotation);
    }

    public Vector3 CamToWorldPos
    {
        get
        {
            Vector3 mousePos = Mouse.current.position.value;
            mousePos.z = _distance;
            return _cam.ScreenToWorldPoint(mousePos);
        }
    }

    private void OnMouseDown()
    {
        StartCoroutine(Drag());
    }

    private void OnMouseUp()
    {
        Drop();
    }

    public virtual void Drop()
    {
        // Usual drop stuff
    }
    IEnumerator Drag()
    {
        Debug.Log("Start dragging");
        _isDragging = true;
        while (_isDragging)
        {
            Vector3 newPos = CamToWorldPos + pickupStateTr.pos;
            //Debug.Log("dir =  " + newPos);
            transform.position = newPos;
            transform.rotation = pickupStateTr.rot;
            yield return new WaitForSeconds(_dragTick);
        }
        transform.position = _initialStateTr.pos;
        transform.rotation = _initialStateTr.rot;
        Debug.Log("Stop dragging");
    }

    #region Rotation
    [Button] public void DebugSetPickupState() => SetPickupState();
    protected void SetPickupState()
    {
        pickupStateTr.pos = transform.position;
        pickupStateTr.rot = transform.rotation;

        Utils.BigText("Pickup Rotation saved", "white", 15);
    }
    #endregion
}
