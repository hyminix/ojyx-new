using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Managers;



namespace com.hyminix.game.ojyx.States

{

    public class RevealCardState : IGameState, ICardClickHandler

    {



        public void EnterState(GameManager manager)

        {

            Debug.Log("RevealCardState: Veuillez révéler une carte face cachée.");

            SubscribeToEvents(manager);

        }

        public void ExecuteState(GameManager manager)

        {



        }

        public void ExitState(GameManager manager)

        {

            UnsubscribeFromEvents(manager);

        }

        public void HandleCardClick(GameManager manager, CardController cardController)

        {

            // Vérifie si la carte cliquée appartient au joueur actuel *ET* si elle est face cachée.

            if (cardController.Card.IsFaceUp || !IsCardBelongToPlayer(manager.CurrentPlayer, cardController.Card))

            {

                return; // Ne fait rien si la carte est déjà face visible ou n'appartient pas au joueur

            }

            //Revele la carte visuellement et au niveau du model

            cardController.Flip();

            manager.TransitionToState(new EndTurnState()); //On passe a la fin de tour

        }

        //Fonction pour verifier si la carte appartient bien au joueur

        private bool IsCardBelongToPlayer(PlayerController playerController, Models.Card card)

        {

            foreach (var slot in playerController.PlayerBoardController.playerBoardView.cardSlots)

            {

                //Si le slot n'est pas vide

                if (slot.cardSlot.card != null)

                {

                    //Si la carte du slot correspond a la carte cliqué

                    if (slot.cardSlot.card == card)

                    {

                        return true; //On retourne true

                    }

                }

            }

            return false;

        }

        private void SubscribeToEvents(GameManager manager)

        {

            //On s'abonne aux evenements des cartes du joueur

            foreach (var card in Object.FindObjectsOfType<CardController>())

            {

                card.OnCardClicked += manager.OnCardClicked; //On s'abonne

            }

        }

        private void UnsubscribeFromEvents(GameManager manager)

        {

            foreach (var card in Object.FindObjectsOfType<CardController>())

            {

                card.OnCardClicked -= manager.OnCardClicked; //On se désabonne

            }

        }

    }

}
