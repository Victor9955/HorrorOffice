using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class TestGlitch : MonoBehaviour
{
    [SerializeField] RectTransform rect;

    [Button]
    void ShakeScreent()
    {
        DOTween.Shake(() => rect.sizeDelta, (vec) => rect.sizeDelta = vec, 0.5f,Vector2.one * 300,100);
    }
}
