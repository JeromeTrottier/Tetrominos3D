using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameOverGestion : MonoBehaviour
{
    public static int meilleureScore = 0;
    public Text scoreGameOver;
    public Text jetonsRestant;
    public Text meilleureScoreUI;

    // Start is called before the first frame update
    void Start()
    {
        // Si le meilleur score est plus bas que le score obtenu, on change le meilleur score au score obtenu
        if (gameOverGestion.meilleureScore < ClassificationBlocs.score) gameOverGestion.meilleureScore = ClassificationBlocs.score;
        // Affiche le score
        scoreGameOver.text = ClassificationBlocs.score.ToString();
        // Affiche le nombre de jetons restant
        jetonsRestant.text = "Il vous reste " + GestionMenu.jetons + " jetons";
        // Affiche le meilleur score
        meilleureScoreUI.text = "Meilleure Score : " + gameOverGestion.meilleureScore;
    }

}
