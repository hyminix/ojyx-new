// --- States/CardSelectedState.cs ---
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;

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
            // selectedCard.transform.localScale = Vector3.one * 1.2f; // Optionnel: mise en évidence visuelle

            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)
            {
                slot.SetHighlight(true); // Met en évidence les emplacements *du joueur courant*.
            }
        }

        public void ExecuteState(GameManager manager) { }

        public void ExitState(GameManager manager)
        {
            Debug.Log("CardSelectedState: ExitState");
            // selectedCard.transform.localScale = Vector3.one;

            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)
            {
                slot.SetHighlight(false); // Désactive la mise en évidence.
            }
        }

        public void HandleCardClick(GameManager manager, CardSlotController slotController)
        {
            Debug.Log("CardSelectedState.HandleCardClick: Clic détecté!");

            // 1. Le slot cliqué appartient-il au plateau du joueur courant ?
            if (slotController != null && slotController.transform.IsChildOf(manager.CurrentPlayer.PlayerBoardController.transform))
            {
                Debug.Log("CardSelectedState.HandleCardClick: Clic sur un CardSlotController du joueur courant");

                // 2. Placer la carte sélectionnée (PlaceCard gère la logique).
                slotController.PlaceCard(selectedCard);
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