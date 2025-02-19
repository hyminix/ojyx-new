// --- States/DrawFromDeckState.cs --- (CORRIGÉ)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.States
{
    public class DrawFromDeckState : IGameState
    {
        private CardController drawnCard;

        public void EnterState(GameManager manager)
        {
            Debug.Log("DrawFromDeckState: Pioche une carte...");
            drawnCard = manager.DeckController.DrawFromDeck();
            if (drawnCard != null)
            {
                drawnCard.SetDraggable(true);
                // Affiche la carte suivante *avant* de passer à CardPlacementState.
                manager.DeckController.ShowNextCard();
                manager.TransitionToState(new CardPlacementState(drawnCard));
            }
            else
            {
                Debug.LogError("DrawFromDeckState: Impossible de piocher!");
                manager.TransitionToState(new PlayerTurnState()); // Ou un autre état approprié.
            }
        }


        public void ExecuteState(GameManager manager) { }

        public void ExitState(GameManager manager)
        {
            // Masque la carte suivante en quittant l'état.
            manager.DeckController.HideNextCard();
        }
    }
}
