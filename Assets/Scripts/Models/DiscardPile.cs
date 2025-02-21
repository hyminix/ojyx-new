// --- Models/DiscardPile.cs ---

using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;



namespace com.hyminix.game.ojyx.Models

{

    public class DiscardPile

    {

        [ShowInInspector, ReadOnly]

        public List<Card> cards = new List<Card>(); // Pile de cartes



        public Card TopCard => cards.Count > 0 ? cards[cards.Count - 1] : null;



        // Ajoute une carte sur le dessus de la défausse

        public void AddCard(Card card)

        {

            if (card == null)

            {

                Debug.LogError("Trying to add a null card to the discard pile!");

                return;

            }

            card.IsFaceUp = true; // Les cartes dans la défausse sont face visible

            cards.Add(card);

        }



        // Retire la carte du dessus

        public Card DrawCard()

        {

            if (cards.Count == 0) return null;

            Card card = cards[cards.Count - 1];

            cards.RemoveAt(cards.Count - 1);

            return card;

        }



        // Vide la défausse

        public void Clear()

        {

            cards.Clear();

        }



        // Récupère toutes les cartes sauf la dernière (pour reshuffle)

        public List<Card> GetCardsForReshuffle()

        {

            List<Card> reshuffledCards = new List<Card>(cards);

            if (reshuffledCards.Count > 0)

            {

                reshuffledCards.RemoveAt(reshuffledCards.Count - 1);

            }

            return reshuffledCards;

        }

    }

}
