
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mail : MonoBehaviour
{
    [SerializeField] Image pp;
    [SerializeField] TextMeshProUGUI title;
    [HideInInspector] public int mailId;
    [HideInInspector] public MailData mailData;
    Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(baseScale, 0.3f);
    }

    public void OnClicked()
    {
        Singleton.Instance<MailApp>().OpenMail(mailData);
    }
}
