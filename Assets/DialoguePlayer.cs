using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private string separarionChar;
    [SerializeField] private float waitTimeBetweenPhrases;
    [SerializeField] private float speed;
    [SerializeField] private TextMeshProUGUI dialogueTMP;
    string[] phrases;

    public void SetDialogue(string dialogue)
    {
        dialogueTMP.transform.parent.gameObject.SetActive(false);
        if (dialogue.Contains(separarionChar))
        {
            List<string> test = new();
            string str = "";
            foreach (char item in dialogue)
            {
                if(item == separarionChar[0])
                {
                    test.Add(str);
                    str = "";
                }
                else
                {
                    str += item;
                }
            }
            phrases = test.ToArray();
        }
        else
        {
            phrases = new string[] { dialogue };
        }
        
    }

    public void Say()
    {
        dialogueTMP.transform.parent.gameObject.SetActive(true);
        StartCoroutine(Say(phrases));
    }

    IEnumerator Say(string[] phrases)
    {
        foreach (string s in phrases)
        {
            float duration = s.Length * speed;
            dialogueTMP.text = "";
            Tween tween = DOTween.To(() => dialogueTMP.text, (str) => dialogueTMP.text = str, s, duration);
            yield return tween.WaitForCompletion();
            yield return new WaitForSeconds(waitTimeBetweenPhrases);
        }
    }
}
