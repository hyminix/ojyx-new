// --- States/RevealTwoCardsState.cs --- (Appel de ActivateCurrentPlayer après DetermineFirstPlayer)

using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Enums;
using System.Collections.Generic;

namespace com.hyminix.game.ojyx.States
{
    public class RevealTwoCardsState : IGameState
    {
        private int playersRevealedCount = 0;

        public void EnterState(GameManager manager)
        {
            Debug.Log("RevealTwoCardsState: Chaque joueur doit révéler deux cartes.");
            playersRevealedCount = 0;
            Debug.Log("Joueur " + manager.CurrentPlayer.playerID + " : veuillez révéler deux cartes.");
            //On supprime l'appel au UIManager
        }

        public void ExecuteState(GameManager manager) { }
        public void ExitState(GameManager manager)
        {
            Debug.Log("RevealTwoCardsState: Fin de la phase de révélation.");
        }

        public void HandleCardClick(GameManager manager, CardSlotController slotController)
        {
            //Remplacé par GetValidActions et ExecuteAction
        }

        public List<PlayerAction> GetValidActions(GameManager manager)
        {
            return new List<PlayerAction>() { PlayerAction.RevealCard };
        }

        public void ExecuteAction(GameManager manager, PlayerAction action, CardSlotController slotController = null, CardController card = null)
        {
            if (!GetValidActions(manager).Contains(action))
            {
                Debug.LogError($"Action {action} non valide dans l'état RevealTwoCardsState !");
                return;
            }

            if (action == PlayerAction.RevealCard)
            {
                if (slotController?.CardController == null || !IsCardBelongToPlayer(manager.CurrentPlayer, slotController.CardController.Card))
                {
                    Debug.Log("RevealTwoCardsState.HandleCardClick: Carte invalide ou n'appartenant pas au joueur.");
                    return;
                }

                if (slotController.CardController.Card.IsFaceUp)
                {
                    Debug.Log("RevealTwoCardsState.HandleCardClick: Carte déjà révélée.");
                    return;
                }

                if (manager.CurrentPlayer.Player.revealedCardCount >= manager.CurrentPlayer.Player.maxRevealedCards)
                {
                    Debug.Log("RevealTwoCardsState.HandleCardClick: Le joueur a déjà révélé le maximum de cartes.");
                    return;
                }

                slotController.CardController.Flip();
                manager.CurrentPlayer.Player.revealedCardCount++;
                manager.CurrentPlayer.PlayerBoardController.UpdateDebugInfo();
                Debug.Log("RevealTwoCardsState: Carte révélée. Nombre total révélé pour le joueur : " + manager.CurrentPlayer.Player.revealedCardCount);

                if (manager.CurrentPlayer.Player.revealedCardCount >= manager.CurrentPlayer.Player.maxRevealedCards)
                {
                    playersRevealedCount++;
                    Debug.Log("RevealTwoCardsState: Joueur " + manager.CurrentPlayer.playerID + " a révélé toutes ses cartes.");


                    if (playersRevealedCount >= manager.players.Count)
                    {
                        manager.DetermineFirstPlayer();
                        // *** Appel de ActivateCurrentPlayer AVANT la transition ***
                        manager.ActivateCurrentPlayer();
                        manager.TransitionToState(new PlayerTurnState());
                        return;
                    }
                    else
                    {
                        manager.NextPlayer(); // On passe au joueur suivant pour la révélation
                        //Le SetInfo se fera dans ActivateCurrentPlayer
                    }
                }
            }
        }
        public PlayerAction? GetActionForCardSlotClick(GameManager manager)
        {
            return PlayerAction.RevealCard;
        }

        private bool IsCardBelongToPlayer(PlayerController playerController, Models.Card card)
        {
            foreach (var slot in playerController.PlayerBoardController.playerBoardView.cardSlots)
            {
                if (slot.cardSlot.card != null && slot.cardSlot.card == card)
                {
                    return true;
                }
            }
            return false;
        }
    }
}