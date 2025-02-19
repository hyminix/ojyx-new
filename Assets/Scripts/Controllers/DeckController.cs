// --- Controllers/DeckController.cs --- (CORRIGÉ : DrawFromDeck crée toujours un nouveau CardController)

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
        [SerializeField] private List<CardData> cardTemplates;

        [Title("Prefabs & Positions")]
        public GameObject cardPrefab;
        public Transform deckPosition;

        [Title("Composants")]
        [SerializeField, ReadOnly] private Deck deck;
        [SerializeField, ReadOnly] private DeckView deckView;
        [SerializeField, ReadOnly] private DiscardPile discardPile;
        public DiscardPile DiscardPile => discardPile;
        [SerializeField] public DiscardPileView discardPileView;

        public delegate void CardDrawAction(Card card);
        public static event CardDrawAction OnCardDrawnFromDeck;
        public static event CardDrawAction OnCardDrawnFromDiscard;

        [ShowInInspector, ReadOnly]
        public List<CardController> deckCards = new List<CardController>(); // Garde la liste publique.

        // Référence à la carte du dessus du deck.
        private CardController currentTopCardController;


        private void Start()
        {
            discardPile = new DiscardPile();
        }


        [Button("Initialiser le Deck")]
        public void InitializeDeck()
        {
            deck = new Deck(cardTemplates);
            deckView = GetComponent<DeckView>();
            if (deckView == null)
            {
                Debug.LogError("Pas de DeckView trouvé !");
                return;
            }

            discardPile.Clear();
            discardPileView.Clear();

            if (deck.cards.Count > 0)
            {
                ShowTopDeckCard(); // Affiche la carte du dessus.
            }
        }

        // --- DrawFromDeck : CORRIGÉ pour créer un NOUVEAU CardController ---
        public CardController DrawFromDeck()
        {
            Card drawnCard = deck.DrawCard();
            if (drawnCard == null)
            {
                ReshuffleDiscardIntoDeck();
                drawnCard = deck.DrawCard();
                if (drawnCard == null)
                {
                    Debug.LogError("La pioche et la défausse sont vides!");
                    return null;
                }
            }

            // Crée *toujours* un nouveau CardController.
            CardController drawnCardController = CreateCardController(drawnCard);


            OnCardDrawnFromDeck?.Invoke(drawnCard);

            // Affiche la carte suivante (si elle existe).
            ShowNextCard();  // Met à jour l'affichage de la pioche.

            return drawnCardController; // Retourne le *nouveau* CardController.
        }
        // --- Fin de DrawFromDeck ---


        public void DiscardCard(Card card)
        {
            if (card == null) return;

            discardPile.AddCard(card);
            CardController cardController = CreateCardController(card);
            cardController.transform.position = discardPileView.discardCenter.position;
            discardPileView.PlaceCardInDiscard(cardController);
        }

        public CardController DrawFromDiscardPile()
        {
            Card card = discardPile.DrawCard();
            if (card == null)
            {
                Debug.Log("La défausse est vide");
                return null;
            }

            // TROUVER le CardController existant.
            CardController cardController = discardPileView.discardSlot.GetComponentInChildren<CardController>();
            if (cardController == null)
            {
                Debug.LogError("DrawFromDiscardPile: CardController not found in discard pile!"); // Cela ne devrait *JAMAIS* arriver.
                return null; // Ou, potentiellement, créer un nouveau CardController ici, mais c'est un signe de problème ailleurs.
            }

            discardPileView.discardSlot.RemoveCard(); //Supprime la carte du model
            cardController.Initialize(card); // Réinitialise avec la nouvelle carte (important si le CardController est réutilisé)
            OnCardDrawnFromDiscard?.Invoke(card); // Gardez cet événement, il peut être utile.
            return cardController; // Retourne le CardController *existant*.
        }


        public void ReshuffleDiscardIntoDeck()
        {
            Card topCard = discardPile.TopCard;
            List<Card> reshuffledCards = discardPile.GetCardsForReshuffle();
            discardPile.Clear();
            discardPileView.Clear();
            deck.AddCards(reshuffledCards);
            deck.Shuffle();


            // On détruit l'ancienne carte visible du deck.
            if (currentTopCardController != null)
            {
                Destroy(currentTopCardController.gameObject);
                currentTopCardController = null; // Important de réinitialiser.
            }

            // Mise à jour importante : Vider la liste deckCards *AVANT* de la reconstruire.
            deckCards.Clear();

            // Affiche la nouvelle carte du dessus.
            if (deck.cards.Count > 0)
            {
                ShowTopDeckCard(); // Réutilise la méthode.
            }


            if (topCard != null)
            {
                discardPile.AddCard(topCard);
                CardController cardController = CreateCardController(topCard);
                cardController.transform.position = discardPileView.discardCenter.position;
                discardPileView.PlaceCardInDiscard(cardController);
            }

            Debug.Log("Deck reshuffled. Top card of discard pile remains.");
        }


        public CardController CreateCardController(Card card)
        {
            GameObject cardObject = Instantiate(cardPrefab);
            CardController cardController = cardObject.GetComponent<CardController>();
            if (cardController == null)
            {
                cardController = cardObject.AddComponent<CardController>();
            }
            cardController.Initialize(card);
            return cardController;
        }

        public void MoveCardFromDeckToDiscard()
        {
            Card cardToDiscard = deck.DrawCard();
            if (cardToDiscard != null)
            {
                cardToDiscard.IsFaceUp = true;
                DiscardCard(cardToDiscard);
            }
        }

        // MODIFIÉ: ShowTopDeckCard crée la carte *une seule fois*.
        private void ShowTopDeckCard()
        {
            if (deck.cards.Count > 0 && currentTopCardController == null) // Vérifie si la carte n'existe pas déjà
            {
                Card topCard = deck.cards[0]; // Prend la première carte du *modèle*.
                currentTopCardController = CreateCardController(topCard);
                currentTopCardController.transform.position = deckPosition.position;
                currentTopCardController.transform.SetParent(deckPosition);
                currentTopCardController.SetDraggable(false); // Par défaut, non-draggable.
                deckCards.Add(currentTopCardController); // Ajoute à la liste (même si on n'en affiche qu'une).
            }
        }

        // MODIFIÉ: ShowNextCard ne crée plus de doublons, et gère le cas où il n'y a plus de carte suivante.
        public void ShowNextCard()
        {
            // D'abord, on supprime l'ancienne "prochaine carte" (s'il y en avait une).
            if (deckCards.Count > 1)
            {
                Destroy(deckCards[1].gameObject);
                deckCards.RemoveAt(1);
            }

            // Ensuite, on affiche la nouvelle prochaine carte (s'il y en a une).
            if (deck.cards.Count > 1)
            {
                Card nextCard = deck.cards[1]; // La *deuxième* carte du modèle.
                CardController nextCardController = CreateCardController(nextCard);

                // Positionne la carte *légèrement* en dessous de la carte du dessus.
                float zOffset = 0.01f; // Reprend l'offset.  Ajuste au besoin.
                nextCardController.transform.position = deckPosition.position + new Vector3(0, 0, -zOffset);
                nextCardController.transform.SetParent(deckPosition);
                nextCardController.SetDraggable(false); // La carte suivante n'est pas draggable.
                nextCardController.gameObject.SetActive(false); //On la desactive
                deckCards.Insert(1, nextCardController); // L'insère à l'index 1, pour un ordre correct.
            }
        }

        // NOUVELLE MÉTHODE : Masque la carte suivante (après le drag).
        public void HideNextCard()
        {
            if (deckCards.Count > 1)
            {
                Destroy(deckCards[1].gameObject); // Détruit la carte suivante (visuellement).
                deckCards.RemoveAt(1);          // La supprime de la liste.
            }
        }
    }
}

