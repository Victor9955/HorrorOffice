using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
class Stat
{
    public int maxGrade;
    public TextMeshProUGUI tmp;
    [HideInInspector] public int value { get { return _value; } set { _value = Mathf.Clamp(_value + value, 0, maxGrade); } }
    private int _value;

    public void ShowGrade(List<string> grades)
    {
        float normalizedGrade = Mathf.Clamp01((float)value / (float)maxGrade);
        int index = Mathf.FloorToInt((1f - normalizedGrade) * (grades.Count - 1));
        index = Mathf.Clamp(index, 0, grades.Count - 1);
        tmp.text = grades[index];
    }
}

public class EvaluationReport : MonoBehaviour
{
    [SerializeField] GameplayEventSender gameplayEventSender;
    [SerializeField] LevelSender levelSender;
    [SerializeField] WindowAnimation windowAnimation;
    [SerializeField] List<string> grades;
    [SerializeField] TextMeshProUGUI binderCount;

    int RightSheets;//le nombre de fiches triées correctement
    int WrongSheets;//le nombre de fiches mal triées

    [Header("H.E.A.L.T.H")]
    [SerializeField] Stat Helpfulness;//se calcule par le ratio de fiche bien noté (donc ayant reçu une évalution positive) par rapport au nombre de fiche dans la journée
    [SerializeField] Stat Empathy;//Casier Positif = +1  //le nombre dépend du nombre de fois ou le joueur a accéder à une demande d'un collègue (ex: PersoA demande fiche dans "Promotion" le joueur le fait)
    //se chiffre se calcul en prenant le nombre de fois ou le joueur a eu une demande de la part des NPC par le nombre de fois ou il a accepté une requête
    [SerializeField] Stat Authenticity;//GameplayEventSender //le nombre de fiche ayant été trié sans que le joueur prenne en compte les demdandes de la direction ou des collègues 
    [SerializeField] Stat Liveliness;//Nombre de clique par jour (min max)  //le nombre d'action effectué par le joueur (ex: aller sur l'ordi, regarder autour de lui, inspecter les objets sur son bureau etc...)
    [SerializeField] Stat Trustworthiness;//GameplayEventSender //le nombre de fiches qui ont été triés en respectant les demande de la direction
    [SerializeField] Stat Hopefulness;//Résultat d'aujourdhui comparer a hier comparer a tout ce qui est au dessus //on compare les résultats du jour à ceux de la veille (augmentation = bonne note, régression = mauvaise note)

    [SerializeField] Stat PlayerStat;
    static float lastHopefulness;

    private void Start()
    {
        levelSender.OnBeginDay += OnBeginDay;
        RightSheets = 0;
        WrongSheets = 0;
        Liveliness.value = 0;
        Helpfulness.value = 0;
        Empathy.value = 0;
        Authenticity.value = 0;
        Trustworthiness.value = 0;
        Hopefulness.value = 0;
        levelSender.OnEndDay += OnEndDay;
        gameplayEventSender.AddAuthenticityEvent += (amount) => { Authenticity.value += amount; };
        gameplayEventSender.AddHelpfulnessEvent += (amount) => { Helpfulness.value += amount; };
        gameplayEventSender.AddEmpathyEvent += (amount) => { Empathy.value += amount; };
        gameplayEventSender.AddTrustworthinessEvent += (amount) => { Trustworthiness.value += amount; };
    }

    DayData currentDayData;

    private void OnBeginDay(DayData obj)
    {
        levelSender.OnBeginDay -= OnBeginDay;
        currentDayData = obj;
    }

    private void OnEndDay()
    {
        levelSender.OnEndDay -= OnEndDay;
        windowAnimation.Open();
        ShowGrades();
    }

    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            Liveliness.value++;
        }
    }

    public void ShowGrades()
    {
        Helpfulness.ShowGrade(grades);
        Empathy.ShowGrade(grades);
        Authenticity.ShowGrade(grades);
        Liveliness.ShowGrade(grades);
        Trustworthiness.ShowGrade(grades);
        Hopefulness.ShowGrade(grades);

        Helpfulness.tmp.text = currentDayData.Helpfulness;
        Empathy.tmp.text = currentDayData.Empathy;
        Authenticity.tmp.text = currentDayData.Authenticity;
        Liveliness.tmp.text = currentDayData.Liveliness;
        Trustworthiness.tmp.text = currentDayData.Trustworthiness;
        Hopefulness.tmp.text = currentDayData.Hopefulness;

        PlayerStat.maxGrade = RightSheets + WrongSheets;
        PlayerStat.value = RightSheets;
        PlayerStat.ShowGrade(grades);

        binderCount.text = $"Properly sorted files : {RightSheets} \r\nIncorrectly sorted files : {WrongSheets}";

        /*
        if (LevelSender.day == 0)
        {
            Hopefulness.value = Hopefulness.maxGrade / 2;
        }
        else
        {
            float currentGrade = ((Helpfulness.value / Helpfulness.maxGrade) +  (Empathy.value / Empathy.maxGrade) + (Authenticity.value / Authenticity.maxGrade) + (Liveliness.value / Liveliness.maxGrade) + (Trustworthiness.value / Trustworthiness.maxGrade)) / 5f;
            Hopefulness.value = Mathf.FloorToInt((lastHopefulness + (0.5f - currentGrade)) * Hopefulness.maxGrade);
            lastHopefulness = 0.5f - currentGrade;
        }*/


    }

    public void ChangeBinderValue(bool isRight)
    {
        if(isRight)
        {
            RightSheets++;
        }
        else
        {
            WrongSheets++;
        }
    }
}
