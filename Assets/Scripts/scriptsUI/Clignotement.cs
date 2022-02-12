using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clignotement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Fait clignoter à chaque seconde
        InvokeRepeating("clignoter", 0f, 1f);
    }

    // Fonction qui fait clignoter le text sur lequel ce script est posé
    void clignoter()
    {
        if (gameObject.GetComponent<Text>().color.a == 1)
        {
            gameObject.GetComponent<Text>().color = new Color(255, 0, 128, 0);
        } else if (gameObject.GetComponent<Text>().color.a == 0)
        {
            gameObject.GetComponent<Text>().color = new Color(255, 0, 128, 1);
        }
        
    }
}
