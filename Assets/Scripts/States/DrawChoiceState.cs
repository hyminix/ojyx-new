// --- States/DrawChoiceState.cs ---
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.States
{
    public class DrawChoiceState : IGameState
    {
        private DeckController deckController;
        private GameManager gameManager;

        public void EnterState(GameManager manager)
        {
            Debug.Log("DrawChoiceState: Choisissez entre piocher ou prendre la défausse.");
            deckController = manager.DeckController;
            gameManager = manager;
        }

        public void ExecuteState(GameManager manager) { }

        public void ExitState(GameManager manager) { }

        public void HandleCardClick(GameManager manager, CardSlotController slotController)
        {
            //Cette état ne doit plus gérer les clics
        }
    }
}