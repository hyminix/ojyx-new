// --- States/DrawFromDeckState.cs ---



using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Managers;



namespace com.hyminix.game.ojyx.States

{

    public class DrawFromDeckState : IGameState

    {

        private CardController drawnCard; //Carte pioché

        public void EnterState(GameManager manager)

        {

            Debug.Log("DrawFromDeckState: Pioche une carte...");

            drawnCard = manager.DeckController.DrawFromDeck(); //Pioche une carte et la stock

            //Transition vers l'état de placement de la carte

            manager.TransitionToState(new CardPlacementState(drawnCard));

        }



        public void ExecuteState(GameManager manager) { }



        public void ExitState(GameManager manager) { }

    }

}
