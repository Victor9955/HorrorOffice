using DG.Tweening;
using HuntroxGames.Utils;
using NaughtyAttributes;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Camera cameraRef;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float transitionSpeed;

    [Header("Movement")]
    [SerializeField] Vector2 triggerAmounts;
    [SerializeField] Vector2 rotationClamp;
    [SerializeField] float rotationSpeed;

    [Header("PC")]
    [SerializeField] Vector2 offset;
    [SerializeField] Transform pc;
    [SerializeField] float fovPC;

    bool isFocused = false;

    Quaternion lastRotation;
    float lastFov;

    float cameraRotation;

    private void Start()
    {
        lastFov = cameraRef.fieldOfView;
        lastRotation = cameraTransform.localRotation;
        cameraRotation = lastRotation.y;
    }

    [ConsoleCommand]
    public void StopFocus()
    {
        cameraTransform.DOLocalRotate(lastRotation.eulerAngles, transitionSpeed).OnComplete(() =>
        {
            isFocused = false;
        });
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, lastFov, transitionSpeed);
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

        cameraTransform.DOLocalRotate(finalRoation,transitionSpeed);
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, fovPC, transitionSpeed);
    }

    private void Update()
    {
        if(!isFocused)
        {
            cameraTransform.rotation = lastRotation;

            Vector2 mousePosition = Mouse.current.position.value;
            //Rights
            if(mousePosition.x > Screen.width - (triggerAmounts.y * Screen.width))
            {
                cameraRotation += rotationSpeed * Time.deltaTime;
            }

            //Left
            if (mousePosition.x < (triggerAmounts.x * Screen.width))
            {
                cameraRotation -= rotationSpeed * Time.deltaTime;
            }

            cameraRotation = Mathf.Clamp(cameraRotation, rotationClamp.x, rotationClamp.y);
            Vector3 finalRotation = lastRotation.eulerAngles;
            finalRotation.y = cameraRotation;
            lastRotation.eulerAngles = finalRotation;
        }
    }
}
