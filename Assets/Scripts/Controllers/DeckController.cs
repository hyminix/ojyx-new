// --- Controllers/DeckController.cs ---

using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

using com.hyminix.game.ojyx.Models;

using com.hyminix.game.ojyx.Views;

using com.hyminix.game.ojyx.Data;



namespace com.hyminix.game.ojyx.Controllers

{

    public class DeckController : MonoBehaviour

    {

        [Title("Configuration")]

        [SerializeField] private List<CardData> cardTemplates;  // Les données de carte (ScriptableObjects).



        [Title("Prefabs & Positions")]

        public GameObject cardPrefab; // Le prefab de carte 3D

        public Transform deckPosition;  // La position du deck



        [Title("Composants")]

        [SerializeField, ReadOnly] private Deck deck;  // Le modèle Deck.

        [SerializeField, ReadOnly] private DeckView deckView; // La vue DeckView

        [SerializeField, ReadOnly] private DiscardPile discardPile;  // Le modèle DiscardPile.

        [SerializeField] public DiscardPileView discardPileView; // La vue DiscardPileView.



        // Événements (pourraient être déplacés dans un script séparé si besoin de plus de centralisation).

        public delegate void CardDrawAction(Card card); //Delegate

        public static event CardDrawAction OnCardDrawnFromDeck;

        public static event CardDrawAction OnCardDrawnFromDiscard;





        private void Start()

        {

            discardPile = new DiscardPile(); // Initialise la défausse

            // InitializeDeck();  // Initialise le deck  <- On le fait dans le GameManager

        }



        [Button("Initialiser le Deck")]

        public void InitializeDeck()

        {

            deck = new Deck(cardTemplates);  // Initialise le modèle Deck.

            deckView = GetComponent<DeckView>();

            if (deckView == null)

            {

                Debug.LogError("Pas de DeckView trouvé !");

                return;

            }



            discardPile.Clear();

            discardPileView.Clear(); // Nettoie la vue

            GenerateDeckView(); // Initialise la vue du deck.



        }



        private void GenerateDeckView()

        {

            if (deck.cards.Count == 0)

            {

                Debug.LogError("Le deck est vide !");

                return;

            }



            foreach (var card in deck.cards)

            {

                //On utilise la fonction centralisé

                CardController cardController = CreateCardController(card);

                cardController.transform.position = deckPosition.position;

            }

        }



        [Button("Piocher une carte")]

        public CardController DrawFromDeck()

        {

            Card drawnCard = deck.DrawCard();

            if (drawnCard == null)

            {

                ReshuffleDiscardIntoDeck(); //On remelange

                drawnCard = deck.DrawCard(); // Tente de piocher à nouveau

                if (drawnCard == null)

                {

                    Debug.LogError("La pioche et la défausse sont vides!");

                    return null;

                }

            }



            //On utilise la fonction de création centralisé

            CardController cardController = CreateCardController(drawnCard);

            cardController.transform.position = deckPosition.position; //On place la carte

            OnCardDrawnFromDeck?.Invoke(drawnCard);

            return cardController;

        }



        //Fonction pour la pioche initiale, elle ne doit pas faire apparaitre la carte au niveau visuel

        public Card DrawFromDeckInitial()

        {

            Card drawnCard = deck.DrawCard(); //On pioche la carte

            if (drawnCard == null) //Si il n'y a plus de carte

            {

                ReshuffleDiscardIntoDeck(); // On remelange la pioche

                drawnCard = deck.DrawCard(); // Tente de piocher à nouveau

                if (drawnCard == null) //Si il n'y a vraiment plus de carte

                {

                    Debug.LogError("La pioche et la défausse sont vides!");

                    return null;

                }

            }

            OnCardDrawnFromDeck?.Invoke(drawnCard); //Déclenche l'évenement, la carte n'est pas retourné, donc pas de visuel

            return drawnCard; //Mais on la retourne quand même

        }

        //Fonction pour défausser une carte

        public void DiscardCard(Card card)

        {

            if (card == null) return;



            discardPile.AddCard(card); // Ajoute le modèle Card à la discardPile (modèle).

            //On utilise la fonction de création centralisé

            CardController cardController = CreateCardController(card);

            cardController.transform.position = discardPileView.discardCenter.position;

            discardPileView.PlaceCardInDiscard(cardController); // Met à jour la vue

        }

        //Fonction pour piocher une carte de la défausse

        public CardController DrawFromDiscardPile()

        {

            Card card = discardPile.DrawCard(); //On pioche la carte du model

            if (card != null)

            {

                OnCardDrawnFromDiscard?.Invoke(card); //Déclenche l'evenement

                //On utilise la méthode centralisé de création de carte

                CardController cardController = CreateCardController(card); //Création du controller

                return cardController;//Retourne le controller

            }

            return null;

        }

        //Fonction pour remélanger la défausse dans la pioche

        public void ReshuffleDiscardIntoDeck()

        {

            //Sauvegarde de la top carte

            Card topCard = discardPile.TopCard;



            //On recupere les cartes de la defausse

            List<Card> reshuffledCards = discardPile.GetCardsForReshuffle();



            discardPile.Clear();//On vide la defausse

            deck.AddCards(reshuffledCards); //On ajoute les cartes

            deck.Shuffle();//On melange

            //On remet la top carte

            if (topCard != null)

            {

                discardPile.AddCard(topCard);

            }

            //TODO : Animation du reshuffle

            Debug.Log("Deck reshuffled. Top card of discard pile remains.");



            // Mettre à jour la vue, potentiellement en créant de nouveaux CardControllers.

            // C'est ici qu'il faudrait détruire les anciens CardControllers du deck

            // et en créer de nouveaux, pour refléter le nouvel ordre du deck.

            GenerateDeckView(); //On genere la vue

        }



        //Fonction pour créer un CardController

        public CardController CreateCardController(Card card)

        {

            GameObject cardObject = Instantiate(cardPrefab); // Plus besoin de position ici

            CardController cardController = cardObject.GetComponent<CardController>();

            if (cardController == null)

            {

                cardController = cardObject.AddComponent<CardController>();

            }

            cardController.Initialize(card);

            return cardController;

        }

        //Fonction pour déplacer une carte de la pioche vers la défausse

        public void MoveCardFromDeckToDiscard()

        {

            Card cardToDiscard = deck.DrawCard(); //On pioche une carte

            if (cardToDiscard != null) //Si il y a une carte

            {

                cardToDiscard.IsFaceUp = true;  //La carte est face visible

                DiscardCard(cardToDiscard); //On la défausse

            }

        }

    }

}

