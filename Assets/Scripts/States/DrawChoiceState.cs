// --- States/DrawChoiceState.cs --- (CORRIGÉ et SIMPLIFIÉ)

using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.States
{
    public class DrawChoiceState : IGameState
    {
        private DeckController deckController;
        private GameManager gameManager;

        public void EnterState(GameManager manager)
        {
            Debug.Log("DrawChoiceState: Choisissez entre piocher ou prendre la défausse.");
            deckController = manager.DeckController;
            this.gameManager = manager;

            SubscribeToEvents();
        }

        public void ExecuteState(GameManager manager) { }

        public void ExitState(GameManager manager)
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            // PIOCHE
            if (deckController.deckCards.Count > 0)
            {
                CardController topDeckCard = deckController.deckCards[0];
                topDeckCard.OnCardClicked += HandleDeckClick; // Utilisez l'événement de CardController
            }

            // DEFAUSSE
            if (deckController.DiscardPile.cards.Count > 0)
            {
                CardController topDiscardCard = deckController.discardPileView.discardSlot.GetComponentInChildren<CardController>();
                if (topDiscardCard != null)
                {
                    topDiscardCard.OnCardClicked += HandleDiscardClick; // Utilisez l'événement de CardController
                }
            }
        }

        private void UnsubscribeFromEvents()
        {
            // PIOCHE
            if (deckController.deckCards.Count > 0)
            {
                CardController topDeckCard = deckController.deckCards[0];
                topDeckCard.OnCardClicked -= HandleDeckClick;
            }

            // DEFAUSSE
            if (deckController.DiscardPile.cards.Count > 0)
            {
                CardController topDiscardCard = deckController.discardPileView.discardSlot.GetComponentInChildren<CardController>();
                if (topDiscardCard != null)
                {
                    topDiscardCard.OnCardClicked -= HandleDiscardClick;
                }
            }
        }

        private void HandleDeckClick(CardController cardController) // Ajout du paramètre
        {
            gameManager.DrawFromDeck();
        }

        private void HandleDiscardClick(CardController cardController) // Ajout du paramètre
        {
            gameManager.DrawFromDiscard();
        }
    }
}
