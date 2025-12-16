using DG.Tweening;
using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.UIElements;



// https://www.tiktok.com/@unkn0wn_person09/video/7503004467252579601 real important

public class Clickable : MonoBehaviour
{
    [Space(5), Header("Focus settings")]
    [SerializeField] private bool _doNotFocus;
    [SerializeField] private bool _playsSound;
    [SerializeField] private EventReference _fmodEventReference;
    [SerializeField, Range(10f, 100f)] private float _viewDistanceFromObject = 100f;
    [SerializeField] private float _fov = 58;
    [SerializeField] private string _innerDialogueID;

    [Header("Animation")]
    [SerializeField] private float _focusDuration;

    [Space(5), Header("Unfocus limits")]
    [SerializeField] private Vector2 _verticalLimits = Vector2.zero;
    [SerializeField] private Vector2 _horizontalLimits = Vector2.zero;

    private bool IsCamFocused => _camMovement.focusState != FocusState.Unfocused;
    private bool IsChangingFocus => _camMovement.ischangingFocus;

    public bool canFocus = true;

    // cam info
    private Vector3 _position;
    private Vector3 _dir;
    public Vector3 RotationInEulerAngles => _rotationInEulerAngles;
    private Vector3 _rotationInEulerAngles;
    public float Fov => _fov;

    // unfocus limits
    public Vector2 HorizontalLimits => _horizontalLimits;
    public Vector2 VerticalLimits => _verticalLimits;
    public float FocusDuration => _focusDuration;

    //private refs
    CameraMovement _camMovement;


    private void Start()
    {
        _camMovement = Camera.main.GetComponent<CameraMovement>();

        Transform cam = Camera.main.transform;
        Vector3 camObjDir = transform.position - cam.position;
        Vector3 pos = transform.position + (-camObjDir * (_viewDistanceFromObject * 0.01f));
        Quaternion rot = Quaternion.LookRotation(camObjDir, Vector3.up);
        _position = pos;
        _rotationInEulerAngles = rot.eulerAngles;
        _dir = camObjDir.normalized;
    }

    private void OnMouseDown()
    {
        if(_playsSound) RuntimeManager.PlayOneShot(_fmodEventReference);

        if (!canFocus || _doNotFocus) return;
        if (!IsCamFocused && !IsChangingFocus)
        {
            HUDController.instance.SetThoughtText(_innerDialogueID);
            _camMovement.FocusClickable(this);
            // activate text ui
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw its frustum
        Transform cam = Camera.main.transform;
        Vector3 camObjDir = transform.position - cam.position;
        Vector3 pos = transform.position + (-camObjDir * (_viewDistanceFromObject * 0.01f));
        Quaternion rot = Quaternion.LookRotation(camObjDir, Vector3.up);
        Gizmos.matrix = Matrix4x4.TRS(pos, rot, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, _fov, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;

    }
}
