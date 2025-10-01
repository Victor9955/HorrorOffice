
using UnityEngine;
using UnityEngine.UI;

public class Mail : MonoBehaviour
{
    bool isChecked = false;
    [SerializeField] Image backGround;
    [SerializeField] Image check;
    [SerializeField] Sprite chekedSprite;
    [SerializeField] Sprite unchekedSprite;

    public void UpdateCheck()
    {
        isChecked = !isChecked;
        if (isChecked) Check();
        else UnCheck();
    }

    void Check()
    {
        backGround.color = Color.grey;
        check.sprite = chekedSprite;
    }

    void UnCheck()
    {
        backGround.color = Color.white;
        check.sprite = unchekedSprite;

    }
}
