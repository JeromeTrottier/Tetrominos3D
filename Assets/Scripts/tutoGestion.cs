using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tutoGestion : MonoBehaviour
{
    public GameObject regles;
    public GameObject controles;

    // Fonctions qui affichent le bon panel selon le bouton cliqué
    public void afficherRegles()
    {
        controles.SetActive(false);
        regles.SetActive(true);
    }
    public void afficherControles()
    {
        regles.SetActive(false);
        controles.SetActive(true);
    }
}
