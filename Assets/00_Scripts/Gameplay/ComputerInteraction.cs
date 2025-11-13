using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    [SerializeField] List<MeshRenderer> computerMeshs;
    [SerializeField] InputAction quitAction;
    [SerializeField] Collider interactionCol;
    List<Material> materials = new();

    private void Start()
    {
        quitAction.Enable();
        quitAction.performed += (Input) =>
        {
            Camera.main.GetComponent<CameraMovement>().StopFocus();
            interactionCol.enabled = true;
        };

        computerMeshs.ForEach((m) =>
        {
            foreach (var item in m.materials)
            {
                if (item.HasFloat("_Size"))
                {
                    materials.Add(item);
                }
            }

        });
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
            
        });
    }

    public void StopOver()
    {
        float size = materials[0].GetFloat("_Size");
        DOTween.To(() => size, _ => size = _, 1f, 0.25f).OnUpdate(() =>
        {
            materials.ForEach(m => m.SetFloat("_Size", size));
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
