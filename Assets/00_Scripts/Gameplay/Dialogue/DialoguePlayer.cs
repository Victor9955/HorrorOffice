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

    CharacterStaticInfo current;

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
                    phrase.Add(str);
                    str = "";
                }
                else
                {
                    str += item;
                }
            }
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

    public void Say()
    {
        dialogueUI.gameObject.SetActive(true);
        dialogueTMP.font = defaultFont;
        StartCoroutine(Say(phrases));
    }

    IEnumerator Say(string[] phrases)
    {
        int test = 0;
        foreach (string s in phrases)
        {
            dialogueTMP.text = s;
            float duration = s.Length * current.saySpeed;

            int meshIndex = dialogueTMP.textInfo.characterInfo[0].materialReferenceIndex;
            int vertexIndex = dialogueTMP.textInfo.characterInfo[0].vertexIndex;
            Color32[] vertexColors = dialogueTMP.textInfo.meshInfo[meshIndex].colors32;
            vertexColors[vertexIndex + 0] = Color.red;
            vertexColors[vertexIndex + 1] = Color.red;
            vertexColors[vertexIndex + 2] = Color.red;
            vertexColors[vertexIndex + 3] = Color.red;

            dialogueTMP.textInfo.meshInfo[meshIndex].colors32 = vertexColors;
            dialogueTMP.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            if (test % 2 == 0)
            {
                dialogueTMP.textInfo.characterInfo[test].color = Color.red;
            }
            else
            {
                dialogueTMP.textInfo.characterInfo[test].color = Color.white;
            }
            test++;
            Tween tween = DOTween.To(() => dialogueTMP.maxVisibleCharacters, (count) => dialogueTMP.maxVisibleCharacters = count, s.Length, duration);
            tween.SetEase(Ease.Linear);
            yield return tween.WaitForCompletion();
            yield return new WaitForSeconds(current.waitBetweenPhrases);
        }
    }
}
