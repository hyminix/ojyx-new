// --- States/SetupState.cs --- (Version Complète)
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
                playerController.Initialize(playerController.playerID);
                playerController.DistributeInitialCards(manager.DeckController); // Distribue les cartes
            }

            // Désactive le CardController de *toutes* les cartes au début.
            foreach (var cardController in Object.FindObjectsOfType<CardController>(true)) // VRAIMENT important : true
            {
                cardController.SetDraggable(false);
            }

            manager.TransitionToState(new RevealTwoCardsState()); //On passe a la selection des deux cartes
        }

        public void ExecuteState(Managers.GameManager manager) { }
        public void ExitState(Managers.GameManager manager)
        {
            Debug.Log("SetupState: Fin de l'initialisation.");
        }
    }
}
