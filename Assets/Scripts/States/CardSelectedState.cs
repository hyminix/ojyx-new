// --- States/CardSelectedState.cs --- (SIMPLIFIÉ : Plus d'événements)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
using UnityEngine.EventSystems;

namespace com.hyminix.game.ojyx.States
{
    public class CardSelectedState : IGameState
    {
        private CardController selectedCard;

        public CardSelectedState(CardController cardController)
        {
            selectedCard = cardController;
            Debug.Log($"CardSelectedState: Carte selectionné : {selectedCard.Card.Data.value}");
        }

        public void EnterState(GameManager manager)
        {
            Debug.Log("CardSelectedState: EnterState");
            // selectedCard.transform.localScale = Vector3.one * 1.2f;

            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)
            {
                slot.SetHighlight(true);
            }
        }

        public void ExecuteState(GameManager manager) { }

        public void ExitState(GameManager manager)
        {
            Debug.Log("CardSelectedState: ExitState");
            selectedCard.transform.localScale = Vector3.one;

            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)
            {
                slot.SetHighlight(false);
            }
        }

        //On garde la fonction HandleCardClick mais on simplifie
        public void HandleCardClick(GameManager manager, CardController cardController, PointerEventData eventData)
        {
            Debug.Log("CardSelectedState.HandleCardClick: Clic détecté!");

            // 1. Est-ce que la carte cliquée est un slot du joueur courant ?
            CardSlotController slotController = cardController.GetComponentInParent<CardSlotController>();
            if (slotController != null && slotController.transform.IsChildOf(manager.CurrentPlayer.PlayerBoardController.transform))
            {
                Debug.Log("CardSelectedState.HandleCardClick: Clic sur un CardSlotController du joueur courant");

                // 2. Le slot appartient-il au plateau du joueur courant?
                if (manager.CurrentPlayer.PlayerBoardController.PlayerBoard.cardSlots.Contains(slotController.cardSlot))
                {
                    Debug.Log("CardSelectedState.HandleCardClick: Le slot appartient au PlayerBoard.");

                    // 3. Tenter de PLACER la carte sélectionnée.
                    slotController.PlaceCard(selectedCard);

                    Debug.Log("CardSelectedState.HandleCardClick: Carte placée, transition vers EndTurnState.");
                    manager.TransitionToState(new EndTurnState());
                    return;
                }
                else
                {
                    Debug.Log("CardSelectedState.HandleCardClick: Le slot n'appartient PAS au PlayerBoard.");
                }
            }
            else if (cardController == selectedCard)
            {
                Debug.Log("CardSelectedState.HandleCardClick: Clic sur la carte sélectionnée, retour à DrawChoiceState.");
                manager.TransitionToState(new DrawChoiceState());
                return;
            }
            else
            {
                Debug.Log("CardSelectedState.HandleCardClick: Pas de clic sur un CardSlotController du joueur courant.");
            }
        }
    }
}