using Coffee.UIEffects;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPEffects.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private string separarionChar;
    [SerializeField] private float waitTimeBetweenPhrases;
    [SerializeField] private float speed;
    [SerializeField] private TextMeshProUGUI dialogueTMP;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private RectTransform dialogueUI;
    [SerializeField] private RectTransform dialogueBG;
    [SerializeField] private TMP_FontAsset defaultFont;
    [SerializeField] private TMPWriter writer;
    [SerializeField] private UIEffect spawnEffect;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private UnityEvent onClicked;
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
            nameTMP.font = defaultFont;
        }
        else
        {
            dialogueTMP.font = current.font;
            nameTMP.font = current.font;
        }
        doHideUI = phrases[0] == "";
        nameTMP.text = character.name;
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
            onClicked?.Invoke();
        }
        else
        {
            skipped = true;
        }
    }

    private void Update()
    {
        Debug.Log($"Is Writing : {writer.IsWriting} ");
    }

    Tweener fade;

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
            spawnEffect.transitionRate = 1f;
            fade = DOVirtual.Float(1f, 0f, fadeTime, (t) =>
            {
                spawnEffect.transitionRate = t;
            }).OnComplete(() => spawnEffect.enabled = false);
        }
    }

    private void OnDestroy()
    {
        fade.Kill();
    }

    bool skipped;
    IEnumerator Say(string[] phrases)
    {
        IsTalking = true;
        FindObjectsByType<Clickable>(FindObjectsSortMode.None).ToList().ForEach((c) => c.canFocus = false);
        foreach (string s in phrases)
        {
            skipped = false;
            dialogueTMP.text = s;
            writer.DefaultDelays.delay = current.saySpeed;
            dialogueBG.sizeDelta = dialogueBG.sizeDelta * 1.001f;
            dialogueBG.ForceUpdateRectTransforms();
            writer.StartWriter();
            yield return new WaitUntil(() => !writer.IsWriting || skipped);
            writer.SkipWriter();
            yield return new WaitForSecondsRealtime(current.timeBetweenPhrases);
        }
        FindObjectsByType<Clickable>(FindObjectsSortMode.None).ToList().ForEach((c) => c.canFocus = true);
        IsTalking = false;
    }
}
