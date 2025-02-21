// --- States/DiscardFirstCardState.cs ---
using UnityEngine;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Controllers;

namespace com.hyminix.game.ojyx.States
{
    public class DiscardFirstCardState : IGameState
    {
        public void EnterState(GameManager manager)
        {
            Debug.Log("DiscardFirstCardState: Déplacement de la première carte de la pioche à la défausse.");

            CardController cardToDiscard = manager.DeckController.DrawFromDeck();
            if (cardToDiscard != null)
            {
                cardToDiscard.Flip(); // Assure que la carte est face visible.
                manager.DeckController.DiscardCardWithAnimation(cardToDiscard, 0.5f, DG.Tweening.Ease.OutQuad);
            }
            else
            {
                Debug.LogError("DiscardFirstCardState: Impossible de piocher une carte.");
            }

            manager.TransitionToState(new RevealTwoCardsState());
        }

        public void ExecuteState(GameManager manager) { }
        public void ExitState(GameManager manager) { }
        public void HandleCardClick(GameManager manager, CardSlotController slotController)
        {
            // Ne rien faire ici.
        }
    }
}