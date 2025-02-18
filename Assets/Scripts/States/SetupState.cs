// --- States/SetupState.cs ---

using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Views;



namespace com.hyminix.game.ojyx.States

{

    public class SetupState : IGameState

    {

        public void EnterState(Managers.GameManager manager)

        {

            Debug.Log("SetupState: Initialisation du jeu...");



            // Initialise les joueurs.

            foreach (var playerController in manager.players)

            {

                playerController.Initialize(playerController.playerID); // Initialisation

                // Distribue les cartes initiales a chaque joueur.

                playerController.DistributeInitialCards(manager.DeckController);

            }



            // Déplace une carte de la pioche à la défausse.  <- PLUS ICI

            // manager.DeckController.MoveCardFromDeckToDiscard();



            //Abonnement aux evenements

            SubscribeToEvents(manager);



            // Passe à l'état de tour du premier joueur (ou un autre état initial).

            // manager.TransitionToState(new PlayerTurnState()); //On le fait plus ici

            manager.TransitionToState(new RevealTwoCardsState()); //On passe a la selection des deux cartes

        }



        public void ExecuteState(Managers.GameManager manager) { }

        public void ExitState(Managers.GameManager manager)

        {

            Debug.Log("SetupState: Fin de l'initialisation.");

            //Desabonnement aux evenements

            UnsubscribeFromEvents(manager);

        }

        //Abonnement aux evenements

        private void SubscribeToEvents(Managers.GameManager manager)

        {

            foreach (var player in manager.players)

            {

                foreach (var slot in player.PlayerBoardController.playerBoardView.cardSlots) //On récupere la liste des slots

                {

                    slot.OnCardPlaced += manager.OnCardPlaced; //On s'abonne

                    slot.OnCardRemoved += manager.OnCardRemoved;

                }

            }

        }

        //Desabonnement aux evenements

        private void UnsubscribeFromEvents(Managers.GameManager manager)

        {

            foreach (var player in manager.players)

            {

                foreach (var slot in player.PlayerBoardController.playerBoardView.cardSlots) //On récupere la liste des slots

                {

                    slot.OnCardPlaced -= manager.OnCardPlaced; //On se désabonne

                    slot.OnCardRemoved -= manager.OnCardRemoved;

                }

            }

        }

    }

}

