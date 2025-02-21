// --- States/DrawChoiceState.cs ---
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.States
{
    public class DrawChoiceState : IGameState
    {
        private DeckController deckController;
        private GameManager gameManager;

        public void EnterState(GameManager manager)
        {
            Debug.Log("DrawChoiceState: Choisissez entre piocher ou prendre la défausse.");
            deckController = manager.DeckController;
            gameManager = manager;
        }

        public void ExecuteState(GameManager manager) { }

        public void ExitState(GameManager manager) { }

        public void HandleCardClick(GameManager manager, CardSlotController slotController)
        {
            // Debug.Log("DrawChoiceState.HandleCardClick: Clic détecté!");

            // // Vérifie si le clic concerne la carte du dessus du deck
            // CardController topDeckCard = deckController.DeckView.PeekTopCardController();
            // //Modif : On verifie si le controller n'est pas null avant d'acceder a ses propriétés
            // if (topDeckCard != null && slotController.CardController != null && slotController.CardController == topDeckCard)
            // {
            //     Debug.Log("DrawChoiceState.HandleCardClick: Clic sur la carte du dessus de la pioche.");
            //     topDeckCard.Flip(); // On flip si besoin
            //     gameManager.TransitionToState(new CardSelectedState(topDeckCard));
            //     return;
            // }

            // // Vérifie si le clic concerne la carte du dessus de la défausse
            // CardController topDiscardCard = deckController.discardPileView.GetTopCardController();
            // //Modif : On verifie si le controller n'est pas null avant d'acceder a ses propriétés
            // if (topDiscardCard != null && slotController.CardController != null && slotController.CardController == topDiscardCard)
            // {
            //     Debug.Log("DrawChoiceState.HandleCardClick: Clic sur la carte du dessus de la défausse.");
            //     // Pas besoin de flipper la carte de la défausse, elle est déjà visible.
            //     gameManager.TransitionToState(new CardSelectedState(topDiscardCard));
            //     return;
            // }

            // Debug.LogWarning("DrawChoiceState.HandleCardClick: Clic sur une carte non gérée dans cet état.");
        }
    }
}