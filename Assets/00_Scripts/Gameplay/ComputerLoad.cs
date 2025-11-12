using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ComputerLoad : MonoBehaviour
{
    [SerializeField] string username;
    [SerializeField] int passwordLength;

    [SerializeField] float loadTime;
    bool isLoading = false;

    [Header("Refs")]
    [SerializeField] TextMeshProUGUI usernameTMP;
    [SerializeField] TextMeshProUGUI passwordTMP;
    [SerializeField] WindowAnimation windowAnim;

    public void Load()
    {
        if(!isLoading)
        {
            isLoading = true;
            StartCoroutine(LoadRoutine());
        }
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
