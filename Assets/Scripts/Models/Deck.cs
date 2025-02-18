// --- Models/Deck.cs ---

using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;



namespace com.hyminix.game.ojyx.Models

{

    public class Deck

    {

        [ShowInInspector, ReadOnly]

        public List<Card> cards = new List<Card>(); //Liste de cartes



        //Initialisation du deck

        public Deck(List<Data.CardData> cardDatas)

        {

            foreach (var cardData in cardDatas) //Pour chaque carte

            {

                int count = GetCardCount(cardData.value); //On recupere le nombre d'exemplaire

                //On ajoute les cartes

                for (int i = 0; i < count; i++)

                {

                    cards.Add(new Card(cardData));

                }

            }

            Shuffle(); //On melange

        }

        //Fonction pour récuperer le nombre de cartes

        private int GetCardCount(int value)

        {

            switch (value)

            {

                case -2: return 5;

                case 0: return 15;

                case -1: return 10;

                default: return 10;

            }

        }



        //Fonction pour melanger le deck

        public void Shuffle()

        {

            //Melange de Fisher-Yates

            for (int i = 0; i < cards.Count; i++)

            {

                Card temp = cards[i];

                int randomIndex = Random.Range(i, cards.Count);

                cards[i] = cards[randomIndex];

                cards[randomIndex] = temp;

            }

        }



        //Fonction de pioche

        public Card DrawCard()

        {

            if (cards.Count == 0) return null;

            Card drawn = cards[0];

            cards.RemoveAt(0); //On la supprime

            return drawn;

        }

        //Fonction pour remettre les cartes dans la pioche

        public void AddCards(List<Card> cards)

        {

            this.cards.AddRange(cards);

        }

    }

}

