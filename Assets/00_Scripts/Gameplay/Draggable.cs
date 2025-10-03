using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Draggable : MonoBehaviour
{
    [SerializeField] protected float _distance;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected bool _getsConsumedOnCorrectDrop = true;
    [SerializeField] protected float _draggingTick = 0.2f;
    //Refs
    protected Camera _cam;
    //Info
    protected Vector3 _initialPosition;
    protected bool _isDragging;

    private float _dragTick;
    private void Start()
    {
        _cam = Camera.main;
        _dragTick = _draggingTick;
    }

    public Vector3 CamToWorldPos
    {
        get
        {
            Vector3 mousePos = Mouse.current.position.value;
            mousePos.z = _distance;
            Debug.Log("mousePos =  " + mousePos);
            return _cam.ScreenToWorldPoint(mousePos);
        }
    }

    private void OnMouseDown()
    {
        _initialPosition = transform.position;
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
            Vector3 newPos = CamToWorldPos;
            Debug.Log("dir =  " + newPos);
            transform.position = newPos;
            yield return new WaitForSeconds(_dragTick);
        }
        transform.position = _initialPosition;
        Debug.Log("Stop dragging");
    }
}
