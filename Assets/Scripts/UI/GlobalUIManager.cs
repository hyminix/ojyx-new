using System.Collections.Generic;
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers; // Pour accéder à PlayerController, GameManager

public class GlobalUIManager : MonoBehaviour
{
    [Header("Prefab & Container")]
    [SerializeField] private GameObject playerViewPrefab;
    [SerializeField] private RectTransform playerViewContainer;

    private List<GameObject> currentViews = new List<GameObject>();

    /// <summary>
    /// Petit struct pour stocker l'ancre et l'offset
    /// anchor = où l'objet est ancré (0=left,1=right sur X ; 0=bottom,1=top sur Y)
    /// offset = décalage en pixels par rapport à l'ancre
    /// </summary>
    [System.Serializable]
    public struct SeatData
    {
        public Vector2 anchor;   // ex. (0.5, 0) => bas-centre
        public Vector2 offset;   // ex. (0, 100) => 100 px au-dessus du bord bas
    }

    /// <summary>
    /// Dictionnaire : pour chaque nombre de joueurs (2 à 8),
    /// on définit un tableau de SeatData.
    /// - seatData[0] correspond à la place du joueur courant (index 0 = bas-centre).
    /// - seatData[1], seatData[2], etc. = les autres joueurs dans l'ordre.
    /// 
    /// Ajustez anchor & offset si vous voulez un placement précis (plus ou moins collé au bord).
    /// </summary>
    private Dictionary<int, SeatData[]> seatDataByPlayerCount = new Dictionary<int, SeatData[]>
    {
        {
            2, new SeatData[]
            {
                // Joueur courant (bas-centre)
                new SeatData { anchor = new Vector2(0.5f, 0f), offset = new Vector2(0f, 100f) },
                // L'autre joueur (haut-centre)
                new SeatData { anchor = new Vector2(0.5f, 1f), offset = new Vector2(0f, -100f) },
            }
        },
        {
            3, new SeatData[]
            {
                // Bas-centre
                new SeatData { anchor = new Vector2(0.5f, 0f), offset = new Vector2(0f, 100f) },
                // Haut-gauche
                new SeatData { anchor = new Vector2(0f, 1f), offset = new Vector2(100f, -100f) },
                // Haut-droite
                new SeatData { anchor = new Vector2(1f, 1f), offset = new Vector2(-100f, -100f) },
            }
        },
        {
            4, new SeatData[]
            {
                // Bas-centre
                new SeatData { anchor = new Vector2(0.5f, 0f), offset = new Vector2(0f, 100f) },
                // Droite-centre
                new SeatData { anchor = new Vector2(1f, 0.5f), offset = new Vector2(-100f, 0f) },
                // Haut-centre
                new SeatData { anchor = new Vector2(0.5f, 1f), offset = new Vector2(0f, -100f) },
                // Gauche-centre
                new SeatData { anchor = new Vector2(0f, 0.5f), offset = new Vector2(100f, 0f) },
            }
        },
        {
            5, new SeatData[]
            {
                // Bas-centre
                new SeatData { anchor = new Vector2(0.5f, 0f), offset = new Vector2(0f, 100f) },
                // Bas-droite
                new SeatData { anchor = new Vector2(1f, 0.2f), offset = new Vector2(-100f, 0f) },
                // Haut-droite
                new SeatData { anchor = new Vector2(1f, 0.8f), offset = new Vector2(-100f, 0f) },
                // Haut-gauche
                new SeatData { anchor = new Vector2(0f, 0.8f), offset = new Vector2(100f, 0f) },
                // Bas-gauche
                new SeatData { anchor = new Vector2(0f, 0.2f), offset = new Vector2(100f, 0f) },
            }
        },
        {
            6, new SeatData[]
            {
                // Bas-centre
                new SeatData { anchor = new Vector2(0.5f, 0f), offset = new Vector2(0f, 100f) },
                // Bas-droite
                new SeatData { anchor = new Vector2(1f, 0.2f), offset = new Vector2(-100f, 0f) },
                // Droite-haut
                new SeatData { anchor = new Vector2(1f, 0.8f), offset = new Vector2(-100f, 0f) },
                // Haut-centre
                new SeatData { anchor = new Vector2(0.5f, 1f), offset = new Vector2(0f, -100f) },
                // Gauche-haut
                new SeatData { anchor = new Vector2(0f, 0.8f), offset = new Vector2(100f, 0f) },
                // Bas-gauche
                new SeatData { anchor = new Vector2(0f, 0.2f), offset = new Vector2(100f, 0f) },
            }
        },
        {
            7, new SeatData[]
            {
                // Bas-centre (joueur courant)
                new SeatData { anchor = new Vector2(0.5f, 0f), offset = new Vector2(0f, 100f) },
                // Bas-droite (1)
                new SeatData { anchor = new Vector2(1f, 0.15f), offset = new Vector2(-100f, 0f) },
                // Milieu-droite (2)
                new SeatData { anchor = new Vector2(1f, 0.5f), offset = new Vector2(-100f, 0f) },
                // Haut-droite (3)
                new SeatData { anchor = new Vector2(1f, 0.85f), offset = new Vector2(-100f, 0f) },
                // Haut-gauche (4)
                new SeatData { anchor = new Vector2(0f, 0.85f), offset = new Vector2(100f, 0f) },
                // Milieu-gauche (5)
                new SeatData { anchor = new Vector2(0f, 0.5f), offset = new Vector2(100f, 0f) },
                // Bas-gauche (6)
                new SeatData { anchor = new Vector2(0f, 0.15f), offset = new Vector2(100f, 0f) },
            }
        },
        {
            8, new SeatData[]
            {
                // Bas-centre (joueur courant)
                new SeatData { anchor = new Vector2(0.5f, 0f), offset = new Vector2(0f, 100f) },
                // Bas-droite (1)
                new SeatData { anchor = new Vector2(1f, 0.125f), offset = new Vector2(-100f, 0f) },
                // Milieu-droite bas (2)
                new SeatData { anchor = new Vector2(1f, 0.375f), offset = new Vector2(-100f, 0f) },
                // Milieu-droite haut (3)
                new SeatData { anchor = new Vector2(1f, 0.625f), offset = new Vector2(-100f, 0f) },
                // Haut-droite (4)
                new SeatData { anchor = new Vector2(1f, 0.875f), offset = new Vector2(-100f, 0f) },
                // Haut-centre (5)
                new SeatData { anchor = new Vector2(0.5f, 1f), offset = new Vector2(0f, -100f) },
                // Milieu-gauche haut (6)
                new SeatData { anchor = new Vector2(0f, 0.625f), offset = new Vector2(100f, 0f) },
                // Milieu-gauche bas (7)
                new SeatData { anchor = new Vector2(0f, 0.375f), offset = new Vector2(100f, 0f) },
            }
        },
    };

