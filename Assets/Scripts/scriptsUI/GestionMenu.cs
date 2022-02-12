using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionMenu : MonoBehaviour
{
    public static int jetons = 5;
    public Text jetonsRestants;
    // Start is called before the first frame update
    void Start()
    {
        // Affiche le nombre de jetons restant
        jetonsRestants.text = "Vous avez " + GestionMenu.jetons + " jetons";
    }
}
