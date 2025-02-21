// --- States/RevealCardState.cs --- (Complet et Optimisé)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
//SUPPRIMER using UnityEngine.EventSystems;

namespace com.hyminix.game.ojyx.States
{
    public class RevealCardState : IGameState
    {
        public void EnterState(GameManager manager)
        {
            Debug.Log("RevealCardState: Veuillez révéler une carte face cachée.");
        }

        public void ExecuteState(GameManager manager)
        {

        }

        public void ExitState(GameManager manager)
        {

        }

        public void HandleCardClick(GameManager manager, CardSlotController slotController)
        {
            // Vérifier si le slot a une carte
            if (slotController.CardController == null)
            {
                return;
            }

            // Vérifie si la carte cliquée appartient au joueur actuel *ET* si elle est face cachée.
            if (slotController.CardController.Card.IsFaceUp || !IsCardBelongToPlayer(manager.CurrentPlayer, slotController.CardController.Card))
            {
                return; // Ne fait rien si la carte est déjà face visible ou n'appartient pas au joueur
            }

            //Revele la carte visuellement et au niveau du model
            slotController.CardController.Flip();
            manager.TransitionToState(new EndTurnState()); //On passe a la fin de tour
        }

        //Fonction pour verifier si la carte appartient bien au joueur
        private bool IsCardBelongToPlayer(PlayerController playerController, Models.Card card)
        {
            foreach (var slot in playerController.PlayerBoardController.playerBoardView.cardSlots)
            {
                //Si le slot n'est pas vide
                if (slot.cardSlot.card != null)
                {
                    //Si la carte du slot correspond a la carte cliqué
                    if (slot.cardSlot.card == card)
                    {
                        return true; //On retourne
                    }
                }
            }
            return false;
        }
    }
}