using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffichageOmbreTetrominos : MonoBehaviour
{
    public GameObject objetJoueurOrigine;
    public List<GameObject> blocsJoueur;
    public bool remplirListDeBlocsJoueur = false;
    public LayerMask layerMask;
    public List<float> distancesBlocsSol;
    public List<float> distancesBlocsColliders;
    public List<float> distancesBlocsOmbresColliders;
    public float distanceMinimumBlocOmbreCollider;
    public bool inverserDifferenceBonus;
    public List<float> positionsYBlocs;


    public float distanceLaPlusCourte;
    public int indexBlocReference;
    public float differenceDistance;
    public float differenceBonus;

    public GameObject ombreJoueurParent;
    public GameObject ombreJoueur;
    public bool creerOmbre;

    // Update is called once per frame
    void Update()
    {
        //Cherche la forme d'origine control� par le joueur
        objetJoueurOrigine = GetComponent<DeplacementJoueur>().objetJoueur.gameObject;
        //Si la liste des blocs composant la forme originelle n'est pas compl�t� : la compl�ter
        if (!remplirListDeBlocsJoueur && objetJoueurOrigine != null) collecterBlocsEnfants();
        //Lorsque la liste est compl�t�e, trouver la hauteur � laquelle on doit afficher l'ombre
        if (remplirListDeBlocsJoueur) trouverDifferenceDeDistance();
        //Lorsque la hauteur est trouv�e, afficher l'ombre
        if (distanceLaPlusCourte != 0.0f) afficherOmbre();
    }
    void collecterBlocsEnfants()
    {
        // � chaque update, on resaisie l'information contenu dans chaque array, donc il est obligatoire de les Clear()
        blocsJoueur.Clear();
        distancesBlocsColliders.Clear();
        distancesBlocsSol.Clear();
        positionsYBlocs.Clear();
        distancesBlocsOmbresColliders.Clear();
        // Pour chaque bloc, les contenir dans un array, ainsi que qu'initialiser 4 diff�rents array pour plus tard
        for (int i = 0; i < objetJoueurOrigine.transform.childCount; i++)
        {
            GameObject unBlocDuTetrominos = objetJoueurOrigine.transform.GetChild(i).gameObject;
            blocsJoueur.Add(unBlocDuTetrominos);
            // Initilisation de la liste qui va permettre de savoir � quelle distance chaque bloc est du sol
            distancesBlocsSol.Add(0.0f);
            // Initilisation de la liste qui va permettre de savoir � quelle distance chaque bloc est de n'importe quel collider
            distancesBlocsColliders.Add(0.0f);
            // Initilisation de la liste qui va permettre de savoir quelle est la position en Y de chaque bloc
            positionsYBlocs.Add(0.0f);
            // Initialisation de la liste que va permettre de savoir quelle est la distance entre chaque bloc de l'ombre de n'importe quel collider
            distancesBlocsOmbresColliders.Add(0.0f);
        }
        // Affirmer que la liste des blocs est compl�t�e
        remplirListDeBlocsJoueur = true;
    }
    void trouverDifferenceDeDistance()
    {
        // Pour chaque blocs du tetrominos control� par le joueur, on lance deux RayCast vers le bas
        for (int i = 0; i < blocsJoueur.Count; i++)
        {
            // Ce Raycast trouve la distance la plus courte possible entre la forme et le prochain collider
            RaycastHit infoCollisionsDistanceLaPlusCourte;
            if (Physics.Raycast(blocsJoueur[i].transform.position, -Vector3.up, out infoCollisionsDistanceLaPlusCourte, 50f))
            {
                distancesBlocsColliders[i] = infoCollisionsDistanceLaPlusCourte.distance;
            }
            // Ce Raycast trouve la distance la plus courte possible entre la forme et le sol (la base noire)
            RaycastHit infoCollisionsDitanceAuSol;
            if (Physics.Raycast(blocsJoueur[i].transform.position, -Vector3.up, out infoCollisionsDitanceAuSol, 50f, layerMask))
            {
                distancesBlocsSol[i] = infoCollisionsDitanceAuSol.distance;
            }
        }
        // Ensuite, on calcule la distance la plus courte de la liste des distances les plus courtes entre les colliders et les blocs
        distanceLaPlusCourte = Mathf.Min(distancesBlocsColliders.ToArray());
        // Boucle qui va chercher l'index de bloc qui ve �tre le bloc de r�f�rence pour donn� la position de l'ombre
        for (int i = 0; i < blocsJoueur.Count; i++)
        {
            if (distancesBlocsColliders[i] == distanceLaPlusCourte) indexBlocReference = distancesBlocsColliders.IndexOf(distancesBlocsColliders[i]);
        }
        // Exception de distance si la forme d'origine est leL ou le troisMauve
        if (objetJoueurOrigine.gameObject.name == "leL(Clone)" || objetJoueurOrigine.gameObject.name == "troisMauve(Clone)")
        {
            // Si inverserDifferenceBonus est � False (d�finit dans afficherOmbre())
            if (!inverserDifferenceBonus)
            {
                //On ajoute une distance bonus pour ajuster la hauteur de l'ombre par rapport � son point de pivot
                differenceDistance = distancesBlocsSol[indexBlocReference] - distanceLaPlusCourte + differenceBonus;
            }
            else
            {
                //On affiche la forme normalement
                differenceDistance = distancesBlocsSol[indexBlocReference] - distanceLaPlusCourte;
            }
        }
        else
        {
            //On ajoute une distance bonus pour ajuster la hauteur de l'ombre par rapport � son point de pivot
            differenceDistance = distancesBlocsSol[indexBlocReference] - distanceLaPlusCourte + differenceBonus;
        }
    }
    void afficherOmbre()
    {
        // Raycast qui pointe vers le bas qui va d�terminer la position Y de l'ombre
        RaycastHit infoCollisionsObjetJoueur;
        Physics.Raycast(objetJoueurOrigine.transform.position, -Vector3.up, out infoCollisionsObjetJoueur, 50f, layerMask);
        if (!creerOmbre)
        {
            // Si l'ombre n'est pas cr��e : la cr�er
            ombreJoueur = Instantiate(objetJoueurOrigine, new Vector3(objetJoueurOrigine.transform.position.x, infoCollisionsObjetJoueur.point.y + 0.5f, objetJoueurOrigine.transform.position.z), Quaternion.identity, ombreJoueurParent.transform);
            creerOmbre = true;
        }
        // L'ombre n'utilise pas la gravit� pour �viter qu'elle tombe
        ombreJoueur.GetComponent<Rigidbody>().useGravity = false;
        // POur chaque bloc de l'ombre : 
        for (int i = 0; i < blocsJoueur.Count; i++)
        {
            // Si le bloc contient quelquechose, le detruire
            if (ombreJoueur.transform.GetChild(i).childCount > 0) Destroy(ombreJoueur.transform.GetChild(i).transform.GetChild(0).gameObject);
            // R�duire de 0.1 le scale de chaque scale
            ombreJoueur.transform.GetChild(i).transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            // Rendre le boxCollider � Trigger
            ombreJoueur.transform.GetChild(i).GetComponent<BoxCollider>().isTrigger = true;
            // D�sactiver le ParentDestroyer pour ne pas que l'ombre agisse comme la forme originale
            ombreJoueur.GetComponent<ParentDestroyer>().enabled = false;
            // changer la couleur du bloc � gris et l'emission � black
            ombreJoueur.transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            ombreJoueur.transform.GetChild(i).GetComponent<Renderer>().material.color = Color.grey;
        }
        // Pour chaque bloc, on trouve l'information de leur position Y et on la stocke dans une liste
        for (int i = 0; i < blocsJoueur.Count; i++)
        {
            positionsYBlocs[i] = objetJoueurOrigine.transform.GetChild(i).position.y;
        }
        // On trouve la posistion Y la plus petite de l'ombre
        float positionYblocMin = Mathf.Min(positionsYBlocs.ToArray());
        // La diff�renceBonus est �gale � la hauteur du point de pivot + la position Y la plus petite de l'ombre
        differenceBonus = objetJoueurOrigine.transform.position.y - positionYblocMin;
        // On change la position et la rotation de l'ombre en fonction de la forme originale sauf pour sa position en Y
        ombreJoueur.transform.position = new Vector3(objetJoueurOrigine.transform.position.x, differenceDistance, objetJoueurOrigine.transform.position.z);
        ombreJoueur.transform.eulerAngles = new Vector3(objetJoueurOrigine.transform.eulerAngles.x, objetJoueurOrigine.transform.eulerAngles.y, objetJoueurOrigine.transform.eulerAngles.z);
        //On lance un raycast de chaque bloc de l'ombre pour savoir si l'ombre ne rentre pas dans le sol
        for (int i = 0; i < blocsJoueur.Count; i++)
        {
            RaycastHit infoCollisionDistColliderPlusProche;
            if (Physics.Raycast(ombreJoueur.transform.GetChild(i).transform.position, -Vector3.up, out infoCollisionDistColliderPlusProche, 50f))
            {
                distancesBlocsOmbresColliders[i] = infoCollisionDistColliderPlusProche.distance;
                Debug.DrawRay(ombreJoueur.transform.GetChild(i).transform.position, -Vector3.up * 20, Color.red);
            }
        }
        distanceMinimumBlocOmbreCollider = Mathf.Min(distancesBlocsOmbresColliders.ToArray());
        // Si l'ombre ne rentre pas dans le sol, on n'utilise pas la diff�renceBonus
        if (distanceMinimumBlocOmbreCollider > 1f)
        {
            inverserDifferenceBonus = true;
        } else if (GetComponent<DeplacementJoueur>().positionAligne == false) // Si l'ombre rentre dans le sol, on utilise la diff�rencebonus
        {
            inverserDifferenceBonus = false;
        }
    }
}