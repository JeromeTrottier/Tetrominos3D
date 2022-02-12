using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementJoueur : MonoBehaviour
{
    public GameObject[] formePossibles;
    public Transform nouveauParent;
    public GameObject objetJoueur;
    private Vector3 directionDeplacementX;
    private Vector3 directionDeplacementZ;
    public GameObject prochainObjet;
    private Vector3 coordonneesProchainObjet;
    public GameObject objetStocker;
    public GameObject canvas;
    public bool positionAligne = true;
    private bool stockerEffetcue = false;
    public GameObject pivotCamera;
    public GameObject cameraMain;
    public float vitesseDescente;

    private bool objetTourneY = false;
    private bool objetTourneX = false;
    private bool objetTourneZ = false;
    public float vitesseRotation;
    private float tempsEcoule;
    private Vector3 rotationDepart;
    private float rotationFinaleAngle;
    private Vector3 rotationFinale;

    private void Start()
    {
        //Vérifie que le tetrominos controlé par le joueur n'est pas en déplacement
        positionAligne = true;
        //Génère le premier prochain objet
        prochainObjet = formePossibles[genererIndexAleatoireForme()];
        //Associe le prochain objet parent des blocs lorsqu'ils sont positionnés sur la plateforme
        nouveauParent = GameObject.Find("Blocs").transform;
    }
    // Update is called once per frame
    void Update()
    {
        var tempsEcoule = Time.deltaTime;
        if(vitesseDescente > 0) vitesseDescente -= (tempsEcoule * 0.01f);
        if (objetJoueur != null) objetJoueur.GetComponent<Rigidbody>().drag = vitesseDescente;
        //Calcul la direction des déplacements des tetrominos en fonction de la position de la caméra
        //directionDeplacementX = new Vector3(Mathf.Round(cameraMain.transform.right.x), 0f, Mathf.Round(cameraMain.transform.right.z));
        //directionDeplacementZ = new Vector3(Mathf.Round(cameraMain.transform.forward.x), 0f, Mathf.Round(cameraMain.transform.forward.z));
        if (Mathf.Round(cameraMain.transform.right.x) == 0)
        {
            directionDeplacementZ = new Vector3(Mathf.Round(cameraMain.transform.forward.x), 0f, Mathf.Round(cameraMain.transform.forward.z));
            directionDeplacementX = new Vector3(Mathf.Round(cameraMain.transform.right.x), 0f, Mathf.Round(cameraMain.transform.right.z));
        } else
        {
            directionDeplacementZ = new Vector3(0f, 0f, Mathf.Round(cameraMain.transform.forward.z));
            directionDeplacementX = new Vector3(Mathf.Round(cameraMain.transform.right.x), 0f, 0f);
        }
        //Déplacement des tretrominos

        if (Input.GetKeyDown(KeyCode.A) && positionAligne == true) //Si le joueur ne se déplace pas déjà, il peut se déplacer
        {
            positionAligne = false;
            objetJoueur.GetComponent<Rigidbody>().velocity += directionDeplacementX * -(10f + vitesseDescente);
            Invoke("reinitialiserVitesse", 0.1f);
            GetComponent<AffichageOmbreTetrominos>().inverserDifferenceBonus = false;
        }
        if (Input.GetKeyDown(KeyCode.D) && positionAligne == true)
        {
            positionAligne = false;
            objetJoueur.GetComponent<Rigidbody>().velocity += directionDeplacementX * (10f + vitesseDescente);
            Invoke("reinitialiserVitesse", 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.W) && positionAligne == true)
        {
            positionAligne = false;
            objetJoueur.GetComponent<Rigidbody>().velocity += directionDeplacementZ * (10f + vitesseDescente) ;
            Invoke("reinitialiserVitesse", 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.S) && positionAligne == true)
        {
            positionAligne = false;
            objetJoueur.GetComponent<Rigidbody>().velocity += directionDeplacementZ * -(10f + vitesseDescente);
            Invoke("reinitialiserVitesse", 0.1f);
        }
        //Gérer la rotation des tetrominos//
        if (Input.GetKeyDown(KeyCode.J) && positionAligne == true)
        {
            //La position n'est plus aligné
            positionAligne = false;
            // On met la variable correspondate à l'axe de rotation à true (Ce qui démarre une rotation en Mathf.Lerp dans la fonction rotateLerp())
            objetTourneX = true;
            //On prend la rotation de départ du tetrominos avant la rotation
            rotationDepart = objetJoueur.transform.eulerAngles;
            print(objetJoueur.transform.eulerAngles.x);
            // On calcul la rotation que l'axe visée du tetrominos va avoir après la rotation
            rotationFinaleAngle = objetJoueur.transform.eulerAngles.x + 90f;
            print(rotationFinaleAngle);
            //On met la rotation finale dans une variable de type Vector3
            rotationFinale = new Vector3(rotationFinaleAngle, objetJoueur.transform.eulerAngles.y, objetJoueur.transform.eulerAngles.z);
        }
        if (Input.GetKeyDown(KeyCode.K) && positionAligne == true)
        {
            positionAligne = false;
            objetTourneY = true;
            rotationDepart = objetJoueur.transform.eulerAngles;
            rotationFinaleAngle = objetJoueur.transform.eulerAngles.y + 90f;
            rotationFinale = new Vector3(objetJoueur.transform.eulerAngles.x, rotationFinaleAngle, objetJoueur.transform.eulerAngles.z);
        }
        if (Input.GetKeyDown(KeyCode.L) && positionAligne == true)
        {
            positionAligne = false;
            objetTourneZ = true;
            rotationDepart = objetJoueur.transform.eulerAngles;
            rotationFinaleAngle = objetJoueur.transform.eulerAngles.z + 90f;
            rotationFinale = new Vector3(objetJoueur.transform.eulerAngles.x, objetJoueur.transform.eulerAngles.y, rotationFinaleAngle);
        }
        rotateLerp(ref objetTourneX);
        rotateLerp(ref objetTourneY);
        rotateLerp(ref objetTourneZ);
        // Gérer le stockage des formes
        if (Input.GetKeyDown(KeyCode.R) && stockerEffetcue == false)
        {
            stockerForme();
            Destroy(GetComponent<AffichageOmbreTetrominos>().ombreJoueur.gameObject);
        }
        //Input qui permet d'accélérer la descente des tetrominos
        if (Input.GetKey(KeyCode.Space))
        {
            objetJoueur.GetComponent<Rigidbody>().velocity += new Vector3(0, -1, 0);
        }
        // Si le joueur n'a plus tetrominos, lui en générer un nouveau
        if (transform.childCount < 1 && objetStocker == null)
        {
            instantierProchaineForme();
        } else if (transform.childCount < 2 && objetStocker != null)
        {
            instantierProchaineForme();
        }
        
    }
    // Fonction qui gère l'animation de rotation des tetrominos
    void rotateLerp(ref bool objetAxeRotation)
    {
        //On vérifie si l'objet tourne sur l'axe de référence
        if (objetAxeRotation)
        {
            //Calcul du temps écoulé depuis le début de la rotation
            tempsEcoule += Time.deltaTime;
            //On divise le temps écoulé par la vitesse de rotation pour avoir une valeur qui part de 0 à 1 et que la rotation finisse à 1
            var pourcentageCompletion = tempsEcoule / vitesseRotation;
            //L'animation de rotation est enclenchée à l'aide d'un Quaternion.Lerp qui interpole de façon progressive la rotation de l'objet
            objetJoueur.transform.rotation = Quaternion.Lerp(Quaternion.Euler(rotationDepart), Quaternion.Euler(rotationFinale), pourcentageCompletion);
            if (tempsEcoule >= vitesseRotation)
            {
                //Lorsque la rotation est finie, on désactive le bool de référence et on désigne que la position du tetrominos est aligée
                tempsEcoule = 0f;
                objetAxeRotation = false;
                positionAligne = true;
            }
        }
    }
    // Fonction qui gère l'action de pouvoir "Hold" un tetrominos en jeu
    void stockerForme()
    {   //Si aucun tetrominos n'a été mis en "Hold" : 
        if(objetStocker == null)
        {
            //Permet de stocker une seule fois par tetrominos placé
            stockerEffetcue = true;
            objetJoueur.tag = "objetStocker";
            // On stock une copie du tetrominos en hold dans la variable ObjetStocker
            objetStocker = objetJoueur;
            // On désactive l'objetStocké
            objetStocker.SetActive(false);
            // On instantie le prochain tetrominos
            instantierProchaineForme();
            //On modifie quel tetrominos apparait dans la boite UI du tetrominos en Hold
            canvas.GetComponent<GestionUI>().changerObjetStocke();
        } else // Sinon : 
        {
            stockerEffetcue = true;
            //On instantie l'objet qui était déjà stocker
            GameObject objetRestaure = Instantiate(objetStocker, prochainObjet.transform.position + new Vector3(0f, 20f, 0f), Quaternion.identity, transform);
            if (objetRestaure.gameObject.name == "cubeJaune(Clone)(Clone)") objetRestaure.transform.position = new Vector3(0.5f, 20f, 0.5f);
            // On détruit l'objet qui était stocker dans la variable objetStocker et on y remplace l'ancien objetJoueur que l'on souhaite stocker
            Destroy(objetStocker);
            objetStocker = objetJoueur;
            objetJoueur = objetRestaure;
            objetStocker.SetActive(false);
            // On détruit l'objet qui était dans la boite  hold du UI 
            Destroy(canvas.GetComponent<GestionUI>().objetStockerUIEspace.transform.GetChild(0).gameObject);
            // On affiche le nouvel objet stocker dans le UI
            canvas.GetComponent<GestionUI>().changerObjetStocke();
            objetRestaure.SetActive(true);
            GetComponent<AffichageOmbreTetrominos>().remplirListDeBlocsJoueur = false;
            GetComponent<AffichageOmbreTetrominos>().creerOmbre = false;
        }
    }
    // Fonction qui gère la génération du prochain tetrominos
    void instantierProchaineForme()
    {
        var formeAleatoire = prochainObjet;
        //Instantiation de l'objet stocker dans la variable prochainObjet
        GameObject forme = Instantiate(formeAleatoire, new Vector3(0f, 25f, 0f), Quaternion.identity, transform);
        //Un if qui modifie le positionnement de certains tetrominos à cause de leur point de pivot unique (permet de les centrer sur la grille)
        if (forme.gameObject.name == "cubeJaune(Clone)") forme.transform.position = new Vector3(0.5f, 25f, 0.5f);
        // On active la forme instantiée
        forme.SetActive(true);
        // L'objet que le joueur controle devient l'objet instantiée
        objetJoueur = forme;
        GetComponent<AffichageOmbreTetrominos>().remplirListDeBlocsJoueur = false;
        GetComponent<AffichageOmbreTetrominos>().creerOmbre = false;
        // Permet au joueur de réutiliser la fonctionnalité de Hold
        stockerEffetcue = false;
        //Tout de suite générer quel sera le prochain objet pour pouvoir le montrer au joueur
        //Générer un index aleatoire
        int indexAleatoire = genererIndexAleatoireForme();
        prochainObjet = formePossibles[indexAleatoire];
        coordonneesProchainObjet = formePossibles[indexAleatoire].transform.position;
        canvas.GetComponent<GestionUI>().changerProchainObjetUI();
    }
    int genererIndexAleatoireForme()
    {
        //Trouve un index aléatoire dans les possibilités des tetrominos offert au joueur
        int nombreAleatoire = Random.Range(0, formePossibles.Length);
        return nombreAleatoire;
    }
    // Fonction qui réinitalise la vélocité des tetrominos lors de leur déplacement pour empêcher qu'ils puissent se déplacer de plus que 1 cube de loin à la fois
    void reinitialiserVitesse()
    {
        float posXArrondie = Mathf.Round(objetJoueur.transform.position.x * 2) / 2;
        float posZArrondie = Mathf.Round(objetJoueur.transform.position.z * 2) / 2;
        objetJoueur.GetComponent<Rigidbody>().velocity = new Vector3(0, objetJoueur.GetComponent<Rigidbody>().velocity.y, 0);
        objetJoueur.transform.position = new Vector3(posXArrondie, objetJoueur.transform.position.y, posZArrondie);
        positionAligne = true;
    }
}
