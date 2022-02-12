using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionCamera : MonoBehaviour
{
    public float vitesseRotation;
    public GameObject cameraMinmap;

    // Update is called once per frame
    void Update()
    {
        // Inputs qui permettent la rotation de la caméra autour de la plateforme
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0f, vitesseRotation * Time.deltaTime, 0f);
            
            cameraMinmap.transform.Rotate(0f, 0f, vitesseRotation * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0f, -vitesseRotation * Time.deltaTime, 0f);
            cameraMinmap.transform.Rotate(0f, 0f, -vitesseRotation * Time.deltaTime);
        }
    }
}
