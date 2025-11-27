using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;



// https://www.tiktok.com/@unkn0wn_person09/video/7503004467252579601 real important

public class Clickable : MonoBehaviour
{
    [SerializeField] private float _focusDuration;
    [Space(5)]
    [SerializeField, Range(10f, 100f)] private float _viewDistanceFromObject = 100f;
    [SerializeField] private float _fov = 58;


    private bool IsCamFocused => _camMovement.focusState != FocusState.Unfocused;

    //Cam Info
    private Vector3 _position;
    private Vector3 _dir;
    private Quaternion _rotation;
    public Vector3 RotationInEulerAngles => _rotation.eulerAngles;
    public float Fov => _fov;

    CameraMovement _camMovement;



    private void Start()
    {
        _camMovement = Camera.main.GetComponent<CameraMovement>();

        Transform cam = Camera.main.transform;
        Vector3 camObjDir = transform.position - cam.position;
        Vector3 pos = transform.position + (-camObjDir * (_viewDistanceFromObject * 0.01f));
        Quaternion rot = Quaternion.LookRotation(camObjDir, Vector3.up);
        _position = pos;
        _rotation = rot;
        _dir = camObjDir.normalized;
    }

    private void OnMouseDown()
    {
        if (!IsCamFocused) _camMovement.FocusClickable(this);
    }

    private void FocusCam()
    {
        Utils.BigText("click");
        Camera.main.transform.DOMove(_position, _focusDuration).SetEase(Ease.InOutQuad);
        Camera.main.transform.DORotate(_rotation.eulerAngles, _focusDuration).SetEase(Ease.InOutQuad);
        Camera.main.DOFieldOfView(_fov, _focusDuration).SetEase(Ease.InOutQuad);
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
