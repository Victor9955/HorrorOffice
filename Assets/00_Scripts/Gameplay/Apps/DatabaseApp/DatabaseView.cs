using UnityEngine;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
struct TMPInput
{
    public string before;
    public TextMeshProUGUI input;
}

public class DatabaseView : MonoBehaviour
{
    [SerializeField] TMPInput id;
    [SerializeField] TMPInput characterName;
    [SerializeField] TMPInput characterDepartement;
    [SerializeField] TMPInput characterService;
    [SerializeField] TMPInput characterPosition;

    [SerializeField] List<TextMeshProUGUI> _grades = new();

    public void Show(SheetData data)
    {
        id.input.text = id.before + data.charachterId;
        characterName.input.text = characterName.before + data.characterName;

        _grades[0].text = data.HGrade;
        _grades[1].text = data.EGrade;
        _grades[2].text = data.AGrade;
        _grades[3].text = data.LGrade;
        _grades[4].text = data.TGrade;
        _grades[5].text = data.HGradeTwo;
    }
}
