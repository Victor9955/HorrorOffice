using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

public class EmployeeFile : Draggable
{

    private SheetData _sheetData;
    public SheetData GetSheetData => _sheetData;

    [SerializeField] private SpriteRenderer _spriteRend;
    [SerializeField] private Transform ancor;
    [SerializeField] private float _deskHeight = 0.1f;
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

    public Action<Binder,EmployeeFile> OnFileDroppedInSorter;
    public Color FileColor
    {
        get => SpriteRend.color;
        set
        {
            SpriteRend.color = value;
        }
    }

    DragInfo lastOnDeskInfo;

    private void Start()
    {
        Vector3 old = ancor.position;
        ancor.position += Vector3.up * 3.5f;
        ancor.DOMove(old, 2f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            ancor.DOShakeScale(0.35f,0.1f).SetEase(Ease.OutElastic);
        });
    }

    public void Init(SheetData data)
    {
        gameObject.SetActive(true);
        _isDraggable = _draggableOnInit;

        //Init Data
        _sheetData = data;
        SpriteRend.sprite = _sheetData.sprite;
        _initDI = new(
            transform.position,
            transform.rotation
            );

        lastOnDeskInfo = _initDI;
    }
    FileBinder fileBinder;

    protected override void DragTick()
    {
        var ray = CamRaycast();
        if (ray.didHit) // the object is hovering above something
        {
            if (ray.hit.transform.CompareTag("Desk")) // hovering on desk
            {
                _targetDI = new(
                    ray.hit.point + (ray.hit.normal.normalized * _deskHeight),
                    Quaternion.Euler(-Quaternion.LookRotation(ray.hit.normal.normalized).eulerAngles.x, Quaternion.LookRotation(_cam.transform.up).eulerAngles.y, -Quaternion.LookRotation(ray.hit.normal.normalized).eulerAngles.z)
                );
                lastOnDeskInfo = _targetDI;
            }
            if (ray.hit.transform.TryGetComponent<IDropContainer>(out IDropContainer binder)) //hovering on a sorter
            {
                fileBinder = binder as FileBinder;
                _targetDI = new(
                    fileBinder.GetFilePosition(),
                    Quaternion.LookRotation(-fileBinder.transform.up)
                );
            }
            else
            {
                fileBinder = null;
            }
        }
        else // if it isn't hovering on anything
        {
            _targetDI = new DragInfo(
               CamToWorldPos,
               Quaternion.LookRotation(_cam.transform.forward)
               );
        }
        base.DragTick(); // apply DI
    }
    public override void Drop()
    {
        base.Drop();

        if (fileBinder != null) //dropped on anything where it can be dropped
        {
            fileBinder.Drop(this);
            OnFileDroppedInSorter?.Invoke(fileBinder.BinderType, this);
            gameObject.SetActive(!_getsConsumedOnCorrectDrop);
        }
        else
        {
            transform.DOMove(lastOnDeskInfo.Pos, _dragReturnDuration).SetEase(Ease.InOutSine);
            DOTween.To(() => transform.rotation, (q) => transform.rotation = q, lastOnDeskInfo.Rot.eulerAngles, _dragReturnDuration);
        }
    }

    private (bool didHit, RaycastHit hit) CamRaycast()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        bool didHit = Physics.Raycast(ray, out hit, 100, _layerMask);
        return (didHit, hit);
    }
}