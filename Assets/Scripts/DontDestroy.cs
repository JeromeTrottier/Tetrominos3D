using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        //On cherche des objets qui ont le tag Musique
        GameObject[] objs = GameObject.FindGameObjectsWithTag("musique");

        // Si il y a plus d'un objet, on détruit cet objet
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        // Sinon, on le garde pour la prochaine scène
        DontDestroyOnLoad(this.gameObject);
    }
}
