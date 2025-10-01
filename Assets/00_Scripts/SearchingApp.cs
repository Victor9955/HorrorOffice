using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class SearchingApp : MonoBehaviour, IApp
{
    [SerializeField] GameObject toShow;
    [SerializeField] int size;
    [SerializeField] RectTransform parent;
    [SerializeField] RectTransform contentAncor;
    [SerializeField] GridLayoutGroup content;
    [SerializeField] RectTransform idPrefab;
    [SerializeField] TMP_InputField inputField;
    Dictionary<string, GameObject> codes = new();

    private void Start()
    {
        if(parent == null)
        {
            parent = GetComponent<RectTransform>();
        }
        contentAncor.sizeDelta = new Vector2(0,size * content.cellSize.y);
        RandomCode(size);
    }

    public void Open()
    {
        parent.DOScale(1f, 0.3f);
        toShow.SetActive(true);
    }

    public void Close()
    {
        parent.DOScale(0f, 0.3f).OnComplete(() => toShow.SetActive(false));
        Searching("");
        inputField.text = "";
    }

    void RandomCode(int amount)
    {
        int interval = (61439 / (amount * 4));
        string str = "";
        int j = 0;
        for (int i = 4096; i + interval <= 65535; i += interval)
        {
            int randomNumber = UnityEngine.Random.Range(i, i + interval);
            if(j == 3)
            {
                GameObject cash = Instantiate(idPrefab.gameObject, contentAncor);
                cash.GetComponentInChildren<TextMeshProUGUI>().text = str;
                codes.Add(str, cash);
                j = 0;
                str = "";
            }
            else
            {
                if(j == 0)
                {
                    str += Convert.ToString(randomNumber, 16);
                }
                else
                {
                    str += ":" + Convert.ToString(randomNumber, 16);
                }
                j++;
            }
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
                if (str.Contains(input, StringComparison.InvariantCultureIgnoreCase))
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
}
