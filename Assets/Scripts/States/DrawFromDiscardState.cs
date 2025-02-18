// --- States/DrawFromDiscardState.cs ---



using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Managers;



namespace com.hyminix.game.ojyx.States

{

    public class DrawFromDiscardState : IGameState

    {

        private CardController drawnCard; //Carte pioché



        public void EnterState(GameManager manager)

        {

            Debug.Log("DrawFromDiscardState: Pioche une carte de la défausse...");

            drawnCard = manager.DeckController.DrawFromDiscardPile();//Pioche la carte de la défausse et la stock

            //Transition vers l'état de placement de la carte

            manager.TransitionToState(new CardPlacementState(drawnCard));

        }



        public void ExecuteState(GameManager manager) { }



        public void ExitState(GameManager manager) { }

    }

}

