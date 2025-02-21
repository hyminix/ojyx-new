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
        public GameObject cardPrefab; // Public pour l'accès
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
        public DeckView DeckView
        {
            get { return deckView; }
        }
        private void Start()
        {
            // Crée le modèle de défausse
            discardPile = new DiscardPile();
            if (discardPileView != null)
                discardPileView.discardPileModel = discardPile;
        }

        [Button("Initialiser le Deck")]
        public void InitializeDeck()
        {
            // Crée le modèle Deck
            deck = new Deck(cardTemplates);

            // Récupère le DeckView attaché à ce GameObject
            deckView = GetComponent<DeckView>();
            if (deckView == null)
            {
                Debug.LogError("Pas de DeckView trouvé sur " + gameObject.name);
                return;
            }
            deckView.deckModel = deck;

            // Initialise l'affichage complet du deck
            deckView.ClearDeckView();
            deckView.RefreshDeckView();

            // Vide également la défausse
            discardPile.Clear();
            discardPileView?.Clear();
        }

        /// <summary>
        /// Tire la carte du dessus du deck.
        /// </summary>
        public CardController DrawFromDeck()
        {
            CardController topCardController = deckView.RemoveTopCardController();
            if (topCardController == null)
            {
                Debug.LogWarning("Deck vide !");
                return null;
            }
            deckView.UpdateDeckPositions();
            OnCardDrawnFromDeck?.Invoke(topCardController.Card);
            return topCardController;
        }

        /// <summary>
        /// Tire la carte du dessus de la défausse.
        /// </summary>
        public CardController DrawFromDiscardPile()
        {
            CardController topCardController = discardPileView.DrawTopCardController();
            if (topCardController == null)
            {
                Debug.LogWarning("Impossible de piocher depuis la défausse (vide).");
                return null;
            }
            OnCardDrawnFromDiscard?.Invoke(topCardController.Card);
            return topCardController;
        }

        /// <summary>
        /// Défausse une carte directement (sans animation).
        /// </summary>
        public void DiscardCard(Card card)
        {
            if (card == null) return;
            discardPile.AddCard(card);
            CardController cardController = CreateCardController(card);
            discardPileView?.AddCardToDiscardPile(cardController);
        }

        /// <summary>
        /// Défausse une carte avec animation.
        /// </summary>
        public void DiscardCardWithAnimation(CardController cardController, float duration, Ease ease)
        {
            if (cardController == null) return;
            Debug.Log("Mouvement de la carte (DiscardCardWithAnimation)");
            cardController.Flip();
            cardController.transform.DOMove(discardPileView.transform.position, duration)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    discardPile.AddCard(cardController.Card);
                    discardPileView?.AddCardToDiscardPile(cardController);
                });
        }

        /// <summary>
        /// Déplace la première carte du deck vers la défausse.
        /// </summary>
        public void MoveCardFromDeckToDiscard()
        {
            CardController cardToDiscard = DrawFromDeck();
            if (cardToDiscard != null)
            {
                DiscardCardWithAnimation(cardToDiscard, 0.5f, Ease.OutQuad);
            }
        }

        /// <summary>
        /// Crée un CardController pour une carte donnée.
        /// </summary>
        private CardController CreateCardController(Card card)
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
    }
}
