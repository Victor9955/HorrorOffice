using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class SearchingApp : MonoBehaviour, IApp
{
    [SerializeField] int size;
    [SerializeField] RectTransform contentAncor;
    [SerializeField] GridLayoutGroup content;
    [SerializeField] Button idPrefab;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] LevelSender levelSender;
    [SerializeField] DatabaseView view;
    [SerializeField] WindowAnimation viewAnim;
    Dictionary<string, GameObject> codes = new();
    Dictionary<string, SheetData> stats = new();

    [SerializeField] List<SheetData> randomStats = new();

    [SerializeField] List<string> jobsKey = new();


    private void Start()
    {
        contentAncor.sizeDelta = new Vector2(0,size * content.cellSize.y);
        RandomCode(size);
        levelSender.OnBeginDay += InitDataBase;
        levelSender.OnEndDay += EndDay;
    }

    private void InitDataBase(DayData day)
    {
        foreach (var item in day.actions)
        {
            if(item.sheets[0])
            {
                SheetData sheet = item.sheets[0];
                if (codes.ContainsKey(sheet.charachterId) || stats.ContainsKey(sheet.charachterId)) continue;
                Button cash = Instantiate(idPrefab, contentAncor);
                cash.GetComponentInChildren<TextMeshProUGUI>().text = sheet.charachterId;
                cash.onClick.AddListener(() => OnClicked(cash.gameObject));
                codes.Add(sheet.charachterId, cash.gameObject);
                stats.Add(sheet.charachterId, sheet);
            }
        }
    }

    private void EndDay()
    {
        levelSender.OnBeginDay -= InitDataBase;
        levelSender.OnEndDay -= EndDay;
    }

    public void Open()
    {

    }

    public void Close()
    {
        Searching("");
        inputField.text = "";
    }

    void RandomCode(int amount)
    {
        //[PR;HR;RD...]-[0000 to 9999]-[AAA to ZZZ]
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int interval = 9999 / amount;
        for (int i = 0; i + interval <= 9999; i += interval)
        {
            int randomNumber = UnityEngine.Random.Range(i, i + interval);
            string strNum = randomNumber.ToString();
            strNum.PadLeft(4, '0');
            string str = jobsKey[UnityEngine.Random.Range(0, jobsKey.Count)] + "-";
            str += strNum + "-";
            str += chars[UnityEngine.Random.Range(0, chars.Length)];
            str += chars[UnityEngine.Random.Range(0, chars.Length)];
            str += chars[UnityEngine.Random.Range(0, chars.Length)];

            GameObject cash = Instantiate(idPrefab.gameObject, contentAncor);
            cash.GetComponentInChildren<TextMeshProUGUI>().text = str;
            codes.Add(str, cash);
        }
    }

    public void Searching(string input)
    {
        if (input == "")
        {
            foreach (GameObject obj in codes.Values)
            {
                obj.SetActive(true);
            }
            contentAncor.sizeDelta = new Vector2(0, size * content.cellSize.y);
        }
        else
        {
            int counter = 0;
            foreach (string str in codes.Keys)
            {
                if (str.Contains(input))
                {
                    codes[str].SetActive(true);
                    counter++;
                }
                else
                {
                    codes[str].SetActive(false);
                }
            }
            contentAncor.sizeDelta = new Vector2(0, counter * content.cellSize.y);
        }
    }

    public void OnClicked(GameObject self)
    {
        viewAnim.Open();
        if (codes.ContainsValue(self))
        {
            KeyValuePair<string, GameObject> keyValuePair = codes.First(x => x.Value == self);
            if(stats.TryGetValue(keyValuePair.Key, out SheetData sheet))
            {
                view.Show(sheet);
            }
        }
    }
}
