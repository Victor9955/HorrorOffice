
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Mail : MonoBehaviour
{
    public MailApp appRef;
    bool isChecked = false;
    [SerializeField] Image backGround;
    [SerializeField] Image check;
    [SerializeField] Sprite chekedSprite;
    [SerializeField] Sprite unchekedSprite;
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
            appRef.bin.Add(this);
        }
        else
        {
            UnCheck();
            appRef.bin.Remove(this);
        }
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
