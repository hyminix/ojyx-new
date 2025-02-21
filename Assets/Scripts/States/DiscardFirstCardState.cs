// --- States/DiscardFirstCardState.cs --- (Implémentation de GetValidActions et ExecuteAction)
using UnityEngine;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Enums;
using System.Collections.Generic;
namespace com.hyminix.game.ojyx.States
{
    public class DiscardFirstCardState : IGameState
    {
        public void EnterState(GameManager manager)
        {
            Debug.Log("DiscardFirstCardState: Déplacement de la première carte de la pioche à la défausse.");
            //CORRECTION: La logique est déplacée dans ExecuteAction
            manager.ExecuteAction(this, PlayerAction.DiscardDrawnCard); //On utilise l'enum
        }
        public void ExecuteState(GameManager manager) { }
        public void ExitState(GameManager manager) { }
        // Nouvelle méthode GetValidActions
        public List<PlayerAction> GetValidActions(GameManager manager)
        {
            return new List<PlayerAction>() { PlayerAction.DiscardDrawnCard }; // La seule action valide est implicite
        }
        // Nouvelle méthode ExecuteAction
        public void ExecuteAction(GameManager manager, PlayerAction action, CardSlotController cardSlot = null, CardController card = null)
        {
            if (action != PlayerAction.DiscardDrawnCard)
            {
                Debug.LogError("Action non valide dans DiscardFirstCardState");
                return;
            }
            CardController cardToDiscard = manager.DeckController.DrawFromDeck();
            if (cardToDiscard != null)
            {
                cardToDiscard.Flip(); // Assure que la carte est face visible.
                manager.DeckController.DiscardCardWithAnimation(cardToDiscard, 0.5f, DG.Tweening.Ease.OutQuad);
                manager.TransitionToState(new RevealTwoCardsState());
            }
            else
            {
                Debug.LogError("DiscardFirstCardState: Impossible de piocher une carte.");
                // Que faire si la pioche est vide ? Il faudrait gérer ce cas (fin de partie ?).
                manager.TransitionToState(new RevealTwoCardsState()); //Pour l'instant on continue
            }
        }
        public PlayerAction? GetActionForCardSlotClick(GameManager manager)
        {
            throw new System.NotImplementedException();
        }
    }
}