    /// <summary>
    /// Méthode à appeler pour afficher la vue globale.
    /// Elle va instancier un PlayerView par joueur et le positionner
    /// selon le dictionnaire seatDataByPlayerCount.
    /// Le joueur courant (currentPlayerIndex) sera toujours seat[0] (bas-centre).
    /// </summary>
    public void UpdateGlobalView()
    {
        // Nettoyer l’existant
        foreach (var go in currentViews)
        {
            if (go != null) Destroy(go);
        }
        currentViews.Clear();

        int nbPlayers = GameManager.Instance.players.Count;

        if (!seatDataByPlayerCount.ContainsKey(nbPlayers))
        {
            Debug.LogWarning("Aucune configuration pour " + nbPlayers + " joueurs. " +
                             "Adaptez seatDataByPlayerCount pour couvrir tous les cas.");
            return;
        }

        SeatData[] seats = seatDataByPlayerCount[nbPlayers];

        // Pour chaque joueur...
        for (int i = 0; i < nbPlayers; i++)
        {
            // Calcul de l’index du joueur : le joueur courant + i, modulo nbPlayers
            int playerIndex = (GameManager.Instance.currentPlayerIndex + i) % nbPlayers;

            // Instancier le prefab
            GameObject viewGO = Instantiate(playerViewPrefab, playerViewContainer);

            // Récupérer son RectTransform
            RectTransform rt = viewGO.GetComponent<RectTransform>();
            if (rt != null)
            {
                // On fixe anchorMin et anchorMax = l’ancre voulue
                rt.anchorMin = seats[i].anchor;
                rt.anchorMax = seats[i].anchor;

                // Pivot au centre
                rt.pivot = new Vector2(0.5f, 0.5f);

                // On applique l’offset
                rt.anchoredPosition = seats[i].offset;
            }

            // Initialiser la vue
            PlayerView pv = viewGO.GetComponent<PlayerView>();
            if (pv != null)
            {
                pv.Initialize(GameManager.Instance.players[playerIndex]);
            }

            currentViews.Add(viewGO);
        }
    }
}
