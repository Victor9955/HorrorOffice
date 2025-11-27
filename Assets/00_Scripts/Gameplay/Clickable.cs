using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;



// https://www.tiktok.com/@unkn0wn_person09/video/7503004467252579601 real important

public class Clickable : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float _focusDuration;

    [Space(5), Header("Focus settings")]
    [SerializeField, Range(10f, 100f)] private float _viewDistanceFromObject = 100f;
    [SerializeField] private float _fov = 58;

    [Space(5), Header("Unfocus limits")]
    [SerializeField] private Vector2 _verticalLimits = Vector2.zero;
    [SerializeField] private Vector2 _horizontalLimits = Vector2.zero;

    private bool IsCamFocused => _camMovement.focusState != FocusState.Unfocused;

    // cam info
    private Vector3 _position;
    private Vector3 _dir;
    public Vector3 RotationInEulerAngles => _rotationInEulerAngles;
    private Vector3 _rotationInEulerAngles;
    public float Fov => _fov;

    // unfocus limits
    public Vector2 HorizontalLimits => _horizontalLimits;
    public Vector2 VerticalLimits => _verticalLimits;

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
        if (!IsCamFocused) _camMovement.FocusClickable(this);
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
