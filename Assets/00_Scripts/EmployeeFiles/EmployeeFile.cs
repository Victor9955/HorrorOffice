using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EmployeeFile : Draggable
{

    private int _id;
    public int FileID => _id;
    private MeshRenderer _mesh;
    public Action<bool> OnDropped;
    public UnityEvent OnDroppedUEvent;
    public Color FileColor
    {
        get => _mesh.material.color;
        set
        {
            Debug.Log("GYAAAAAAAAAAT");
            _mesh.material.color = value;
        }
    }

    public void Init(FileBinder binder)
    {
        _id = binder.Id;
        FileColor = binder.MeshMatColor;
    }

    public void ResetFile()
    {
        transform.position = _initialPosition;
        gameObject.SetActive(true);
    }


    private void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
    }
    public override void Drop()
    {
        base.Drop();

        _isDragging = false;
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        bool didHit = Physics.Raycast(ray, out hit, 100, _layerMask);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 2f);
        if (didHit)
        {
            if (hit.transform.gameObject.TryGetComponent(out IDropContainer container))
            {
                if (container.IsOpen())
                {
                    bool isMatched = container.Drop(this);
                    OnDropped?.Invoke(isMatched);
                    OnDroppedUEvent?.Invoke();
                    gameObject.SetActive(!_getsConsumedOnCorrectDrop);
                }
            }
            else Debug.LogError("Cant get da DropContainer :(");
        }
        else Debug.Log("Cant hit anything :(");
    }
}