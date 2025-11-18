using DG.Tweening;
using NaughtyAttributes;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI.Table;

public class EmployeeFile : Draggable
{

    private SheetData _sheetData;
    public SheetData GetSheetData => _sheetData;

    [SerializeField] private SpriteRenderer _spriteRend;
    private SpriteRenderer SpriteRend
    {
        get
        {
            if (_spriteRend == null) _spriteRend = GetComponent<SpriteRenderer>();
            return _spriteRend;
        }
    }
    public Action<Binder> OnFileDroppedInSorter;
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
        //Init Object
        name = $"SheetInstance_{fileIndex}";
        gameObject.SetActive(true);
        _canBeDragged = _draggableOnInit;

        //Init Data
        _sheetData = data;
        SpriteRend.sprite = _sheetData.sprite;
        SetState(ref _initDI);
    }

    protected override void DragTick()
    {
        var ray = CamRaycast();
        if (ray.didHit) // the object is hovering above something
        {
            if (ray.hit.transform.CompareTag("Desk")) // hovering on desk
            {
                _targetDI = new(
                    ray.hit.point + (ray.hit.normal * 0.2f),
                    Quaternion.LookRotation(-ray.hit.normal)
                );
            }
            if (ray.hit.transform.TryGetComponent<IDropContainer>(out IDropContainer binder)) //hovering on a sorter
            {
                FileBinder fileBinder = binder as FileBinder;
                _targetDI = new(
                    fileBinder.GetFilePosition(),
                    Quaternion.LookRotation(fileBinder.transform.up)
                );
            }
        }
        else // if it isn't hoverwhere on anything
        {
            _targetDI = new(
                CamToWorldPos,
                Quaternion.LookRotation(_cam.transform.forward, Vector3.up)
            );
        }
        base.DragTick(); // apply DI
    }
    public override void Drop()
    {
        base.Drop();
        var ray = CamRaycast();

        if (ray.didHit) //dropped on anything where it can be dropped
        {
            if (ray.hit.transform.gameObject.TryGetComponent(out IDropContainer container)) // drop in sorter
            {
                if (container.IsUnlocked())
                {
                    bool hasDropped = container.Drop(this);
                    OnFileDroppedInSorter?.Invoke(_sheetData.rightBinder);

                    OnDroppedUEvent?.Invoke();
                    gameObject.SetActive(!_getsConsumedOnCorrectDrop);
                }
            }
            if (ray.hit.transform.CompareTag("Desk")) // Drop on desk
            {
                DragInfo deskDI = new
                    (
                        ray.hit.point + (ray.hit.normal),
                        Quaternion.LookRotation(-ray.hit.normal)
                    );
                _initDI = deskDI;
            }
        }
        else
        {
            Utils.BigText("hihihi");
            transform.DOMove(_initDI.Pos, _dragReturnDuration).SetEase(Ease.InOutSine).OnComplete(() => Debug.Log("Returned"));
            transform.DORotate(_initDI.Rot.eulerAngles, _dragReturnDuration);
        }
    }

    private (bool didHit, RaycastHit hit) CamRaycast()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        bool didHit = Physics.Raycast(ray, out hit, 100, _layerMask);
        //Debug.DrawRay(ray.origin, ray.direction, Color.blue, 2f);
        return (didHit, hit);
    }
}