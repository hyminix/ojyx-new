// --- Managers/GameManager.cs --- (Modifications pour ClickDetector et DrawFromDeck/Discard)
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.States;

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

        // SUPPRIMÉ:  private List<ClickDetector> addedClickDetectors = new List<ClickDetector>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            deckController = FindObjectOfType<DeckController>();
        }

        private void Start()
        {
            deckController.InitializeDeck();
            TransitionToState(new SetupState());
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


        public void DrawFromDeck()
        {
            CardController drawnCard = deckController.DrawFromDeck(); //Pioche une carte et la stock
            if (drawnCard != null)
            {
                drawnCard.SetDraggable(true);
                drawnCard.Flip(); // Montre la face *MAINTENANT*.
                TransitionToState(new CardPlacementState(drawnCard));
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
                drawnCard.SetDraggable(true); // Active le drag.
                TransitionToState(new CardPlacementState(drawnCard));
            }
            else
            {
                Debug.LogWarning("Impossible de piocher depuis la défausse (vide).");
                // On pourrait re-transitionner vers DrawChoiceState, mais attention aux boucles infinies!
                // Pour l'instant, on ne fait rien, ce qui laisse le joueur cliquer à nouveau.
            }
        }

        // --- Méthodes de gestion des événements  ---

        public void OnCardClicked(CardController cardController)
        {
            (currentState as ICardClickHandler)?.HandleCardClick(this, cardController);
        }

        public void OnCardDragStarted(CardController cardController)
        {
            (currentState as ICardDragStartHandler)?.HandleCardDragStart(this, cardController);
        }

        public void OnCardDragEnded(CardController cardController)
        {
            (currentState as ICardDragEndHandler)?.HandleCardDragEnd(this, cardController);
        }

        public void OnCardPlaced(CardSlotController cardSlotController)
        {
            (currentState as ICardPlacementHandler)?.HandleCardPlacement(this, cardSlotController);
        }

        public void OnCardRemoved(CardSlotController cardSlotController)
        {
            (currentState as ICardRemoveHandler)?.HandleCardRemove(this, cardSlotController);
        }
    }
}
