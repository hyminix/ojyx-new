using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
using UnityEngine.EventSystems;

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

        public void HandleCardClick(GameManager manager, CardController cardController, PointerEventData eventData)
        {
            Debug.Log("DrawChoiceState.HandleCardClick: Clic détecté!");

            // Vérifie si le clic concerne la carte du dessus du deck
            CardController topDeckCard = deckController.DeckView.PeekTopCardController();
            if (topDeckCard != null && cardController == topDeckCard)
            {
                Debug.Log("DrawChoiceState.HandleCardClick: Clic sur la carte du dessus de la pioche.");
                cardController.Flip(); // On flip si besoin
                gameManager.TransitionToState(new CardSelectedState(cardController));
                return;
            }

            // Vérifie si le clic concerne la carte du dessus de la défausse
            CardController topDiscardCard = deckController.discardPileView.GetTopCardController();
            if (topDiscardCard != null && cardController == topDiscardCard)
            {
                Debug.Log("DrawChoiceState.HandleCardClick: Clic sur la carte du dessus de la défausse.");
                gameManager.TransitionToState(new CardSelectedState(cardController));
                return;
            }

            Debug.LogWarning("DrawChoiceState.HandleCardClick: Clic sur une carte non gérée dans cet état.");
        }
    }
}
