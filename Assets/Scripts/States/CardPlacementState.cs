// --- States/CardPlacementState.cs ---

using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Managers;

using com.hyminix.game.ojyx.Views;



namespace com.hyminix.game.ojyx.States

{

    public class CardPlacementState : IGameState, ICardDragEndHandler, ICardPlacementHandler

    {

        private CardController draggedCard;

        private CardSlotController originalSlot;

        public CardPlacementState(CardController cardController)

        {

            draggedCard = cardController;

        }

        public void EnterState(GameManager manager)

        {

            Debug.Log("CardPlacementState: Placez la carte piochée.");

            //On s'abonne a l'evenement de drag de la carte pioché

            draggedCard.OnCardDragEnded += manager.OnCardDragEnded; //On s'abonne au drag de la carte

        }



        public void ExecuteState(GameManager manager) { }



        public void ExitState(GameManager manager)

        {

            //On se désabonne a l'evenement de drag de la carte pioché

            draggedCard.OnCardDragEnded -= manager.OnCardDragEnded; //On se désabonne au drag de la carte

        }



        public void HandleCardDragEnd(GameManager manager, CardController cardController)

        {

            if (draggedCard == null) return; //Securité



            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Raycast

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))

            {

                //Si on touche un slot

                CardSlotController slot = hit.collider.GetComponent<CardSlotController>();

                if (slot != null)

                {

                    // Tente de placer la carte

                    manager.OnCardPlaced(slot); //On appelle la fonction de placement de carte du manager

                    return; //On quitte la fonction

                }

                //Si on touche la défausse

                if (hit.collider.GetComponent<DiscardPileView>() != null)

                {

                    //Si on a pioché dans la pioche

                    if (manager.CurrentState is DrawFromDeckState)

                    {

                        // Défausser (géré par une méthode séparée, si besoin)

                        manager.CurrentPlayer.DiscardCard(cardController);

                        draggedCard = null; //On remet la carte a null

                        originalSlot = null; //On remet le slot a null

                        manager.TransitionToState(new RevealCardState()); //On passe a l'état de révélation de carte

                        return;

                    }

                    else

                    { //Si on a tiré une carte de la défausse mais qu'on la replace sur la défausse, on ne fait rien

                        //On replace la carte sur la défausse

                        manager.DeckController.DiscardCard(draggedCard.Card);

                        draggedCard = null;

                        return;

                    }

                }

            }



            // Si le placement/défausse échoue, remet la carte à sa place.

            //Ici on ne fait rien, car la carte doit etre placé

            // if (originalSlot != null)

            // {

            //     originalSlot.PlaceCard(draggedCard);

            // }

            // draggedCard = null;

            // originalSlot = null;

        }



        //Fonction de placement de carte

        public void HandleCardPlacement(GameManager manager, CardSlotController cardSlotController)

        {

            if (draggedCard != null)

            {

                //Si le slot est vide

                if (cardSlotController.cardSlot.card == null)

                {

                    cardSlotController.PlaceCard(draggedCard); //Place la carte

                    draggedCard = null; //On remet la carte en cours de drag a null

                    manager.TransitionToState(new EndTurnState()); //On passe a la fin de tour

                }



            }

        }

        public void HandleCardRemove(GameManager manager, CardSlotController cardSlotController)

        {

            //Implémenter si besoin

        }



    }

}
