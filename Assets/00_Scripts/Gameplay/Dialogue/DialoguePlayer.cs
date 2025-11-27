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
    [SerializeField] private RectTransform dialogueUI;
    [SerializeField] private TMP_FontAsset defaultFont;
    string[] phrases;

    [HideInInspector] public CharacterStaticInfo current;

    public void SetDialogue(CharacterStaticInfo character,string dialogue)
    {
        current = character;
        dialogueUI.gameObject.SetActive(false);
        if (dialogue.Contains(separarionChar))
        {
            List<string> phrase = new();
            string str = "";
            foreach (char item in dialogue)
            {
                if(item == separarionChar[0])
                {
                    str = str.TrimEnd();
                    str = str.TrimStart();
                    phrase.Add(str);
                    str = "";
                }
                else
                {
                    str += item;
                }
            }
            phrase.Add(str);
            phrases = phrase.ToArray();
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

    public void Hide()
    {
        dialogueTMP.transform.parent.gameObject.SetActive(false);
    }


    public void Say()
    {
        dialogueUI.gameObject.SetActive(true);
        dialogueTMP.font = defaultFont;
        StartCoroutine(Say(phrases));
    }

    IEnumerator Say(string[] phrases)
    {
        foreach (string s in phrases)
        {
            dialogueTMP.text = s;
            dialogueTMP.maxVisibleCharacters = 0;
            float duration = s.Length * current.saySpeed;
            Tween tween = DOTween.To(() => dialogueTMP.maxVisibleCharacters, (count) => dialogueTMP.maxVisibleCharacters = count, s.Length, duration);
            tween.SetEase(Ease.Linear);
            yield return tween.WaitForCompletion();
            yield return new WaitForSecondsRealtime(current.waitBetweenPhrases);
        }
    }
}
