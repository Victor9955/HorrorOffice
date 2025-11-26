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



    private void OnValidate()
    {
        if (Camera.main == null) Debug.LogError("No MainCam ????");
    }

    private void Start()
    {
        Transform cam = Camera.main.transform;
        Vector3 camObjDir = transform.position - cam.position;
        Vector3 pos = transform.position + (-camObjDir * (_viewDistanceFromObject * 0.01f));
        Quaternion rot = Quaternion.LookRotation(camObjDir, Vector3.up);
        transform.position = pos;
        transform.rotation = rot;

    }

    private void OnMouseDown()
    {
        FocusCam();
    }

    private void FocusCam()
    {
        Camera.main.transform.DOMove(transform.position,_focusDuration);
        Camera.main.transform.DORotate(transform.rotation.eulerAngles, _focusDuration);

        throw new NotImplementedException();
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
