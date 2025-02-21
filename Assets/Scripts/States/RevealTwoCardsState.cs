// --- States/RevealTwoCardsState.cs --- (Utilisation de GetValidActions et ExecuteAction)
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
        }
        public void ExecuteState(GameManager manager)
        {
            // La logique est gérée via HandleCardClick.
        }
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
            // Dans RevealTwoCardsState, on ne peut que révéler une carte.
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
                // Vérifier si le slot a une carte et qu'elle appartient bien au joueur
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
                // Vérifie si le joueur a déjà révélé le maximum de cartes
                if (manager.CurrentPlayer.Player.revealedCardCount >= manager.CurrentPlayer.Player.maxRevealedCards)
                {
                    Debug.Log("RevealTwoCardsState.HandleCardClick: Le joueur a déjà révélé le maximum de cartes.");
                    return;
                }
                // Révélation de la carte
                slotController.CardController.Flip();
                manager.CurrentPlayer.Player.revealedCardCount++;
                Debug.Log("RevealTwoCardsState: Carte révélée. Nombre total révélé pour le joueur : " + manager.CurrentPlayer.Player.revealedCardCount);
                // Si le joueur a terminé sa révélation, passer au joueur suivant ou à l'état suivant.
                if (manager.CurrentPlayer.Player.revealedCardCount >= manager.CurrentPlayer.Player.maxRevealedCards)
                {
                    playersRevealedCount++;
                    Debug.Log("RevealTwoCardsState: Joueur " + manager.CurrentPlayer.playerID + " a révélé toutes ses cartes.");
                    if (playersRevealedCount >= manager.players.Count)
                    {
                        Debug.Log("RevealTwoCardsState: Tous les joueurs ont révélé leurs cartes. Transition vers PlayerTurnState.");
                        manager.TransitionToState(new PlayerTurnState());
                        return;
                    }
                    else
                    {
                        manager.NextPlayer();
                        Debug.Log("RevealTwoCardsState: Passe au joueur " + manager.CurrentPlayer.playerID + " pour la révélation.");
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
