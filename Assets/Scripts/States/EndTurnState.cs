// --- States/EndTurnState.cs --- (Complet)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
using UnityEngine.EventSystems;

namespace com.hyminix.game.ojyx.States
{
    public class EndTurnState : IGameState
    {
        public void EnterState(Managers.GameManager manager)
        {
            Debug.Log("EndTurnState: Fin du tour.");
            manager.CurrentPlayer.EndTurn(); // Termine le tour du joueur actuel. Cette méthode existe déjà.
            CheckRoundEnd(manager); // Vérifie si la manche est terminée
        }

        public void ExecuteState(Managers.GameManager manager) { }
        public void ExitState(Managers.GameManager manager) { }

        public void HandleCardClick(GameManager manager, CardSlotController cardSlotController)
        {
            throw new System.NotImplementedException();
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
    }
}