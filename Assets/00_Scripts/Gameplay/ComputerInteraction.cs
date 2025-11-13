using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ComputerInteraction : MonoBehaviour
{
    [SerializeField] string username;
    [SerializeField] int passwordLength;

    [SerializeField] float loadTime;
    bool isLoading = false;

    [Header("Refs")]
    [SerializeField] TextMeshProUGUI usernameTMP;
    [SerializeField] TextMeshProUGUI passwordTMP;
    [SerializeField] WindowAnimation windowAnim;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] InputAction quitAction;
    [SerializeField] Collider interactionCol;

    private void Start()
    {
        quitAction.Enable();
        quitAction.performed += (Input) =>
        {
            Camera.main.GetComponent<CameraMovement>().StopFocus();
            interactionCol.enabled = true;
        };
    }

    private void OnDestroy()
    {
        quitAction.Disable();
    }

    public void Clicked()
    {
        if(!isLoading)
        {
            isLoading = true;
            StartCoroutine(LoadRoutine());
        }
        Camera.main.GetComponent<CameraMovement>().FocusPC();
        StopOver();
        interactionCol.enabled = false;
    }

    public void Over()
    {
        float size = 1f;
        DOTween.To(() => size, _ => size = _, 1.05f, 0.25f).OnUpdate(() =>
        {
            meshRenderer.materials[1].SetFloat("_Size",size);
        });
    }

    public void StopOver()
    {
        float size = meshRenderer.materials[1].GetFloat("_Size");
        DOTween.To(() => size, _ => size = _, 1f, 0.25f).OnUpdate(() =>
        {
            meshRenderer.materials[1].SetFloat("_Size", size);
        });
    }
    
    IEnumerator LoadRoutine()
    {
        string usernameCash = "";
        string passwordCash = "";
        foreach (char item in username)
        {
            usernameCash += item;
            usernameTMP.text = usernameCash;
            yield return new WaitForSeconds(Random.Range(0.08f, 0.2f));
        }

        for (int i = 0; i < passwordLength; i++)
        {
            passwordCash += "*";
            passwordTMP.text = passwordCash;
            yield return new WaitForSeconds(Random.Range(0.08f, 0.2f));
        }

        yield return new WaitForSeconds(loadTime);

        windowAnim.Close();
    }
}
