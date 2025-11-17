using UnityEngine;
using TMPro;

public class DatabaseView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _characterIdTMP;
    [SerializeField] TextMeshProUGUI _characterName;
    [SerializeField] TextMeshProUGUI _testData;

    public void Show(SheetData data)
    {
        _characterName.text = data.characterName;
        _characterIdTMP.text = data.charachterId;
        _testData.text = "A fait " + data.pausesNumber.ToString() + " pauses";
        //public string beginTime;
        //public string breakfastTime;
        //[TextArea(1, 20)] public string medicineTime;
        //public string HGrade;
        //public string EGrade;
        //public string AGrade;
        //public string LGrade;
        //public string TGrade;
        //public string HGradeTwo;
    }
}
