// --- Managers/GameManager.cs ---
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


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            deckController = FindFirstObjectByType<DeckController>();

            PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
            foreach (PlayerController pc in playerControllers)
            {
                pc.Initialize(players.Count);
                players.Add(pc);
            }
        }

        private void Start()
        {
            deckController.InitializeDeck();
            TransitionToState(new SetupState());
        }

        private void OnEnable()
        {
            GameEvents.OnCardSlotClicked += HandleCardSlotClicked;
        }

        private void OnDisable()
        {
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

            if (currentState != null)
            {
                Debug.Log("Current State: " + currentState.GetType().Name);
            }
            currentState?.EnterState(this);
        }

        public void NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            Debug.Log("Passage au joueur suivant : " + CurrentPlayer.playerID);
        }

        private void HandleCardSlotClicked(CardSlotController slotController)
        {
            Debug.Log("GameManager.HandleCardSlotClicked: Clic détecté!");
            currentState.HandleCardClick(this, slotController);
        }

        public void DrawFromDeck()
        {
            CardController drawnCard = deckController.DrawFromDeck();
            if (drawnCard != null)
            {
                drawnCard.Flip(); // Toujours flipper une carte piochée de la pioche.
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
                // Pas besoin de Flip() ici, la carte de la défausse est déjà visible.
                TransitionToState(new CardSelectedState(drawnCard));
            }
            else
            {
                Debug.LogWarning("Impossible de piocher depuis la défausse (vide).");
            }
        }

        public void DiscardSelectedCard(CardController cardController)
        {
            if (currentState is CardSelectedState)
            {
                // Pas de Flip() ici, la carte a déjà été éventuellement flippée dans DrawChoiceState.
                DeckController.DiscardCardWithAnimation(cardController, 0.5f, DG.Tweening.Ease.OutQuad);
                TransitionToState(new RevealChosenCardState());
            }
            else
            {
                Debug.LogError("DiscardSelectedCard appelée en dehors de CardSelectedState !");
            }
        }
    }
}