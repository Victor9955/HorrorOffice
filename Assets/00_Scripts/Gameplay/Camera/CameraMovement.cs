using DG.Tweening;
using HuntroxGames.Utils;
using NaughtyAttributes;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Camera cameraRef;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float transitionSpeed;


    [Header("PC")]
    [SerializeField] Vector2 offset;
    [SerializeField] Transform pc;
    [SerializeField] float fovPC;

    bool isFocused = false;

    Quaternion lastRotation;
    float lastFov;

    private void Start()
    {
        lastFov = cameraRef.fieldOfView;
        lastRotation = cameraTransform.rotation;
    }

    [ConsoleCommand]
    public void StopFocus()
    {
        isFocused = false;
    }

    public void FocusCharacter(CharacterData character, Vector3 position)
    {
        isFocused = true;
        Quaternion lookRotation = Quaternion.LookRotation(position - cameraTransform.position);
        Vector3 finalRoation = lookRotation.eulerAngles;
        finalRoation.x += character.staticInfo.lookOffset.y;
        finalRoation.y += character.staticInfo.lookOffset.x;

        cameraTransform.DORotate(finalRoation, transitionSpeed);
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, fovPC, transitionSpeed);
    }

    [ConsoleCommand]
    public void FocusPC()
    {
        isFocused = true;

        lastFov = cameraRef.fieldOfView;
        lastRotation = cameraTransform.rotation;

        Quaternion lookRotation = Quaternion.LookRotation(pc.position - cameraTransform.position);
        Vector3 finalRoation = lookRotation.eulerAngles;
        finalRoation.x += offset.y;
        finalRoation.y += offset.x;

        cameraTransform.DORotate(finalRoation,transitionSpeed);
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, fovPC, transitionSpeed);
    }

    private void Update()
    {
        if(!isFocused)
        {
            cameraRef.fieldOfView = lastFov;
            cameraTransform.rotation = lastRotation;
        }
    }
}
