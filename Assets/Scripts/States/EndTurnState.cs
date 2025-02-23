// --- States/EndTurnState.cs --- (Appel de CheckBoard)

using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Managers;

using UnityEngine.EventSystems;

using com.hyminix.game.ojyx.Enums;

using System.Collections.Generic;



namespace com.hyminix.game.ojyx.States

{

    public class EndTurnState : IGameState

    {

        public void EnterState(Managers.GameManager manager)

        {

            Debug.Log("EndTurnState: Fin du tour.");

            manager.CurrentPlayer.EndTurn(); // Termine le tour du joueur actuel.



            // AJOUT : Vérification des lignes/colonnes complètes.

            manager.CurrentPlayer.PlayerBoardController.CheckBoard();



            CheckRoundEnd(manager); // Vérifie si la manche est terminée

        }



        public void ExecuteState(Managers.GameManager manager) { }

        public void ExitState(Managers.GameManager manager) { }



        public void HandleCardClick(GameManager manager, CardSlotController cardSlotController)

        {

            //Remplacé par GetValidActions et ExecuteAction

        }



        //Dans End Turn State on ne peux rien faire

        public List<PlayerAction> GetValidActions(GameManager manager)

        {

            return new List<PlayerAction>();

        }



        public void ExecuteAction(GameManager manager, PlayerAction action, CardSlotController cardSlot = null, CardController card = null)

        {

            //Normalement on y passe jamais

        }



        //Fonction pour vérifier si la manche est terminée

        private void CheckRoundEnd(Managers.GameManager manager)

        {

            //On verifie si un joueur a revelé toutes ses cartes

            foreach (var player in manager.players)

            {

                if (player.PlayerBoardController.AreAllCardsRevealed())

                {

                    //On passe à l'état de fin de manche

                    manager.TransitionToState(new EndRoundState());

                    return;

                }

            }



            //Sinon on passe au joueur suivant

            manager.NextPlayer();

            manager.TransitionToState(new PlayerTurnState()); // Passe au tour du joueur suivant.

        }


        public PlayerAction? GetActionForCardSlotClick(GameManager manager, CardSlotController cardSlotController)
        {
            throw new System.NotImplementedException();
        }

        public PlayerAction? GetActionForCardSlotClick(GameManager manager)
        {
            throw new System.NotImplementedException();
        }
    }

}
