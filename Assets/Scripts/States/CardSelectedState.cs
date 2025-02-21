// --- States/CardSelectedState.cs ---
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;

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
            // selectedCard.transform.localScale = Vector3.one * 1.2f; // On garde ça pour plus tard, quand on aura réglé les pb de DOTween.

            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)
            {
                slot.SetHighlight(true);
            }
        }

        public void ExecuteState(GameManager manager) { }

        public void ExitState(GameManager manager)
        {
            Debug.Log("CardSelectedState: ExitState");
            // selectedCard.transform.localScale = Vector3.one;  // On garde ça pour plus tard

            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)
            {
                slot.SetHighlight(false);
            }
        }

        public void HandleCardClick(GameManager manager, CardSlotController slotController)
        {
            Debug.Log("CardSelectedState.HandleCardClick: Clic détecté!");

            // 1. Est-ce que le slot cliqué appartient au plateau du joueur courant ?
            if (slotController != null && slotController.transform.IsChildOf(manager.CurrentPlayer.PlayerBoardController.transform))
            {
                Debug.Log("CardSelectedState.HandleCardClick: Clic sur un CardSlotController du joueur courant");

                // 2. Tenter de PLACER la carte sélectionnée.
                slotController.PlaceCard(selectedCard); // Place card s'occupe de tout

                Debug.Log("CardSelectedState.HandleCardClick: Carte placée, transition vers EndTurnState.");
                manager.TransitionToState(new EndTurnState());


            }
            else
            {
                Debug.Log("CardSelectedState.HandleCardClick: Pas de clic sur un CardSlotController du joueur courant.");
            }
        }
    }
}