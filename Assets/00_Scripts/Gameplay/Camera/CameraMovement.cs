using DG.Tweening;
using HuntroxGames.Utils;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Camera cameraRef;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float transitionSpeed;

    [Header("Movement")]
    [SerializeField] Vector2 triggerAmountsA;
    [SerializeField] Vector2 triggerAmountsB;
    [SerializeField] Vector2 triggerAmountsC;
    [SerializeField] Vector2 triggerAmountsD;
    [SerializeField] Vector2 triggerAmountsE;
    [SerializeField] Vector2 rotationClamp;
    [SerializeField] float rotationSpeedA;
    [SerializeField] float rotationSpeedB;
    [SerializeField] float rotationSpeedC;
    [SerializeField] float rotationSpeedD;
    [SerializeField] float rotationSpeedE;

    [Header("PC")]
    [SerializeField] Vector2 offset;
    [SerializeField] Transform pc;
    [SerializeField] float fovPC;

    [Header("QuitPC")]
    [SerializeField] Vector2 triggerQuit;

    [HideInInspector] public bool isFocused = false;

    public event Action<bool> OnFocusedChange;

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
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, lastFov, transitionSpeed);
        cameraTransform.DOLocalRotate(lastRotation.eulerAngles, transitionSpeed).OnComplete(() =>
        {
            isFocused = false;
            OnFocusedChange?.Invoke(isFocused);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PR_Computer_Focus", 0);
        });
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
        OnFocusedChange?.Invoke(isFocused);
        lastRotation = cameraTransform.rotation;

        Quaternion lookRotation = Quaternion.LookRotation(pc.position - cameraTransform.position);
        Vector3 finalRoation = lookRotation.eulerAngles;
        finalRoation.x += offset.y;
        finalRoation.y += offset.x;

        cameraTransform.DOLocalRotate(finalRoation, transitionSpeed);
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, fovPC, transitionSpeed);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PR_Computer_Focus", 1);
    }

    private void Update()
    {
        if(!isFocused)
        {
            cameraTransform.rotation = lastRotation;

            Vector2 mousePosition = Mouse.current.position.value;
            //Rights
            if(mousePosition.x > Screen.width - (triggerAmountsA.y * Screen.width))
            {
                cameraRotation += rotationSpeedA * Time.deltaTime;
            }

            //Left
            if (mousePosition.x < (triggerAmountsA.x * Screen.width))
            {
                cameraRotation -= rotationSpeedA * Time.deltaTime;
            }

            //Rights
            if (mousePosition.x > Screen.width - (triggerAmountsB.y * Screen.width))
            {
                cameraRotation += rotationSpeedB * Time.deltaTime;
            }

            //Left
            if (mousePosition.x < (triggerAmountsB.x * Screen.width))
            {
                cameraRotation -= rotationSpeedB * Time.deltaTime;
            }
            //Rights
            if (mousePosition.x > Screen.width - (triggerAmountsC.y * Screen.width))
            {
                cameraRotation += rotationSpeedC * Time.deltaTime;
            }

            //Left
            if (mousePosition.x < (triggerAmountsC.x * Screen.width))
            {
                cameraRotation -= rotationSpeedC * Time.deltaTime;
            }

            //Rights
            if (mousePosition.x > Screen.width - (triggerAmountsD.y * Screen.width))
            {
                cameraRotation += rotationSpeedD * Time.deltaTime;
            }

            //Left
            if (mousePosition.x < (triggerAmountsD.x * Screen.width))
            {
                cameraRotation -= rotationSpeedD * Time.deltaTime;
            }

            //Rights
            if (mousePosition.x > Screen.width - (triggerAmountsE.y * Screen.width))
            {
                cameraRotation += rotationSpeedE * Time.deltaTime;
            }

            //Left
            if (mousePosition.x < (triggerAmountsE.x * Screen.width))
            {
                cameraRotation -= rotationSpeedE * Time.deltaTime;
            }

            cameraRotation = Mathf.Clamp(cameraRotation, rotationClamp.x, rotationClamp.y);
            Vector3 finalRotation = lastRotation.eulerAngles;
            finalRotation.y = cameraRotation;
            lastRotation.eulerAngles = finalRotation;
        }
        else
        {
            if(Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mousePosition = Mouse.current.position.value;
                //Rights
                if (mousePosition.x > Screen.width - (triggerQuit.y * Screen.width))
                {
                    StopFocus();
                }

                //Left
                if (mousePosition.x < (triggerQuit.x * Screen.width))
                {
                    StopFocus();
                }
            }
        }
    }
}
