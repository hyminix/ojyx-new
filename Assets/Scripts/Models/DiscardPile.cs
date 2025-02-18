// --- Models/DiscardPile.cs --- (Suite)

using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;



namespace com.hyminix.game.ojyx.Models

{

    public class DiscardPile

    {

        [ShowInInspector, ReadOnly]

        public List<Card> cards = new List<Card>();  // Utilisation du modèle Card

        public Card TopCard => cards.Count > 0 ? cards[cards.Count - 1] : null;



        //Ajoute une carte a la defausse

        public void AddCard(Card card)

        {

            if (card == null)

            {

                Debug.LogError("Trying to add a null card to the discard pile!");

                return;

            }

            card.IsFaceUp = true; // Les cartes dans la défausse sont toujours face visible.

            cards.Add(card);

        }



        //Retire une carte de la défausse

        public Card DrawCard()

        {

            if (cards.Count == 0) return null;

            Card card = cards[cards.Count - 1];

            cards.RemoveAt(cards.Count - 1);

            return card;

        }



        //Vide la defausse

        public void Clear()

        {

            cards.Clear();

        }

        //Récupère les cartes de la défausse (sauf la dernière)

        public List<Card> GetCardsForReshuffle()

        {

            List<Card> reshuffledCards = new List<Card>(cards);  // Copie la liste

            if (reshuffledCards.Count > 0)

            {

                reshuffledCards.RemoveAt(reshuffledCards.Count - 1); // Garde la carte du dessus.

            }

            return reshuffledCards;

        }

    }

}
