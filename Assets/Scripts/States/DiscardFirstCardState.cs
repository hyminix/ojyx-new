// --- States/DiscardFirstCardState.cs --- (Complet)
using UnityEngine;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Controllers;
using UnityEngine.EventSystems;
namespace com.hyminix.game.ojyx.States
{
    public class DiscardFirstCardState : IGameState
    {
        public void EnterState(GameManager manager)
        {
            Debug.Log("DiscardFirstCardState: Déplacement de la première carte de la pioche à la défausse.");
            manager.DeckController.MoveCardFromDeckToDiscard(); // Déplace la carte.
            manager.TransitionToState(new RevealTwoCardsState()); // Passe au tour du premier joueur.
        }

        public void ExecuteState(GameManager manager) { }
        public void ExitState(GameManager manager) { }

        public void HandleCardClick(GameManager manager, CardSlotController cardSlotController)
        {
            throw new System.NotImplementedException();
        }
    }
}