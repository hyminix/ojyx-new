// --- States/PlayerTurnState.cs --- (Simplifié : Plus de logique de clic)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Views;
using UnityEngine.EventSystems;

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
            //On supprime le unsubscribe
        }

        //On ajoute la fonction HandleCardClick
        public void HandleCardClick(GameManager manager, CardController cardController, PointerEventData eventData)
        {

        }
    }
}