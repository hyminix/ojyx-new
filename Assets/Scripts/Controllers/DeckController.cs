using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Views;
using com.hyminix.game.ojyx.Data;
using DG.Tweening;

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

        [SerializeField] public DiscardPileView discardPileView; // Vue de la défausse

        public delegate void CardDrawAction(Card card);
        public static event CardDrawAction OnCardDrawnFromDeck;
        public static event CardDrawAction OnCardDrawnFromDiscard;

        [ShowInInspector, ReadOnly]
        public List<CardController> deckCards = new List<CardController>(); // Garde la liste publique.

        private CardController currentTopCardController; // Carte du dessus de la pioche (vue)

        private void Start()
        {
            // Initialise le modèle de la défausse
            discardPile = new DiscardPile();

            // IMPORTANT : On assigne le modèle de la défausse à la vue
            if (discardPileView != null)
            {
                discardPileView.discardPileModel = discardPile;
            }
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
                ShowTopDeckCard(); // Affiche la carte du dessus
            }
        }

        // Pioche une carte depuis la pioche (deck)
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

            CardController drawnCardController = CreateCardController(drawnCard);
            OnCardDrawnFromDeck?.Invoke(drawnCard);

            // Met à jour la pioche visuelle
            ShowNextCard();

            return drawnCardController;
        }

        // Défausse une carte directement (sans animation)
        public void DiscardCard(Card card)
        {
            if (card == null) return;
            discardPile.AddCard(card);

            // Crée un nouveau CardController pour la vue
            CardController cardController = CreateCardController(card);
            // Place visuellement dans la défausse
            discardPileView.AddCardToDiscardPile(cardController);
        }

        // Défausse avec animation
        public void DiscardCardWithAnimation(CardController cardController, float duration, Ease ease)
        {
            if (cardController == null) return;

            Debug.Log("Mouvement de la carte (DiscardCardWithAnimation)");
            cardController.Flip(); // On la flip pour la mettre face visible

            // Animation de déplacement vers le centre de la défausse
            cardController.transform.DOMove(discardPileView.transform.position, duration)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    // On l'ajoute au modèle
                    discardPile.AddCard(cardController.Card);
                    // On l'ajoute à la vue de la défausse
                    discardPileView.AddCardToDiscardPile(cardController);
                });
        }

        // Pioche la carte du dessus de la défausse
        public CardController DrawFromDiscardPile()
        {
            // On récupère le topCardController dans la vue
            CardController topCardController = discardPileView.DrawTopCardController();
            if (topCardController == null)
            {
                Debug.LogWarning("Impossible de piocher depuis la défausse (vide).");
                return null;
            }

            // topCardController.Card a déjà été mis à jour dans la vue
            OnCardDrawnFromDiscard?.Invoke(topCardController.Card);
            return topCardController;
        }

        // Remélange la défausse dans la pioche
        public void ReshuffleDiscardIntoDeck()
        {
            Card topCard = discardPile.TopCard;
            List<Card> reshuffledCards = discardPile.GetCardsForReshuffle();
            discardPile.Clear();
            discardPileView.Clear();

            deck.AddCards(reshuffledCards);
            deck.Shuffle();

            // On détruit l'ancienne carte visible du deck
            if (currentTopCardController != null)
            {
                Destroy(currentTopCardController.gameObject);
                currentTopCardController = null;
            }

            deckCards.Clear();

            // Affiche la nouvelle carte du dessus
            if (deck.cards.Count > 0)
            {
                ShowTopDeckCard();
            }

            // On remet la topCard dans la défausse
            if (topCard != null)
            {
                discardPile.AddCard(topCard);
                CardController cardController = CreateCardController(topCard);
                discardPileView.AddCardToDiscardPile(cardController);
            }

            Debug.Log("Deck reshuffled. Top card of discard pile remains.");
        }

        // Instancie un CardController pour un Card donné
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

        // Déplace la première carte de la pioche à la défausse (état DiscardFirstCardState)
        public void MoveCardFromDeckToDiscard()
        {
            CardController cardToDiscard = DrawFromDeck();
            if (cardToDiscard != null)
            {
                DiscardCardWithAnimation(cardToDiscard, 0.5f, Ease.OutQuad);
            }
        }

        // Affiche la carte du dessus de la pioche (visuellement)
        private void ShowTopDeckCard()
        {
            if (deck.cards.Count > 0 && currentTopCardController == null)
            {
                Card topCard = deck.cards[0];
                currentTopCardController = CreateCardController(topCard);
                currentTopCardController.transform.position = deckPosition.position;
                currentTopCardController.transform.SetParent(deckPosition);
                deckCards.Add(currentTopCardController);
            }
        }

        // Affiche la carte suivante (2e carte) de la pioche
        public void ShowNextCard()
        {
            if (deckCards.Count > 1)
            {
                Destroy(deckCards[1].gameObject);
                deckCards.RemoveAt(1);
            }

            if (deck.cards.Count > 1)
            {
                Card nextCard = deck.cards[1];
                CardController nextCardController = CreateCardController(nextCard);
                float zOffset = 0.01f;
                nextCardController.transform.position = deckPosition.position + new Vector3(0, 0, -zOffset);
                nextCardController.transform.SetParent(deckPosition);
                nextCardController.gameObject.SetActive(false);
                deckCards.Insert(1, nextCardController);
            }
        }

        // Masque la 2e carte
        public void HideNextCard()
        {
            if (deckCards.Count > 1)
            {
                Destroy(deckCards[1].gameObject);
                deckCards.RemoveAt(1);
            }
        }
    }
}
