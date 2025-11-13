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
        //ResetFilePosition();
    }
    private void InitObject(int fileIndex)
    {
        Debug.Log($"init : ({_initDI.Pos},{_initDI.Rot}), tr : ({transform.position},{transform.rotation})");
        name = $"SheetInstance_{fileIndex}";
        gameObject.SetActive(true);
        _canBeDragged = _draggableOnReset;
    }
    private void InitData(SheetData sheetData)
    {
        _sheetData = sheetData;
        SpriteRend.sprite = _sheetData.sprite;
        SetState(ref _initDI);
    }


    protected override void DragTick()
    {
        var ray = CamRaycast();
        if (ray.didHit)
        {
            bool didHitDesk = ray.hit.transform.CompareTag("Desk");
            if (didHitDesk)
            {
                DragStateInfo deskDI = new
                    (
                        ray.hit.point + (ray.hit.normal * 0.2f),
                        _initDI.Rot.x,
                        _initDI.Rot.y,
                        _initDI.Rot.z
                    );
                _targetDI = deskDI;
            }
        }
        ;
        base.DragTick();
    }
    public override void Drop()
    {
        base.Drop();
        var ray = CamRaycast();
        if (ray.didHit)
        {
            if (ray.hit.transform.gameObject.TryGetComponent(out IDropContainer container))
            {
                if (container.CanReceive())
                {
                    bool hasDropped = container.Drop(this);
                    OnDropped?.Invoke(_sheetData.rightBinder);

                    OnDroppedUEvent?.Invoke();
                    gameObject.SetActive(!_getsConsumedOnCorrectDrop);
                }
                else if (ray.hit.transform.CompareTag("Desk"))
                {
                    DragStateInfo deskDI = new
                        (
                            ray.hit.point + (ray.hit.normal),
                            _initDI.Rot.x,
                            _initDI.Rot.y,
                            _initDI.Rot.z
                        );

                }

            }
            else Debug.Log("Cant get da DropContainer :(");
        }
        else Debug.Log("Cant hit anything :(");
    }

    private (bool didHit, RaycastHit hit) CamRaycast()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        bool didHit = Physics.Raycast(ray, out hit, 100, _layerMask);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 2f);
        return (didHit, hit);
    }
}