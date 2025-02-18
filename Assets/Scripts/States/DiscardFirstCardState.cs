// --- States/DiscardFirstCardState.cs ---

using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Managers;



namespace com.hyminix.game.ojyx.States

{

    public class DiscardFirstCardState : IGameState

    {

        public void EnterState(GameManager manager)

        {

            Debug.Log("DiscardFirstCardState: Déplacement de la première carte de la pioche à la défausse.");

            manager.DeckController.MoveCardFromDeckToDiscard(); // Déplace la carte.

            manager.TransitionToState(new PlayerTurnState()); // Passe au tour du premier joueur.

        }



        public void ExecuteState(GameManager manager) { }

        public void ExitState(GameManager manager) { }

    }

}
