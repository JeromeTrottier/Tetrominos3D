using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentDestroyer : MonoBehaviour
{
    public Transform nouveauParent;
    public GameObject joueur;
    public GameObject ombreJoueurParent;
    // Update is called once per frame
    void Start()
    {
        nouveauParent = GameObject.Find("Blocs").transform;
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Si le joueur rentre en contact avec le sol et que sa position est aligné : 
        if(joueur.GetComponent<DeplacementJoueur>().positionAligne == true)
        {
            if (collision.gameObject && collision.gameObject.tag != "Mur")
            {
                joueur.GetComponent<AudioSource>().Play();
                joueur.GetComponent<GererParticules>().jouerParticuleAtterissage(collision.contacts[0].point, gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color);
                Debug.Log(gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color);
                // On change de parent le tetrominos du joueur, on lui ajoute le tag objetJoueur pour être détecter par la fonction de classification
                for (int i = 0; i < transform.childCount; i++)
                {
                    gameObject.transform.SetParent(nouveauParent, false);
                    gameObject.tag = "objetJoueur";
                    nouveauParent.tag = "contientObjetJoueur";
                }
                Destroy(ombreJoueurParent.transform.GetChild(0).gameObject);
            }
        }
    }
}
