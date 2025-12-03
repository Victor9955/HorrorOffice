using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPEffects.Components;
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
    [SerializeField] private TMPWriter writer;
    string[] phrases;

    [HideInInspector] public CharacterStaticInfo current;

    bool doHideUI = false;
    bool saidOnce = false;

    public static bool IsTalking;
    public void SetDialogue(CharacterStaticInfo character,string dialogue)
    {
        current = character;
        saidOnce = false;
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
        doHideUI = phrases[0] == "";
    }

    public void Hide()
    {
        dialogueUI.gameObject.SetActive(false);
    }

    public void BeginDialogue()
    {
        if(!saidOnce)
        {
            saidOnce = true;
            StartCoroutine(Say(phrases));
        }
        else
        {
            writer.SkipWriter();
        }
    }

    public void Say()
    {
        if(doHideUI)
        {
            dialogueUI.gameObject.SetActive(false);
        }
        else
        {
            dialogueUI.gameObject.SetActive(true);
            dialogueTMP.text = "<wave>...";
        }
        if(current.font  != null)
        {
            dialogueTMP.font = current.font;
        }
        else
        {
            dialogueTMP.font = defaultFont;
        }
    }

    public void SetFinished() => finished = true;

    bool finished;

    IEnumerator Say(string[] phrases)
    {
        IsTalking = true;
        finished = false;
        foreach (string s in phrases)
        {
            writer.StartWriter();
            dialogueTMP.text = s;
            writer.DefaultDelays.delay = current.saySpeed;
            yield return new WaitUntil(() => finished);
            finished = false;
            yield return new WaitForSecondsRealtime(current.timeBetweenPhrases);
        }
        IsTalking = false;
    }
}
