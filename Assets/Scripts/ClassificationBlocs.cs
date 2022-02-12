using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassificationBlocs : MonoBehaviour
{
    public GameObject joueur;
    public GameObject[][] rangees;
    public int[] rangeePlaceDisponibles; // Array qui contient 20 nombres qui comptes le nombre de cube dans chaques rangées
    public GameObject[] objetJoueurs;
    public static int score;
    [SerializeField]
    private float emissionIntensity;
    // Update is called once per frame
    private void Start()
    {
        // On crée un 2D Array qui contient 20 rangées qui à l'intérieur contiennent 50 places pour accueillir des cubes
        score = 0;
        rangees = new GameObject[21][];
        for (int i = 0; i < rangees.Length; i++)
        {
            rangees[i] = new GameObject[50];
        }
    }
    void Update()
    {
        // Si Blocs n'a pas le tag contientObjetJoueur : 
        if (gameObject.CompareTag("contientObjetJoueur") == false)
        {
            // Pour chaque rangée, on classifie les blocs qui sont à l'intérieur
            for (int rangeeSelectionne = 0; rangeeSelectionne < rangees.Length; rangeeSelectionne++)
            {
                classifierBlocs(rangeeSelectionne + 0.5, rangeeSelectionne - 0.5, rangeeSelectionne, rangeeSelectionne);
            }
            // FOR LOOP SUJET À CHANGÉ, si une des rangée contient 49 blocs, elle est supprimé 
            for (int rangee = 0; rangee < rangeePlaceDisponibles.Length; rangee++)
            {
                if (rangeePlaceDisponibles[rangee] >= 49)
                {
                    supprimerRangee(rangee);
                }
            }
        } else
        { //Si Blocs a le tag contientObjetJoueur, on trouve le tetrominos et on le défait en cube séparé
            objetJoueurs = GameObject.FindGameObjectsWithTag("objetJoueur");
            foreach (GameObject objetJoueur in objetJoueurs)
            {
                Destroy(objetJoueur.GetComponent<Rigidbody>());
                int nombreDeChild = objetJoueur.transform.childCount;
                for (int i = 0; i < nombreDeChild; i++)
                {
                    var positionDetermine = objetJoueur.transform.GetChild(0).position;
                    objetJoueur.transform.GetChild(0).transform.localPosition = positionDetermine;
                    // On change de parent les cubes
                    objetJoueur.transform.GetChild(0).transform.SetParent(gameObject.transform, false);
                }
                Destroy(objetJoueur);
                gameObject.tag = "Untagged";
            }
        }
    }
    // Fonction qui classifie les blocs en rangées
    public void classifierBlocs(double limiteHaut,double limiteBas, float positionArrondie, int indexRangee)
    {
        // Pour l'entiereté des blocs inclus dans Blocs
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform blocAClassifier = transform.GetChild(i);
            // S'il ne sont pas classifiés et qu'ils ne dépassent pas les limites permises
            if (blocAClassifier.position.y > limiteBas && blocAClassifier.position.y < limiteHaut && blocAClassifier.gameObject.CompareTag("classified") == false)
            {
                // On ajuste leur posistion pour qu'elle soit exacte
                blocAClassifier.position = new Vector3(blocAClassifier.position.x, positionArrondie, blocAClassifier.position.z);
                if (blocAClassifier.position.y != 20)
                {
                    // On incrémente la place disponible dans la rangée à laquelle appartient le cube concerné
                    rangeePlaceDisponibles[indexRangee] += 1;
                    //On associe le bloc a la bonne place dans le 2D array
                    rangees[indexRangee][rangeePlaceDisponibles[indexRangee]] = blocAClassifier.gameObject;
                    // Association de couleurs selon la rangée
                    if (indexRangee == 0 || indexRangee == 6 || indexRangee == 12 || indexRangee == 18)
                    {
                        changerCouleurBloc(blocAClassifier, Color.red);
                    }
                    else if (indexRangee == 1 || indexRangee == 7 || indexRangee == 13)
                    {
                        changerCouleurBloc(blocAClassifier, Color.yellow);
                    }
                    else if (indexRangee == 2 || indexRangee == 8 || indexRangee == 14)
                    {
                        changerCouleurBloc(blocAClassifier, Color.green);
                    }
                    else if (indexRangee == 3 || indexRangee == 9 || indexRangee == 15)
                    {
                        changerCouleurBloc(blocAClassifier, Color.cyan);
                    }
                    else if (indexRangee == 4 || indexRangee == 10 || indexRangee == 16)
                    {
                        changerCouleurBloc(blocAClassifier, Color.magenta);
                    }
                    else if (indexRangee == 5 || indexRangee == 11 || indexRangee == 17)
                    {
                        changerCouleurBloc(blocAClassifier, Color.grey);
                    }
                    else if (indexRangee == 19)
                    {
                        changerCouleurBloc(blocAClassifier, Color.black);
                    }
                    blocAClassifier.gameObject.layer = 6;
                    // Le bloc est classifié
                    blocAClassifier.gameObject.tag = "classified";
                } else
                {
                    // Si le bloc classé dépasse l'index de 19, le joueur à perdu
                    SceneManager.LoadScene("GameoverScene");
                }
            }
        }
    }
    //Fonction qui permet de changer la couleur d'un bloc
    void changerCouleurBloc(Transform objetAModifier, Color couleurVoulue) {
        Material materielBloc = objetAModifier.GetComponent<Renderer>().material;
        materielBloc.SetColor("_EmissionColor", couleurVoulue * emissionIntensity);
        materielBloc.color = couleurVoulue;
    }
    // Fonction qui gère la suppression de rangée lorsqu'elle est complète - SUJET À CHANGEMENT
    public void supprimerRangee(int rangeeASuprrimer)
    {
        GetComponent<AudioSource>().Play();
        if (joueur.GetComponent<DeplacementJoueur>().vitesseDescente > 0f) joueur.GetComponent<DeplacementJoueur>().vitesseDescente -= 0.5f;
        score += (100 * (rangeeASuprrimer + 1));
        // 1 - Supprimer la rangée complétée
        for (int blocSelectionne = 0; blocSelectionne < rangees[rangeeASuprrimer].Length; blocSelectionne++)
        {
            if (rangees[rangeeASuprrimer][blocSelectionne] != null)
            {
                Destroy(rangees[rangeeASuprrimer][blocSelectionne]);
            }
        }
        // 2 - Réinitialiser le compte des blocs présents dans toutes les rangées à partir de celle supprimée
        for (int rangee = rangeeASuprrimer; rangee < rangees.Length - 1; rangee++)
        {
            rangeePlaceDisponibles[rangee] = 0;
        }
        // 3 - Descendre tous les blocs des rangées au-dessus de la rangée supprimée de une unité de hauteur
        for (int rangeeABouger = rangeeASuprrimer + 1; rangeeABouger < rangees.Length; rangeeABouger++)
        {
            for (int blocABouger = 0; blocABouger < rangees[rangeeABouger].Length; blocABouger++)
            {
                if (rangees[rangeeABouger][blocABouger] != null)
                {
                    rangees[rangeeABouger][blocABouger].transform.Translate(-Vector3.up);
                    rangees[rangeeABouger][blocABouger].tag = "Untagged";
                }
                rangees[rangeeABouger][blocABouger] = null;
            }
        }
        for (int rangee = rangeeASuprrimer; rangee < rangees.Length; rangee++)
        {
            classifierBlocs(rangee + 0.2, rangee - 0.2, rangee, rangee);
        }
    }
}
    