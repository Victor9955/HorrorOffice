
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Mail : MonoBehaviour
{
    bool isChecked = false;
    [SerializeField] Image backGround;
    [SerializeField] Image check;
    [SerializeField] Sprite chekedSprite;
    [SerializeField] Sprite unchekedSprite;
    [HideInInspector] public int mailId;
    Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(baseScale, 0.3f);
    }

    public void UpdateCheck()
    {
        isChecked = !isChecked;
        if (isChecked)
        {
            Check();
            Singleton.Instance<MailApp>().bin.Add(this);
        }
        else
        {
            UnCheck();
            Singleton.Instance<MailApp>().bin.Remove(this);
        }
    }

    public void OnClicked()
    {
        Singleton.Instance<MailApp>();
    }

    void Check()
    {
        isChecked = true;
        backGround.color = Color.grey;
        check.sprite = chekedSprite;
    }

    public void UnCheck()
    {
        isChecked = false;
        backGround.color = Color.white;
        check.sprite = unchekedSprite;
    }

    public void Delete()
    {
        transform.DOScale(0f, 0.3f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
