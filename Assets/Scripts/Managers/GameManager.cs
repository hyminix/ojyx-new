// --- Managers/GameManager.cs --- (Gestion Centralisée des Clics)
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.States;
using UnityEngine.EventSystems;

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

            // FindFirstObjectByType au lieu de FindObjectOfType
            deckController = FindFirstObjectByType<DeckController>();
        }

        private void Start()
        {
            deckController.InitializeDeck();
            TransitionToState(new SetupState());
        }

        // On utilise OnEnable et OnDisable pour gérer les abonnements aux événements.
        // C'est plus propre et plus sûr que Start/ExitState.
        private void OnEnable()
        {
            // Utilisation directe de GameEvents
            GameEvents.OnCardSlotClicked += HandleCardSlotClicked;
        }

        private void OnDisable()
        {
            // Utilisation directe de GameEvents
            GameEvents.OnCardSlotClicked -= HandleCardSlotClicked;
        }



        private void Update()
        {
            currentState?.ExecuteState(this);
        }

        public void TransitionToState(IGameState newState)
        {
            currentState?.ExitState(this);
            currentState = newState;
            currentState.EnterState(this);
        }

        public void NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            Debug.Log("Passage au joueur suivant : " + CurrentPlayer.playerID);
        }

        // MODIFICATION :  Cette méthode reçoit maintenant un CardSlotController, pas un CardController.
        private void HandleCardSlotClicked(CardSlotController slotController, PointerEventData eventData) // MODIFIÉ
        {
            Debug.Log("GameManager.HandleCardSlotClicked: Clic détecté!");
            CardController cardController = slotController.GetComponentInChildren<CardController>();
            currentState.HandleCardClick(this, cardController, eventData); // MODIFIÉ : Passe eventData
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

        // Dans GameManager.cs
        public void SubscribeToCardSlotEvents()
        {
            foreach (var player in players)
            {
                foreach (var slot in player.PlayerBoardController.playerBoardView.cardSlots)
                {
                    // GameManager s'abonne à l'événement de CHAQUE CardSlot.
                    GameEvents.OnCardSlotClicked += HandleCardSlotClicked;
                }
            }
        }
    }
}