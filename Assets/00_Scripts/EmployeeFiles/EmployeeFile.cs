using NaughtyAttributes;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EmployeeFile : Draggable
{

    private SheetData _sheetData;
    public SheetData GetSheetData => _sheetData;

    private SpriteRenderer _spriteRend;
    private SpriteRenderer SpriteRend
    {
        get
        {
            if (_spriteRend == null) _spriteRend = GetComponent<SpriteRenderer>();
            return _spriteRend;
        }
    }
    public Action<Binder> OnDropped;
    public UnityEvent OnDroppedUEvent;
    public Color FileColor
    {
        get => SpriteRend.color;
        set
        {
            SpriteRend.color = value;
        }
    }

    public void Init(SheetData data, int fileIndex)
    {
        InitObject(fileIndex);
        InitData(data);
        ResetFile();
    }
    private void InitObject(int fileIndex)
    {
        name = $"SheetInstance_{fileIndex}";
        Debug.Log($"{name} type = {_sheetData}");
    }
    private void InitData(SheetData sheetData)
    {
        _sheetData = sheetData;
        SpriteRend.sprite = _sheetData.sprite;
    }

    private void ResetFile()
    {
        
        gameObject.SetActive(true);
    }

    public override void Drop()
    {
        base.Drop();

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
                    OnDropped?.Invoke(_sheetData.rightBinder);

                    OnDroppedUEvent?.Invoke();
                    gameObject.SetActive(!_getsConsumedOnCorrectDrop);
                }
            }
            else Debug.LogError("Cant get da DropContainer :(");
        }
        else Debug.Log("Cant hit anything :(");
    }

}