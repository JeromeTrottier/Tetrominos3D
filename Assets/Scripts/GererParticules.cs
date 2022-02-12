using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GererParticules : MonoBehaviour
{
    public GameObject particuleAtterissage;


    // Fonction qui joue les particules (appel�e lorsque on d�truit le parent d'un tetrominos dans ParentDestroyer.cs)
    public void jouerParticuleAtterissage(Vector3 positionParticules, Color couleurParticules)
    {
        //On active les particules
        particuleAtterissage.SetActive(true);
        //On d�termine sa position avec le param�tre donn�
        particuleAtterissage.transform.position = positionParticules;
        // On d�termine sa couleur par rapport � la couleur de l'objet d'origine
        var psColorOverLifetime = particuleAtterissage.GetComponent<ParticleSystem>().colorOverLifetime;
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(couleurParticules, 0.0f), new GradientColorKey(couleurParticules, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f)});
        psColorOverLifetime.color = grad;
        // On d�sactive les particules dans 2 secondes
        Invoke("desactiverParticules", 2f);
    }

    void desactiverParticules()
    {
        particuleAtterissage.SetActive(false);
    }


}
