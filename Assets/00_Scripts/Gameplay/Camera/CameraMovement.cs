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
    [SerializeField] Vector2 triggerAmounts2;
    [SerializeField] Vector2 triggerAmounts3;
    [SerializeField] Vector2 triggerAmounts4;
    [SerializeField] Vector2 triggerAmounts5;
    [SerializeField] Vector2 rotationClamp;
    [SerializeField] float rotationSpeed;
    [SerializeField] float rotationSpeed2;
    [SerializeField] float rotationSpeed3;
    [SerializeField] float rotationSpeed4;
    [SerializeField] float rotationSpeed5;

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
    [HideInInspector] public bool ischangingFocus = false;

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
        ischangingFocus = true;
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, lastFov, transitionSpeed).SetEase(Ease.InOutCirc);
        cameraTransform.DOLocalRotate(lastRotation.eulerAngles, transitionSpeed).SetEase(Ease.InOutCirc).OnComplete(() =>
        {
            focusState = FocusState.Unfocused;
            OnFocusedChange?.Invoke(false);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PR_Computer_Focus", 0);
            ischangingFocus = false;
        });
        HUDController.instance.SetThoughtActive(false);
    }

    public void FocusCharacter(CharacterData character, Vector3 position)
    {
        focusState = FocusState.Character;
        ischangingFocus = true;

        Quaternion lookRotation = Quaternion.LookRotation(position - cameraTransform.position);
        Vector3 finalRoation = lookRotation.eulerAngles;
        finalRoation.x += character.staticInfo.lookOffset.y;
        finalRoation.y += character.staticInfo.lookOffset.x;

        cameraTransform.DORotate(finalRoation, transitionSpeed);
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, fovPC, transitionSpeed).OnComplete(() =>
            {
                focusState = FocusState.Character;
                ischangingFocus = false;
            }
        );
    }

    [ConsoleCommand]
    public void FocusPC()
    {
        focusState = FocusState.PC;
        ischangingFocus = true;

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
            ischangingFocus = false;
        }
        );
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PR_Computer_Focus", 1);
    }

    public void FocusClickable(Clickable subject)
    {
        focusState = FocusState.Object;
        ischangingFocus = true;

        OnFocusedChange?.Invoke(true);
        lastRotation = cameraTransform.rotation;

        Vector3 finalRot = subject.RotationInEulerAngles;

        cameraTransform.DOLocalRotate(finalRot, subject.FocusDuration).SetEase(Ease.InOutQuad);
        DOTween.To(() => cameraRef.fieldOfView, fov => cameraRef.fieldOfView = fov, subject.Fov, subject.FocusDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                focusState = FocusState.Object;
                ischangingFocus = false;
                unfocusHorizontalLimits = subject.HorizontalLimits;
                unfocusVerticalLimits = subject.VerticalLimits;
            }
        );
    }


    private void Update()
    {
        if (ischangingFocus) return;

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

                //Rights2
                if (mousePosition.x > Screen.width - (triggerAmounts2.y * Screen.width))
                {
                    cameraRotation += rotationSpeed2 * Time.deltaTime;
                }
                
                //Left2
                if (mousePosition.x < (triggerAmounts2.x * Screen.width))
                {
                    cameraRotation -= rotationSpeed2 * Time.deltaTime;
                }

                //Rights3
                if (mousePosition.x > Screen.width - (triggerAmounts3.y * Screen.width))
                {
                    cameraRotation += rotationSpeed3 * Time.deltaTime;
                }

                //Left3
                if (mousePosition.x < (triggerAmounts3.x * Screen.width))
                {
                    cameraRotation -= rotationSpeed3 * Time.deltaTime;
                }

                //Rights4
                if (mousePosition.x > Screen.width - (triggerAmounts4.y * Screen.width))
                {
                    cameraRotation += rotationSpeed4 * Time.deltaTime;
                }

                //Left4
                if (mousePosition.x < (triggerAmounts4.x * Screen.width))
                {
                    cameraRotation -= rotationSpeed4 * Time.deltaTime;
                }

                //Rights5
                if (mousePosition.x > Screen.width - (triggerAmounts5.y * Screen.width))
                {
                    cameraRotation += rotationSpeed5 * Time.deltaTime;
                }

                //Left5
                if (mousePosition.x < (triggerAmounts5.x * Screen.width))
                {
                    cameraRotation -= rotationSpeed5 * Time.deltaTime;
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
