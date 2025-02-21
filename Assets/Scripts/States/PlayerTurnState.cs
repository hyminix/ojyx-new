// --- States/PlayerTurnState.cs --- (Utilisation de GetValidActions et ExecuteAction)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Views;
using com.hyminix.game.ojyx.Enums; // Importer l'enum
using System.Collections.Generic;
namespace com.hyminix.game.ojyx.States
{
    public class PlayerTurnState : IGameState
    {
        public void EnterState(GameManager manager)
        {
            Debug.Log("PlayerTurnState: Début du tour du joueur " + manager.CurrentPlayer.playerID);
            manager.CurrentPlayer.StartTurn();
            //Transition direct
            manager.TransitionToState(new DrawChoiceState());
        }
        public void ExecuteState(GameManager manager)
        {
            // Plus de logique ici
        }
        public void ExitState(GameManager manager)
        {
            Debug.Log("PlayerTurnState: Fin du tour du joueur " + manager.CurrentPlayer.playerID);
        }
        //On ajoute la fonction HandleCardClick, mais elle est vide
        public void HandleCardClick(GameManager manager, CardSlotController slotController)
        {
            //Remplacer par GetValidActions et ExecuteAction
        }
        public List<PlayerAction> GetValidActions(GameManager manager)
        {
            // Dans PlayerTurnState, on peut piocher de la pioche ou de la défausse.
            return new List<PlayerAction>() { PlayerAction.DrawFromDeck, PlayerAction.DrawFromDiscard };
        }
        public void ExecuteAction(GameManager manager, PlayerAction action, CardSlotController cardSlot = null, CardController card = null)
        {
            if (!GetValidActions(manager).Contains(action))
            {
                Debug.LogError($"Action {action} non valide dans l'état PlayerTurnState !");
                return;
            }
            //Pas d'action dans cette état mais on laisse la structure en place
        }
        public PlayerAction? GetActionForCardSlotClick(GameManager manager)
        {
            throw new System.NotImplementedException();
        }
    }
}
