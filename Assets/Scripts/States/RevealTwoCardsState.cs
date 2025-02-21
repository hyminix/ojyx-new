// --- States/RevealTwoCardsState.cs ---
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
//SUPPRIMER using UnityEngine.EventSystems;

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
            // Vérifie si la carte est valide (existe, non déjà révélée, appartient au joueur courant)

            // Vérifier si le slot a une carte
            if (slotController.CardController == null)
            {
                return;
            }

            if (slotController.CardController.Card.IsFaceUp)
            {
                Debug.Log("RevealTwoCardsState.HandleCardClick: Carte déjà révélée.");
                return;
            }
            if (!IsCardBelongToPlayer(manager.CurrentPlayer, slotController.CardController.Card))
            {
                Debug.Log("RevealTwoCardsState.HandleCardClick: La carte ne correspond pas au joueur courant.");
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

            // Si le joueur a terminé sa révélation
            if (manager.CurrentPlayer.Player.revealedCardCount >= manager.CurrentPlayer.Player.maxRevealedCards)
            {
                playersRevealedCount++;
                Debug.Log("RevealTwoCardsState: Joueur " + manager.CurrentPlayer.playerID + " a révélé toutes ses cartes.");

                if (playersRevealedCount >= manager.players.Count)
                {
                    Debug.Log("RevealTwoCardsState: Tous les joueurs ont révélé leurs cartes. Transition vers DiscardFirstCardState.");
                    manager.TransitionToState(new PlayerTurnState());
                    return;
                }
                else
                {
                    // Passage au joueur suivant
                    manager.NextPlayer();
                    Debug.Log("RevealTwoCardsState: Passe au joueur " + manager.CurrentPlayer.playerID + " pour la révélation.");
                }
            }
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