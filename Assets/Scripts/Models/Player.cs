// --- Models/Player.cs ---

using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;



namespace com.hyminix.game.ojyx.Models

{

    public class Player

    {

        [ShowInInspector, ReadOnly]

        public int playerID;



        [ShowInInspector, ReadOnly]

        public List<Card> hand = new List<Card>(); //Main du joueur



        public int revealedCardCount = 0;

        public readonly int maxRevealedCards = 2; // Nombre maximal de cartes révélées en début de partie.



        public Player(int id)

        {

            playerID = id;

        }



        //Ajoute une carte a la main

        public void AddCardToHand(Card card)

        {

            hand.Add(card);

        }



        //Supprime une carte de la main

        public void RemoveCardFromHand(Card card)

        {

            hand.Remove(card);

        }



        //Calcule le score du joueur

        public int CalculateScore()

        {

            int score = 0;

            // TODO: Implémenter la logique de calcul du score (selon les règles du Skyjo).

            return score;

        }



        //Revele une carte

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

    }

}
