using UnityEngine;
using TMPro;

public class DatabaseView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _characterIdTMP;
    [SerializeField] TextMeshProUGUI _characterName;
    [SerializeField] TextMeshProUGUI _pauses;
    [SerializeField] TextMeshProUGUI _grades;
    [SerializeField] TextMeshProUGUI _times;
    [SerializeField] TextMeshProUGUI _medsTimes;

    public void Show(SheetData data)
    {
        _characterName.text = data.characterName;
        _characterIdTMP.text = data.charachterId;
        _pauses.text = "A fait " + data.pausesNumber.ToString() + " pauses";
        _times.text = "Begin time : " + data.beginTime + "\n" + "Breakfast : " + data.breakfastTime;
        _medsTimes.text = data.medicineTime;

        _grades.text = "";
        _grades.text += "H: " + data.HGrade + "\n";
        _grades.text += "E: " + data.EGrade + "\n";
        _grades.text += "A: " + data.AGrade + "\n";
        _grades.text += "L: " + data.LGrade + "\n";
        _grades.text += "T: " + data.TGrade + "\n";
        _grades.text += "H: " + data.HGradeTwo + "\n";
    }
}
