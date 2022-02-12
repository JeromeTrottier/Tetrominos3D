using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionUI : MonoBehaviour
{
    public Text prochainBlocText;
    public GameObject joueur;
    public GameObject prochainObjetUIEspace;
    public GameObject objetStockerUIEspace;
    private GameObject objetProchainUI;
    private GameObject objetHold;
    private int layerUI = 5;

    public Text scoreUI;
    // Update is called once per frame
    void Update()
    {
        rotationPassive(objetProchainUI);
        if (objetStockerUIEspace.transform.childCount == 1)
        {
            rotationPassive(objetHold);
        }
        scoreUI.text = "Score : " + ClassificationBlocs.score.ToString();
    }
    // Fonction qui gère la boite UI du prochain objet
    public void changerProchainObjetUI()
    {
        // Si la boite contient déjà un objet lorsqu'elle est appelé, on détruit cet objet
        if (prochainObjetUIEspace.transform.childCount >= 1)
        {
            Destroy(prochainObjetUIEspace.transform.GetChild(0).gameObject);
        }
        // On va chercher quel sera le prochain objet à l'aide de la variable attribué dans le script de joueur
        GameObject prochainBloc = joueur.GetComponent<DeplacementJoueur>().prochainObjet;
        // On instantie le prochain Bloc à l'intérieur de la boite
        GameObject prochainObjetUI = Instantiate(prochainBloc, prochainBloc.transform.position = new Vector3(0f, 0f, 0f), Quaternion.Euler(-15, 0, 0), prochainObjetUIEspace.transform);
        // On associe l'objet instantier au layer UI pour pouvoir le voir
        prochainObjetUI.layer = layerUI;
        // Pour tout les cubes enfants du prochain Objet, on let met sur le layer UI pour pouvoir les observer
        for (int i = 0; i < prochainObjetUI.transform.childCount; i++)
        {
            var cubeIndex = prochainObjetUI.transform.GetChild(i);
            cubeIndex.gameObject.layer = layerUI;
        }
        // On positionne l'objet dans la boite
        prochainObjetUI.transform.localPosition = new Vector3(0f, 0f, 0.2f);
        // on le Rescale pour qu'il ne soit pas trop gros
        prochainObjetUI.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        // On désactive la gravité
        prochainObjetUI.GetComponent<Rigidbody>().useGravity = false;
        // On associe l'objet instantié à la variable du prochainObjet dans le UI
        objetProchainUI = prochainObjetUI;
        // On active l'objet
        prochainObjetUI.SetActive(true);
    }
    // Fonction qui gère la boite UI de l'objet en Hold
    public void changerObjetStocke()
    {

        GameObject blocEnHold = joueur.GetComponent<DeplacementJoueur>().objetStocker.gameObject;
        GameObject blocEnHoldUI = Instantiate(blocEnHold, blocEnHold.transform.position = new Vector3(0f, 0f, 0f), Quaternion.identity, objetStockerUIEspace.transform);
        blocEnHold.transform.Rotate(-15f, 0f, 0f);
        blocEnHoldUI.layer = layerUI;
        for (int i = 0; i < blocEnHoldUI.transform.childCount; i++)
        {
            var cubeIndex = blocEnHoldUI.transform.GetChild(i);
            cubeIndex.gameObject.layer = layerUI;
            //cubeIndex.GetChild(0).gameObject.layer = layerUI;
        }
        blocEnHoldUI.transform.localPosition = new Vector3(0f, 0f, 0.2f);
        blocEnHoldUI.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        blocEnHoldUI.GetComponent<Rigidbody>().useGravity = false;
        objetHold = blocEnHoldUI;
        blocEnHoldUI.SetActive(true);
    }
    // Fonction qui tourne les objets sur eux-mêmes lorsqu'ils sont dans les boites de UI
    void rotationPassive(GameObject objetQuiTourne)
    {
        objetQuiTourne.transform.Rotate(0, 50 * Time.deltaTime, 0f);
    }
}
