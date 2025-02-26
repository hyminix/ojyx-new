// --- Managers/GameManager.cs ---
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.States;
using com.hyminix.game.ojyx.Enums;
using DG.Tweening;

namespace com.hyminix.game.ojyx.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Title("Configuration du Jeu")]
        [SerializeField] private int numberOfPlayers = 2;
        [SerializeField] private GameObject playerPrefab;
        // PLUS BESOIN de activePlayerBoardPosition

        [Title("Références et Managers")]
        [SerializeField, ReadOnly] private DeckController deckController;
        public DeckController DeckController => deckController;
        public List<PlayerController> players = new List<PlayerController>();
        [SerializeField] private UIManager uiManager;
        public UIManager UIManager => uiManager;
        public PlayerPositionManager PlayerPositionManager => playerPositionManager;

        [Title("État du Jeu")]
        [ReadOnly, ShowInInspector]
        private IGameState currentState;
        public IGameState CurrentState => currentState;

        [Title("Debug")]
        [ReadOnly, ShowInInspector]
        public int currentPlayerIndex = 0;
        public PlayerController CurrentPlayer => players[currentPlayerIndex];

        // Ajout : Dictionnaire pour afficher les scores (Odin)
        [ShowInInspector, ReadOnly]
        private Dictionary<int, int> playerScores = new Dictionary<int, int>();

        [Title("Transition Settings")]
        [SerializeField] private float fadeDuration = 0.5f;
        [SerializeField] private float delayBeforeFade = 1f;

        // NOUVEAU : Référence au gestionnaire de positions des joueurs
        [SerializeField] private PlayerPositionManager playerPositionManager;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            deckController = FindFirstObjectByType<DeckController>();
            if (uiManager == null)
            {
                uiManager = FindObjectOfType<UIManager>();

                if (uiManager == null)
                {
                    GameObject uiManagerGO = new GameObject("UIManager");
                    uiManager = uiManagerGO.AddComponent<UIManager>();
                    Debug.LogWarning("UIManager créé automatiquement. Il est recommandé de l'ajouter manuellement à la scène.");
                }
            }

            //Initialisation des positions joueurs
            playerPositionManager = FindObjectOfType<PlayerPositionManager>();
            if (playerPositionManager == null)
            {
                GameObject playerPositionsGO = new GameObject("PlayerPositions");
                playerPositionManager = playerPositionsGO.AddComponent<PlayerPositionManager>();
                Debug.LogWarning("PlayerPositionManager créé automatiquement. Il est recommandé de l'ajouter manuellement à la scène.");
            }
        }

        private void Start()
        {
            deckController.InitializeDeck();
            InstantiatePlayers(); // On instancie *APRES* avoir initialisé le deck
            playerPositionManager.UpdatePlayerPositions(currentPlayerIndex, numberOfPlayers);
            TransitionToState(new SetupState());

        }
        private void InstantiatePlayers()
        {
            players.Clear();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                // Instancier les joueurs sans les attacher à un parent
                GameObject playerGO = Instantiate(playerPrefab);
                PlayerController pc = playerGO.GetComponent<PlayerController>();
                pc.Initialize(i);
                players.Add(pc);
            }

            // Maintenant que tous les joueurs sont instanciés et ajoutés à la liste,
            // positionner chaque joueur à sa position initiale
            playerPositionManager.UpdatePlayerPositions(currentPlayerIndex, numberOfPlayers);

            // Mise à jour initiale des textes de l'UI
            uiManager?.SetPlayerTurnText(CurrentPlayer.playerID);
        }

        private void Update()
        {
            currentState?.ExecuteState(this);
        }

        public void TransitionToState(IGameState newState)
        {
            currentState?.ExitState(this);
            currentState = newState;

            if (currentState != null)
            {
                Debug.Log("Current State: " + currentState.GetType().Name);
                uiManager?.SetStateText(currentState.GetType().Name);
            }
            currentState?.EnterState(this);

            // *** Logique d'activation/désactivation et d'affichage du texte d'info en fonction de l'état de jeu***
            if (currentState is PlayerTurnState)
            {
                ActivateCurrentPlayer(); // Toujours besoin d'activer/désactiver pour la caméra, etc.
                uiManager?.SetInfoText(""); //On nettoie le texte d'information contextuel
            }
            else if (currentState is RevealTwoCardsState)
            {
                uiManager?.SetInfoText("Joueur " + CurrentPlayer.playerID + " : veuillez révéler deux cartes.");
            }
        }

        public void NextPlayer()
        {
            StartCoroutine(TransitionToNextPlayer());
        }

        private System.Collections.IEnumerator TransitionToNextPlayer()
        {
            yield return new WaitForSeconds(delayBeforeFade);

            uiManager.FadeOut(fadeDuration);
            yield return new WaitForSeconds(fadeDuration);

            // Changer l'index du joueur courant
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            Debug.Log($"Passage au joueur suivant: {currentPlayerIndex} (playerID: {CurrentPlayer.playerID})");

            // Mettre à jour les positions
            playerPositionManager.UpdatePlayerPositions(currentPlayerIndex, numberOfPlayers);

            // Attendre que les positions soient appliquées
            yield return new WaitForSeconds(0.1f);

            // Deuxième mise à jour pour s'assurer que tout est bien positionné
            playerPositionManager.UpdatePlayerPositions(currentPlayerIndex, numberOfPlayers);

            // Activer le joueur courant
            ActivateCurrentPlayer();

            // Attendre que tout soit stable
            yield return new WaitForFixedUpdate();


            uiManager.FadeIn(fadeDuration);
        }

        // Ajoutez cette méthode pour faciliter la calibration des offsets
        [Button("Calibrer les positions des joueurs")]
        public void CalibratePlayerPositions()
        {
            if (playerPositionManager != null)
            {
                // Cette méthode va appeler DetermineOptimalOffset() via la réflexion
                System.Type type = playerPositionManager.GetType();
                System.Reflection.MethodInfo method = type.GetMethod("DetermineOptimalOffset",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (method != null)
                {
                    method.Invoke(playerPositionManager, null);
                    Debug.Log("Calibration des offsets effectuée");
                }
                else
                {
                    Debug.LogError("Méthode DetermineOptimalOffset non trouvée");
                }
            }
        }
        public void ActivateCurrentPlayer()
        {
            //Remplacer par l'appel de PlayerPositionManager
            uiManager?.SetPlayerTurnText(CurrentPlayer.playerID);
            uiManager?.SetInfoText("Au tour de joueur " + CurrentPlayer.playerID);
        }

        public void ExecuteAction(IGameState currentState, PlayerAction action, CardSlotController cardSlot = null, CardController card = null)
        {
            currentState.ExecuteAction(this, action, cardSlot, card);
        }

        // MODIFICATION : DetermineFirstPlayer est maintenant appelé *après* la révélation des cartes (dans RevealTwoCardsState)
        public void DetermineFirstPlayer()
        {
            CalculateAllPlayerScores(); // Calcul initial des scores. On le fait ici, après que les cartes soient révélées.

            int highestScore = -999;
            int highestScorePlayerIndex = 0;

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].InitialScore > highestScore) //Utilisation de InitialScore qui est mis a jour en temps réel
                {
                    highestScore = players[i].InitialScore;
                    highestScorePlayerIndex = i;
                }
            }

            currentPlayerIndex = highestScorePlayerIndex;
            Debug.Log($"Le joueur {currentPlayerIndex} commence (score le plus élevé).");
        }


        [Button("Calculer les scores")]
        public void CalculateAllPlayerScores()
        {
            playerScores.Clear();
            foreach (var player in players)
            {
                playerScores.Add(player.playerID, player.Player.CalculateInitialScore()); // Ou CalculateScore() selon le contexte
            }
        }

        [Button("Redémarrer la Partie")]
        public void RestartGame()
        {
            // Réinitialise le deck
            deckController.InitializeDeck();

            // Réinitialise l'état des joueurs (cartes, etc.)
            foreach (var player in players)
            {
                player.Initialize(player.playerID); // Réinitialise l'ID, le modèle Player, et le plateau.
                player.DistributeInitialCards();    // Redistribue les cartes.
            }

            //On remet à jour les positions
            currentPlayerIndex = 0; // Remet le joueur courant à 0.

            // Réappliquer les positions pour tous les joueurs
            playerPositionManager.UpdatePlayerPositions(currentPlayerIndex, numberOfPlayers);

            TransitionToState(new SetupState());
            uiManager?.SetInfoText("Révélez deux cartes.");
        }


        [Button("Forcer Joueur Suivant")]
        public void ForceNextPlayer()
        {
            NextPlayer();
            uiManager?.SetInfoText("Au tour de joueur " + CurrentPlayer.playerID);
        }

        [Button("Afficher les règles")]
        public void ShowRules()
        {
            //TODO faire une vrai popup
            Debug.Log("Règles du Skyjo : ... (à compléter) ...");
        }

        // Ajoutez aussi cette méthode pour repositionner manuellement si nécessaire
        [Button("Forcer repositionnement")]
        public void ForcePlayerPositioning()
        {
            playerPositionManager.UpdatePlayerPositions(currentPlayerIndex, numberOfPlayers);
        }
    }
}