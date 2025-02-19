// --- States/CardPlacementState.cs --- (COMPLET et CORRIGÉ)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Views;

namespace com.hyminix.game.ojyx.States
{
    public class CardPlacementState : IGameState, ICardDragEndHandler, ICardPlacementHandler
    {
        private CardController draggedCard;

        public CardPlacementState(CardController cardController)
        {
            draggedCard = cardController;
        }

        public void EnterState(GameManager manager)
        {
            Debug.Log("CardPlacementState: Placez la carte piochée.");
            // Désabonnement *avant* réabonnement:
            draggedCard.OnCardDragEnded -= manager.OnCardDragEnded;
            draggedCard.OnCardDragEnded += manager.OnCardDragEnded;
        }

        public void ExecuteState(GameManager manager) { }

        public void ExitState(GameManager manager)
        {
            draggedCard.OnCardDragEnded -= manager.OnCardDragEnded; //Toujours se désabonner

            //Si la carte n'a pas été placé, on la remet dans la pioche
            if (draggedCard != null)
            {
                draggedCard.SetDraggable(false);
                //Si la carte est visible, ca veut dire qu'elle viens de la pioche
                if (draggedCard.Card.IsFaceUp)
                {
                    //On la remet dans la pioche
                    manager.DeckController.deckCards.Add(draggedCard);
                    draggedCard.transform.SetParent(manager.DeckController.deckPosition);
                    draggedCard.transform.position = manager.DeckController.deckPosition.position;
                    draggedCard.Flip(); // On la remet face caché
                }
                else //Si elle viens de la défausse
                {
                    manager.DeckController.DiscardCard(draggedCard.Card); //On la défausse
                }

                draggedCard = null; //On remet la carte a null

            }
        }

        public void HandleCardDragEnd(GameManager manager, CardController cardController)
        {
            if (draggedCard == null) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // Tentative de placement sur un slot
                CardSlotController slot = hit.collider.GetComponent<CardSlotController>();
                if (slot != null)
                {
                    manager.OnCardPlaced(slot);  // Utilise la méthode centralisée
                    return; // Important:  Sortir après le placement
                }

                // Tentative de placement sur la défausse
                DiscardPileView discardPileView = hit.collider.GetComponent<DiscardPileView>();
                if (discardPileView != null)
                {
                    //On verifie que la carte ne viens pas de la defausse
                    if (!draggedCard.Card.IsFaceUp)
                    {
                        Debug.Log("CardPlacementState : Tentative de placement de la carte sur la défausse");
                        manager.TransitionToState(new RevealCardState());
                        return; //On ne fait rien si la carte viens de la défausse
                    }
                    manager.CurrentPlayer.DiscardCard(draggedCard);
                    draggedCard = null; //Important de remettre a null
                    manager.TransitionToState(new RevealCardState());
                    return;
                }
            }

            // Si on arrive ici, le drag s'est terminé sans placement valide.
            // On gère ça dans ExitState *maintenant*.
        }


        public void HandleCardPlacement(GameManager manager, CardSlotController cardSlotController)
        {
            if (draggedCard != null && cardSlotController.cardSlot.card == null)
            {
                cardSlotController.PlaceCard(draggedCard);
                draggedCard.SetDraggable(false);
                draggedCard = null;

                // On vérifie si toutes les cartes du joueur sont révélées *avant* de passer à EndTurnState
                if (manager.CurrentPlayer.PlayerBoardController.AreAllCardsRevealed())
                {
                    manager.TransitionToState(new EndRoundState()); // Fin de manche
                }
                else
                {
                    manager.TransitionToState(new EndTurnState()); // Fin de tour "normale"
                }

            }
        }

        public void HandleCardRemove(GameManager manager, CardSlotController cardSlotController)
        {
            // Pas utilisé pour l'instant
        }
    }
}
