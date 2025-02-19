// --- States/DrawFromDiscardState.cs --- (CORRIGÉ)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.States
{
    public class DrawFromDiscardState : IGameState
    {
        private CardController drawnCard; //Carte pioché

        public void EnterState(GameManager manager)
        {
            Debug.Log("DrawFromDiscardState: Pioche une carte de la défausse...");
            drawnCard = manager.DeckController.DrawFromDiscardPile();//Pioche la carte de la défausse et la stock
            if (drawnCard != null)
            {
                // Activer le drag & drop
                drawnCard.SetDraggable(true);
                manager.TransitionToState(new CardPlacementState(drawnCard));
            }
            else
            {
                // La défausse était vide.  Retourner à l'état de choix.
                Debug.LogWarning("DrawFromDiscardState: La défausse est vide.");
                manager.TransitionToState(new DrawChoiceState());
            }

        }

        public void ExecuteState(GameManager manager) { }
        public void ExitState(GameManager manager) { }
    }
}
