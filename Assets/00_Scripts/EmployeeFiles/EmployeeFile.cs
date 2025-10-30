using NaughtyAttributes;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EmployeeFile : Draggable
{

    private int _binderID;
    public int FileID => _binderID;
    private SpriteRenderer _spriteRend;
    private SpriteRenderer SpriteRend
    {
        get
        {
            if (_spriteRend == null) _spriteRend = GetComponent<SpriteRenderer>();
            return _spriteRend;
        }
    }
    public Action<bool> OnDropped;
    public UnityEvent OnDroppedUEvent;
    public Color FileColor
    {
        get => SpriteRend.color;
        set
        {
            SpriteRend.color = value;
        }
    }
    public void InitObject(int fileIndex)
    {
        name = $"DragFileToSort_{fileIndex}";
    }
    public void InitData(Sprite sheetSprite, int binderID)
    {
        SpriteRend.sprite = sheetSprite;
        _binderID = binderID;
    }

    public void ResetFile()
    {
        
        gameObject.SetActive(true);
    }

    public override void Drop()
    {
        base.Drop();

        _isPickedUp = false;
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