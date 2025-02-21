// --- States/CardSelectedState.cs ---
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Enums;
using System.Collections.Generic;

namespace com.hyminix.game.ojyx.States
{
    public class CardSelectedState : IGameState
    {
        public CardController selectedCard; // Carte sélectionnée
        private bool drawnFromDiscard;     // Vrai si la carte provient de la défausse

        // Ajout du paramètre drawnFromDiscard (par défaut false)
        public CardSelectedState(CardController cardController, bool drawnFromDiscard = false)
        {
            selectedCard = cardController;
            this.drawnFromDiscard = drawnFromDiscard;
            Debug.Log($"CardSelectedState: Carte sélectionnée : {selectedCard.Card.Data.value} (from {(drawnFromDiscard ? "discard" : "deck")})");
        }

        public void EnterState(GameManager manager)
        {
            Debug.Log("CardSelectedState: EnterState");
            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)
            {
                slot.SetHighlight(true);
            }
        }

        public void ExecuteState(GameManager manager) { }

        public void ExitState(GameManager manager)
        {
            Debug.Log("CardSelectedState: ExitState");
            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)
            {
                slot.SetHighlight(false);
            }
        }

        public List<PlayerAction> GetValidActions(GameManager manager)
        {
            // Dans cet état, on peut placer la carte ou (si elle provient de la défausse) annuler
            return new List<PlayerAction>() { PlayerAction.PlaceCard, PlayerAction.DiscardDrawnCard };
        }

        public void ExecuteAction(GameManager manager, PlayerAction action, CardSlotController cardSlot = null, CardController card = null)
        {
            if (!GetValidActions(manager).Contains(action))
            {
                Debug.LogError($"Action {action} non valide dans l'état CardSelectedState !");
                return;
            }
            switch (action)
            {
                case PlayerAction.PlaceCard:
                    if (cardSlot != null && cardSlot.transform.IsChildOf(manager.CurrentPlayer.PlayerBoardController.transform))
                    {
                        cardSlot.PlaceCard(selectedCard);
                        manager.TransitionToState(new EndTurnState());
                    }
                    break;
                case PlayerAction.DiscardDrawnCard:
                    if (drawnFromDiscard)
                    {
                        // Annulation : remettre la carte dans la défausse et retourner à DrawChoiceState
                        manager.DeckController.discardPileView.AddCardToDiscardPile(selectedCard);
                        manager.TransitionToState(new DrawChoiceState());
                    }
                    else
                    {
                        // Pour une carte tirée de la pioche, procéder à la défausse
                        manager.DeckController.DiscardCardWithAnimation(selectedCard, 0.5f, DG.Tweening.Ease.OutQuad);
                        manager.TransitionToState(new RevealChosenCardState());
                    }
                    break;
            }
        }

        public PlayerAction? GetActionForCardSlotClick(GameManager manager)
        {
            return PlayerAction.PlaceCard;
        }
    }
}
