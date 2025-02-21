// --- States/SetupState.cs --- (Version Complète)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Views;
using com.hyminix.game.ojyx.Managers;
//SUPPRIMER using UnityEngine.EventSystems;

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
            //SUPPRIMER Appel de la souscription aux evenements
            //SUPPRIMER manager.SubscribeToCardSlotEvents();
            manager.TransitionToState(new DiscardFirstCardState()); //On passe a la selection des deux cartes
        }

        public void ExecuteState(Managers.GameManager manager) { }
        public void ExitState(Managers.GameManager manager)
        {
            Debug.Log("SetupState: Fin de l'initialisation.");
        }

        // Ajout de la méthode HandleCardClick, mais elle est vide
        public void HandleCardClick(Managers.GameManager manager, CardSlotController slotController)
        {
            // Ne rien faire dans cet état
        }
    }
}