using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GererParticules : MonoBehaviour
{
    public GameObject particuleAtterissage;


    // Fonction qui joue les particules (appelée lorsque on détruit le parent d'un tetrominos dans ParentDestroyer.cs)
    public void jouerParticuleAtterissage(Vector3 positionParticules, Color couleurParticules)
    {
        //On active les particules
        particuleAtterissage.SetActive(true);
        //On détermine sa position avec le paramètre donné
        particuleAtterissage.transform.position = positionParticules;
        // On détermine sa couleur par rapport à la couleur de l'objet d'origine
        var psColorOverLifetime = particuleAtterissage.GetComponent<ParticleSystem>().colorOverLifetime;
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(couleurParticules, 0.0f), new GradientColorKey(couleurParticules, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f)});
        psColorOverLifetime.color = grad;
        // On désactive les particules dans 2 secondes
        Invoke("desactiverParticules", 2f);
    }

    void desactiverParticules()
    {
        particuleAtterissage.SetActive(false);
    }


}
