using UnityEngine;
using UnityEngine.UIElements;



// https://www.tiktok.com/@unkn0wn_person09/video/7503004467252579601 real important

public class Clickable : MonoBehaviour
{
    [SerializeField, Range(10f, 100f)] private float _viewDistanceFromObject = 100f;
    [SerializeField] private float _fov = 58;
    //[SerializeField] private Vector3 _posOffset;
    private void OnValidate()
    {
        Camera cam = Camera.main;

        if (cam == null) Debug.LogError("No MainCam ????");
    }

    private void OnMouseDown()
    {

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
