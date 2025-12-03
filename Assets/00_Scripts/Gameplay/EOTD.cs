using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EOTD : MonoBehaviour
{
    
    [Header("Tri des fiches")]
    [SerializeField] public float RightSheets;//le nombre de fiches triées correctement
    [SerializeField] public float WrongSheets;//le nombre de fiches mal triées

    [Header("H.E.A.L.T.H")]
    [SerializeField] float Helpfulness;//se calcule par le ratio de fiche bien noté (donc ayant reçu une évalution positive) par rapport au nombre de fiche dans la journée
    [SerializeField] float Empathy;//Casier Positif = +1  //le nombre dépend du nombre de fois ou le joueur a accéder à une demande d'un collègue (ex: PersoA demande fiche dans "Promotion" le joueur le fait)
    //se chiffre se calcul en prenant le nombre de fois ou le joueur a eu une demande de la part des NPC par le nombre de fois ou il a accepté une requête
    [SerializeField] float Authenticity;//GameplayEventSender //le nombre de fiche ayant été trié sans que le joueur prenne en compte les demdandes de la direction ou des collègues 
    [SerializeField] float Liveliness;//Nombre de clique par jour (min max)  //le nombre d'action effectué par le joueur (ex: aller sur l'ordi, regarder autour de lui, inspecter les objets sur son bureau etc...)
    [SerializeField] float Trustworthiness;//GameplayEventSender //le nombre de fiches qui ont été triés en respectant les demande de la direction
    [SerializeField] float Hopefulness;//Résultat d'aujourdhui comparer a hier comparer a tout ce qui est au dessus //on compare les résultats du jour à ceux de la veille (augmentation = bonne note, régression = mauvaise note)

    [Header("Barrème")]//le barrème de préferences en public pour qu'on puisse tweak
    [SerializeField] float scaleH;
    [SerializeField] float scaleE;
    [SerializeField] float scaleA;
    [SerializeField] float scaleL;
    [SerializeField] float scaleT;
    [SerializeField] float scaleH2;

    //pour les notes elles doivent être un multiple de 5; on divise en 5 pour faire nos 5 notes (ex: si la valeur est 100 -> pour les notes de 0 à 20 c'est E de 20 à 40 c'est D etc...)

    [Header("NotesFinales")]//les notes sont A,B,C,D,E
    [SerializeField] string FinalGrade;//la note finale de A à E pour la journée de travail du joueur
    [SerializeField] string GradeH;//la note finale en Helpfulness
    [SerializeField] string GradeE;//la note finale en Empathy
    [SerializeField] string GradeA;//la note finale en Authenticité
    [SerializeField] string GradeL;//la note finale en Liveliness
    [SerializeField] string GradeT;//la note finale en Trustworthiness
    [SerializeField] string GradeH2;//la note finale en Hopefulness

    void Start()//il faut reset chaque jour les valeurs
    {
        RightSheets = 0;
        WrongSheets = 0;
    }

    private void DataCalculator()
    {
        if(Helpfulness < scaleH)//on calcule les notes pour chaque évaluation ici
        {

        }
    }

    private void finalReport()
    {
        //ici on envoie les données qui doivent s'afficher dans la feuille d'évaluation journalière du joueur
    }
}
