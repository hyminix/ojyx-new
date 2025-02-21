// --- States/EndRoundState.cs --- (Version complète avec GetValidActions/ExecuteAction)
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using com.hyminix.game.ojyx.Enums;
using System.Collections.Generic;
namespace com.hyminix.game.ojyx.States
{
    public class EndRoundState : IGameState
    {
        public void EnterState(Managers.GameManager manager)
        {
            Debug.Log("EndRoundState : Fin de la manche, calcul des scores");
            CalculateScores(manager); //On calcule les scores
                                      //TODO : Vérifier si la partie est terminée
                                      //Si la partie n'est pas terminée, on passe à la manche suivante
            manager.TransitionToState(new SetupState()); //Pour l'instant on relance une manche
        }
        public void ExecuteState(Managers.GameManager manager)
        {
        }
        public void ExitState(Managers.GameManager manager)
        {
        }
        //Fonction de calcul des scores
        private void CalculateScores(Managers.GameManager manager)
        {
            //On parcours les joueurs
            foreach (var player in manager.players)
            {
                //Calcul du score
                int score = CalculatePlayerScore(player); // Utilisation de la nouvelle méthode
                Debug.Log("Score du joueur " + player.playerID + " : " + score);
            }
        }
        // Nouvelle méthode pour calculer le score d'UN joueur.
        private int CalculatePlayerScore(PlayerController player)
        {
            int score = 0;
            foreach (var slot in player.PlayerBoardController.PlayerBoard.cardSlots)
            {
                if (slot.IsOccupied && slot.card.IsFaceUp)
                {
                    score += slot.card.Data.value;
                }
            }
            return score;
        }
        public void HandleCardClick(GameManager manager, CardSlotController cardSlotController)
        {
            // Remplacé par GetValidActions et ExecuteAction
        }
        public List<PlayerAction> GetValidActions(GameManager manager)
        {
            return new List<PlayerAction>(); // Aucune action valide dans cet état.
        }
        public void ExecuteAction(GameManager manager, PlayerAction action, CardSlotController cardSlot = null, CardController card = null)
        {
            // Normalement, on n'arrive jamais ici, car GetValidActions retourne une liste vide.
            // On garde une sécurité, au cas où.
            Debug.LogError($"Action {action} non valide dans l'état EndRoundState !");
        }
        public PlayerAction? GetActionForCardSlotClick(GameManager manager)
        {
            throw new System.NotImplementedException();
        }
    }
}
