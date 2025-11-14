using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleTMP;
    [SerializeField] TextMeshProUGUI mailTextTMP;
    [SerializeField] Image pp;

    private void OnEnable()
    {
        titleTMP.enabled = false;
        mailTextTMP.enabled = false;
        pp.enabled = false;
    }

    public void Show(MailData mailData)
    {
        titleTMP.enabled = true;
        mailTextTMP.enabled = true;
        pp.enabled = true;

        titleTMP.text = mailData.title;
        mailTextTMP.text = mailData.mailText;
        pp.sprite = mailData.character.staticInfo.mailPP;
    }
}
