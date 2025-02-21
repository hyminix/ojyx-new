// --- States/DrawChoiceState.cs --- (Utilisation de GetValidActions et ExecuteAction)

using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Managers;

using com.hyminix.game.ojyx.Enums;

using System.Collections.Generic;



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

            //Remplacé par GetValidActions et ExecuteAction

        }



        public List<PlayerAction> GetValidActions(GameManager manager)

        {

            // Dans DrawChoiceState, on peut piocher de la pioche ou de la défausse.

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

                    //CORRECTION: Utilisation de DeckController directement.

                    CardController drawnCardDeck = manager.DeckController.DrawFromDeck();

                    if (drawnCardDeck != null)

                    {

                        drawnCardDeck.Flip(); // Toujours flipper une carte piochée de la pioche.

                        manager.TransitionToState(new CardSelectedState(drawnCardDeck));

                    }

                    else

                    {

                        Debug.LogError("Impossible de piocher depuis le deck !");

                    }

                    break;

                case PlayerAction.DrawFromDiscard:

                    //CORRECTION: Utilisation de DeckController directement.

                    CardController drawnCardDiscard = manager.DeckController.DrawFromDiscardPile();

                    if (drawnCardDiscard != null)

                    {

                        // Pas besoin de Flip() ici, la carte de la défausse est déjà visible.

                        manager.TransitionToState(new CardSelectedState(drawnCardDiscard));

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
            throw new System.NotImplementedException();
        }
    }

}
