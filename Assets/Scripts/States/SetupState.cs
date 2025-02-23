// --- States/SetupState.cs --- (Pas d'action spécifique dans SetupState)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Views;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Enums;
using System.Collections.Generic;
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
                playerController.DistributeInitialCards(); // Distribue les cartes
            }
            manager.TransitionToState(new DiscardFirstCardState()); //On passe a la selection des deux cartes
        }
        public void ExecuteState(Managers.GameManager manager) { }
        public void ExitState(Managers.GameManager manager)
        {
            Debug.Log("SetupState: Fin de l'initialisation.");
        }
        public void HandleCardClick(Managers.GameManager manager, CardSlotController slotController)
        {
            //Remplacer par GetValidActions et ExecuteAction
        }
        public List<PlayerAction> GetValidActions(GameManager manager)
        {
            return new List<PlayerAction>(); // Aucune action possible dans cet état.
        }
        public void ExecuteAction(GameManager manager, PlayerAction action, CardSlotController cardSlot = null, CardController card = null)
        {
            // Rien à faire ici, car aucune action n'est valide.
        }
        public PlayerAction? GetActionForCardSlotClick(GameManager manager)
        {
            throw new System.NotImplementedException();
        }
    }
}