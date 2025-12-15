using DG.Tweening;
using HuntroxGames.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    [Header("Refs")]
    [SerializeField] private CanvasGroup _charThoughtObj;
    [SerializeField] private TMP_Text _textCharThought;
    [SerializeField] private DialogueData _dialogueData;
    [Header("Character Thought Settings")]
    [SerializeField] private float _fadeDuration;

    private float _initAlpha;
    private CameraMovement _cam;

    private void Awake()
    {
        if (instance == null) instance = this;
        Debug.Log(instance.name);
        _initAlpha = _charThoughtObj.GetComponentInChildren<Image>().color.a;
    }

    private void Start()
    {
        _cam = Camera.main.GetComponent<CameraMovement>();
        SetThoughtActive(false);

    }

public void SetThoughtText(string id)
{
    _dialogueData.GetDialogue(id, out string dialogue);
    _textCharThought.text = dialogue;
}
public void SetThoughtActive(bool active)
{
    if (active)
    {

        _charThoughtObj.gameObject.SetActive(active);
        DOVirtual.Float(0f, _initAlpha, _fadeDuration, (a) =>
        {
            _charThoughtObj.alpha = a;
        });
    }
    else
    {
        DOVirtual.Float(_initAlpha, 0f, _fadeDuration, (a) =>
        {
            _charThoughtObj.alpha = a;
        })
        .OnComplete(() =>
        {
            if (!_cam.ischangingFocus) _charThoughtObj.gameObject.SetActive(active);
            _cam.ischangingFocus = false;
        });
    }
}
}
