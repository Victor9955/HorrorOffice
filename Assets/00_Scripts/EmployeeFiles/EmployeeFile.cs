using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EmployeeFile : Draggable
{

    private int _id;
    public int FileID => _id;
    private MeshRenderer _mesh;
    public Action OnDroppedEvent;
    public void Init(FileChoiceData choice)
    {
        _id = choice._id;
        FileColor = choice._color;
    }

    public void ResetFile()
    {
        transform.position = _initialPosition;
        gameObject.SetActive(true);
    }

    public Color FileColor
    {
        get => _mesh.material.color;
        set
        {
            Debug.Log("GYAAAAAAAAAAT");
            _mesh.material.color = value;
        }
    }

    private void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
    }
    public override void Drop()
    {
        base.Drop();

        _isDragging = false;
        bool didDrop = false;
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        bool didHit = Physics.Raycast(ray, out hit, 100, _layerMask);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 2f);
        if (didHit)
        {
            if (hit.transform.gameObject.TryGetComponent(out IDropContainer container))
            {
                didDrop = container.Drop(this);
            }
            else Debug.LogError("Cant get da DropContainer :(");
            if (didDrop)
            {
                OnDroppedEvent?.Invoke();
                gameObject.SetActive(!_getsConsumedOnCorrectDrop);
            }
        }
        else Debug.Log("Cant hit anything :(");
    }
}