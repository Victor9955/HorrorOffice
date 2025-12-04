
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Mail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleTMP;
    [SerializeField] Color nameColor;
    [HideInInspector] public MailData mailData;
    [HideInInspector] public MailApp mailAppRef;
    [HideInInspector] public bool wasOpened = false;
    

    Vector3 baseScale;
    private void Start()
    {
        baseScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(baseScale, 0.3f);
        titleTMP.text = mailData.title;
        titleTMP.text += $"\n <color=#{nameColor.ToHexString()}><size=12.5><" + mailData.character.staticInfo.name + ">";
    }

    public void OnClicked()
    {
        mailAppRef.OpenMail(this);
        wasOpened = true;
    }
}
