using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

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

    public bool IsDraggable
    {
        get => _isDraggable;
        set { _isDraggable = value; }
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
        _isDraggable = _draggableOnInit;

        //Init Data
        _sheetData = data;
        SpriteRend.sprite = _sheetData.sprite;
        _initDI = new(
            transform.position,
            transform.rotation
            );

        transform.localPosition = _initDI.Pos;
        transform.localRotation = _initDI.Rot;
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
                Debug.DrawRay(ray.hit.point, ray.hit.normal);
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
        else // if it isn't hovering on anything
        {
             _targetDI = new DragInfo(
                CamToWorldPos,
                Quaternion.LookRotation(_cam.transform.forward , Vector3.up)
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
            if (ray.hit.transform.CompareTag("Desk")) // Drop on desk
            {
                DragInfo deskDI = new
                    (
                        ray.hit.point + (ray.hit.normal),
                    Quaternion.LookRotation(ray.hit.normal, Vector3.up)
                        );
                Debug.DrawRay(ray.hit.point, ray.hit.normal);
                _initDI = deskDI;

            }
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
        }
        else
        {
            Utils.BigText("hihihi");
            transform.DOMove(_initDI.Pos, _dragReturnDuration).SetEase(Ease.InOutSine).OnComplete(() => Debug.Log("Returned"));
            transform.DOLocalRotate(Quaternion.LookRotation(Vector3.down).eulerAngles, _dragReturnDuration).OnComplete(() => Debug.Log("pluh"));
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