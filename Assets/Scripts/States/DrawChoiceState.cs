// --- States/DrawChoiceState.cs --- (Utilisation des méthodes du DeckController)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Enums;
using System.Collections.Generic;

namespace com.hyminix.game.ojyx.States
{
    public class DrawChoiceState : IGameState
    {
        private DeckController deckController; // Gardé pour DrawFromDeck ET DrawFromDiscardPile
        private GameManager gameManager;

        public void EnterState(GameManager manager)
        {
            Debug.Log("DrawChoiceState: Choisissez entre piocher ou prendre la défausse.");
            deckController = manager.DeckController;
            gameManager = manager;
        }

        public void ExecuteState(GameManager manager) { }
        public void ExitState(GameManager manager) { }

        public List<PlayerAction> GetValidActions(GameManager manager)
        {
            // On peut piocher depuis la pioche ou la défausse.
            return new List<PlayerAction>() { PlayerAction.DrawFromDeck, PlayerAction.DrawFromDiscard };
        }

        public void ExecuteAction(GameManager manager, PlayerAction action, CardSlotController cardSlot = null, CardController card = null)
        {
            if (!GetValidActions(manager).Contains(action))
            {
                Debug.LogError($"Action {action} non valide dans l'état DrawChoiceState !");
                return;
            }
            switch (action)
            {
                case PlayerAction.DrawFromDeck:
                    CardController drawnCardDeck = manager.DeckController.DrawFromDeck();
                    if (drawnCardDeck != null)
                    {
                        drawnCardDeck.Flip();
                        manager.TransitionToState(new CardSelectedState(drawnCardDeck, false));
                    }
                    else
                    {
                        Debug.LogError("Impossible de piocher depuis le deck !");
                    }
                    break;
                case PlayerAction.DrawFromDiscard:
                    // MODIFICATION : Utilisation de la méthode du DeckController
                    CardController drawnCardDiscard = manager.DeckController.DrawFromDiscardPile();
                    if (drawnCardDiscard != null)
                    {
                        // La carte de la défausse est déjà face visible.
                        manager.TransitionToState(new CardSelectedState(drawnCardDiscard, true));
                    }
                    else
                    {
                        Debug.LogWarning("Impossible de piocher depuis la défausse (vide).");
                    }
                    break;
            }
        }
        public PlayerAction? GetActionForCardSlotClick(GameManager manager)
        {
            return null;
        }
    }
}