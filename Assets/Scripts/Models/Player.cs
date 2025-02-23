// --- Models/Player.cs --- (Correction de CalculateInitialScore)

using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.Models
{
    public class Player
    {
        [ShowInInspector, ReadOnly]
        public int playerID;
        [ShowInInspector, ReadOnly]
        public List<Card> hand = new List<Card>(); //Main du joueur  -- Conserver, utile plus tard
        public int revealedCardCount = 0;
        public readonly int maxRevealedCards = 2;

        public Player(int id)
        {
            playerID = id;
        }

        public void AddCardToHand(Card card)
        {
            hand.Add(card);
        }

        public void RemoveCardFromHand(Card card)
        {
            hand.Remove(card);
        }

        public int CalculateScore()
        {
            int score = 0;
            // TODO: Implémenter la logique de calcul du score *complet* (règles du Skyjo).
            return score;
        }

        public bool RevealCard(Card card)
        {
            if (revealedCardCount < maxRevealedCards && !card.IsFaceUp)
            {
                card.Flip();
                revealedCardCount++;
                return true;
            }
            return false;
        }


        //CORRECTION : On calcule le score initial à partir du *plateau*, pas de la main.
        public int CalculateInitialScore()
        {
            int score = 0;
            PlayerBoard board = GameManager.Instance.players.Find(p => p.playerID == this.playerID).PlayerBoardController.PlayerBoard; //On trouve le plateau du joueur

            foreach (CardSlot slot in board.cardSlots) //On boucle sur les cartes du plateau
            {
                if (slot.IsOccupied && slot.card.IsFaceUp) //Si la carte est visible
                {
                    score += slot.card.Data.value; //On ajoute la valeur de la carte
                }
            }
            return score;
        }
    }
}