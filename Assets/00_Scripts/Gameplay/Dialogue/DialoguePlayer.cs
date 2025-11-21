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
    [SerializeField] private TMP_FontAsset defaultFont;
    string[] phrases;

    CharacterStaticInfo current;

    public void SetDialogue(CharacterStaticInfo character,string dialogue)
    {
        current = character;
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
        
        if(current.font == null)
        {
            dialogueTMP.font = defaultFont;
        }
        else
        {
            dialogueTMP.font = current.font;
        }
    }

    public void Say()
    {
        dialogueTMP.transform.parent.gameObject.SetActive(true);
        dialogueTMP.font = defaultFont;
        StartCoroutine(Say(phrases));
    }

    IEnumerator Say(string[] phrases)
    {
        foreach (string s in phrases)
        {
            dialogueTMP.text = s;
            float duration = s.Length * current.saySpeed;
            dialogueTMP.maxVisibleCharacters = 0;
            Tween tween = DOTween.To(() => dialogueTMP.maxVisibleCharacters, (count) => dialogueTMP.maxVisibleCharacters = count, s.Length, duration);
            yield return tween.WaitForCompletion();
            yield return new WaitForSeconds(current.waitBetweenPhrases);
        }
    }
}
