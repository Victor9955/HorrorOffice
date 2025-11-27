using DG.Tweening;
using HuntroxGames.Utils;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


public enum FocusState
{
    Unfocused,
    PC,
    Object,
    Character
}

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

    [Header("QuitPC")]
    [SerializeField] Vector2 triggerQuit;
    [Space(10)]
    [SerializeField] private Vector2 unfocusHorizontalLimits;
    [SerializeField] private Vector2 unfocusVerticalLimits;

    [HideInInspector] public FocusState focusState = FocusState.Unfocused;
    [HideInInspector] public bool isFocusing = false;

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
            focusState = FocusState.Unfocused;
            OnFocusedChange?.Invoke(false);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PR_Computer_Focus", 0);
        });
    }

    public void FocusCharacter(CharacterData character, Vector3 position)
    {
        focusState = FocusState.Character;
        isFocusing = true;

        Quaternion lookRotation = Quaternion.LookRotation(position - cameraTransform.position);
        Vector3 finalRoation = lookRotation.eulerAngles;
        finalRoation.x += character.staticInfo.lookOffset.y;
        finalRoation.y += character.staticInfo.lookOffset.x;

        cameraTransform.DORotate(finalRoation, transitionSpeed);
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, fovPC, transitionSpeed).OnComplete(() =>
            {
                focusState = FocusState.Character;
                isFocusing = false;
            }
        );
    }

    [ConsoleCommand]
    public void FocusPC()
    {
        focusState = FocusState.PC;
        isFocusing = true;

        OnFocusedChange?.Invoke(true);
        lastRotation = cameraTransform.rotation;

        Quaternion lookRotation = Quaternion.LookRotation(pc.position - cameraTransform.position);
        Vector3 finalRoation = lookRotation.eulerAngles;
        finalRoation.x += offset.y;
        finalRoation.y += offset.x;

        cameraTransform.DOLocalRotate(finalRoation, transitionSpeed);
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, fovPC, transitionSpeed).OnComplete(() =>
        {
            focusState = FocusState.PC;
            isFocusing = false;
        }
        );
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PR_Computer_Focus", 1);
    }

    public void FocusClickable(Clickable subject)
    {
        focusState = FocusState.Object;
        isFocusing = true;

        OnFocusedChange?.Invoke(true);
        lastRotation = cameraTransform.rotation;

        Vector3 finalRot = subject.RotationInEulerAngles;

        cameraTransform.DOLocalRotate(finalRot, transitionSpeed);
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, subject.Fov, transitionSpeed).OnComplete(() =>
        {
            focusState = FocusState.Object;
            isFocusing = false;
        }
        );
        unfocusHorizontalLimits = subject.HorizontalLimits;
        unfocusVerticalLimits = subject.VerticalLimits;
    }


    private void Update()
    {
        if (isFocusing) return;

        Debug.Log("Focus State : " + focusState.ToString());
        switch (focusState)
        {
            case FocusState.Unfocused:
                cameraTransform.rotation = lastRotation;

                Vector2 mousePosition = Mouse.current.position.value;
                //Rights
                if (mousePosition.x > Screen.width - (triggerAmounts.y * Screen.width))
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
                break;

            case FocusState.PC:
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Vector2 mousePos = Mouse.current.position.value;
                    //Rights
                    if (mousePos.x > Screen.width - (triggerQuit.y * Screen.width))
                    {
                        StopFocus();
                    }

                    //Left
                    if (mousePos.x < (triggerQuit.x * Screen.width))
                    {
                        StopFocus();
                    }
                }
                break;

            case FocusState.Object:
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Vector2 mousePos = Mouse.current.position.value;

                    // right & left limits
                    bool clickedOffRightLimit = mousePos.x < (unfocusHorizontalLimits.y * Screen.width);
                    bool clickedOffLeftLimit = mousePos.x > Screen.width - (unfocusHorizontalLimits.y * Screen.width);
                    if (clickedOffLeftLimit || clickedOffRightLimit)
                    {
                        StopFocus();
                    }

                    // top & bottom limits
                    bool clickedOffTopLimit = mousePos.y < (unfocusVerticalLimits.x * Screen.height);
                    bool clickedOffBottomLimit = mousePos.y > Screen.height - (unfocusVerticalLimits.y * Screen.height);
                    if (clickedOffTopLimit || clickedOffBottomLimit)
                    {
                        StopFocus();
                    }
                }
                break;

        }

    }
}
