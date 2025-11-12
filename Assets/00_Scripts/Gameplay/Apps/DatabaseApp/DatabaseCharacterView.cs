using UnityEngine;
using TMPro;

public class DatabaseCharacterView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _characterIdTMP;
    [SerializeField] TextMeshProUGUI _characterName;
    [SerializeField] TextMeshProUGUI _testData;

    public void View(SheetData data)
    {
        _characterName.text = data.characterName;
        _characterIdTMP.text = data.charachterId;
        _testData.text = "A fait " + data.pausesNumber.ToString() + " pauses";

    }
}
