// --- States/CardSelectedState.cs --- (Utilisation de GetValidActions et ExecuteAction)

using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Managers;

using com.hyminix.game.ojyx.Enums;

using System.Collections.Generic;



namespace com.hyminix.game.ojyx.States

{

    public class CardSelectedState : IGameState

    {

        public CardController selectedCard; // Doit être public pour DiscardPileClickHandler



        public CardSelectedState(CardController cardController)

        {

            selectedCard = cardController;

            Debug.Log($"CardSelectedState: Carte sélectionnée : {selectedCard.Card.Data.value}");

        }



        public void EnterState(GameManager manager)

        {

            Debug.Log("CardSelectedState: EnterState");



            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)

            {

                slot.SetHighlight(true); // Met en évidence les emplacements *du joueur courant*.

            }

        }



        public void ExecuteState(GameManager manager) { }



        public void ExitState(GameManager manager)

        {

            Debug.Log("CardSelectedState: ExitState");



            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)

            {

                slot.SetHighlight(false); // Désactive la mise en évidence.

            }

        }



        public List<PlayerAction> GetValidActions(GameManager manager)
        {
            // Dans CardSelectedState, on peut placer la carte ou la défausser.
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

                    //CORRECTION: Utilisation de DeckController directement.

                    manager.DeckController.DiscardCardWithAnimation(selectedCard, 0.5f, DG.Tweening.Ease.OutQuad);

                    manager.TransitionToState(new RevealChosenCardState());

                    break;

            }

        }

        public PlayerAction? GetActionForCardSlotClick(GameManager manager)
        {
            return PlayerAction.PlaceCard;
        }

    }

}
