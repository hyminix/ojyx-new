// --- Managers/GameManager.cs --- (Gestion Centralisée des Clics)
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.States;
//SUPPRIMER using UnityEngine.EventSystems;

namespace com.hyminix.game.ojyx.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Title("Références et Managers")]
        [SerializeField, ReadOnly] private DeckController deckController;
        public DeckController DeckController => deckController;
        public List<PlayerController> players = new List<PlayerController>();

        [Title("État du Jeu")]
        [ReadOnly, ShowInInspector]
        private IGameState currentState;
        public IGameState CurrentState => currentState;

        [Title("Debug")]
        [ReadOnly, ShowInInspector]
        public int currentPlayerIndex = 0;
        public PlayerController CurrentPlayer => players[currentPlayerIndex];


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            deckController = FindFirstObjectByType<DeckController>();

            // --- Initialisation des PlayerControllers ---
            PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
            foreach (PlayerController pc in playerControllers)
            {
                // Initialise le PlayerController avec un ID unique (pour l'instant, basé sur l'ordre dans la scène)
                pc.Initialize(players.Count);
                players.Add(pc);
            }
            // --- Fin de l'initialisation des PlayerControllers ---
        }

        private void Start()
        {
            deckController.InitializeDeck();
            TransitionToState(new SetupState());
        }

        private void OnEnable()
        {
            GameEvents.OnCardSlotClicked += HandleCardSlotClicked; // MODIFIÉ
        }

        private void OnDisable()
        {
            GameEvents.OnCardSlotClicked -= HandleCardSlotClicked; // MODIFIÉ
        }



        private void Update()
        {
            currentState?.ExecuteState(this);
        }

        public void TransitionToState(IGameState newState)
        {
            currentState?.ExitState(this);
            currentState = newState;

            // --- Affichage du CurrentState dans l'inspecteur (Correction) ---
            if (currentState != null)
            {
                Debug.Log("Current State: " + currentState.GetType().Name); // Ajout pour le débogage
            }
            // --- Fin de l'affichage ---
            currentState?.EnterState(this);
        }

        public void NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            Debug.Log("Passage au joueur suivant : " + CurrentPlayer.playerID);
        }

        // MODIFICATION :  Cette méthode reçoit maintenant un CardSlotController, pas un CardController.
        private void HandleCardSlotClicked(CardSlotController slotController) // MODIFIÉ
        {
            Debug.Log("GameManager.HandleCardSlotClicked: Clic détecté!");

            //On regarde dans quel état on est et ce qu'on doit faire
            currentState.HandleCardClick(this, slotController);
        }

        public void DrawFromDeck()
        {
            CardController drawnCard = deckController.DrawFromDeck();
            if (drawnCard != null)
            {
                drawnCard.Flip();
                TransitionToState(new CardSelectedState(drawnCard));
            }
            else
            {
                Debug.LogError("Impossible de piocher depuis le deck !");
            }
        }

        public void DrawFromDiscard()
        {
            CardController drawnCard = deckController.DrawFromDiscardPile();
            if (drawnCard != null)
            {
                TransitionToState(new CardSelectedState(drawnCard));
            }
            else
            {
                Debug.LogWarning("Impossible de piocher depuis la défausse (vide).");
            }
        }

    }
}