using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GererScenes : MonoBehaviour
{
    // son du bouton
    public AudioClip sonBouton;

    public void changerScene(string prochaineScene)
    {
        //Lorsqu'on change de scène, on joue le son du bouton
        GetComponent<AudioSource>().PlayOneShot(sonBouton);
        // On change de scène dans 0.5 seconde
        Invoke(prochaineScene, 0.5f);
    }
    public void lancerJeu()
    {
        // Lorsque on lance le jeu, on détruit toutes les musiques
        GameObject[] musiqueObjets = GameObject.FindGameObjectsWithTag("musique");
        foreach (GameObject musique in musiqueObjets) Destroy(musique.gameObject);
        // On perd un jeton
        GestionMenu.jetons--;
        // S'il ne reste plus de jetons, on lance la scène de GameOver, sinon on lance le jeu
        if (GestionMenu.jetons > 0) 
        {
            SceneManager.LoadScene("SampleScene");
        } else
        {
            SceneManager.LoadScene("aucunJetonRestantScene");
        }
            
    }
    // Lance la scène de tutoriel
    public void lancerTuto()
    {
        SceneManager.LoadScene("TutorielScene");
    }
    // Lance le menu
    public void lancerMenu()
    {
        SceneManager.LoadScene("IntroScene");
    }
    // Reset les jetons
    public void resetJetons() 
    {
        GestionMenu.jetons = 5;
    }
    

}
