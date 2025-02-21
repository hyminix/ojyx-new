// --- Controllers/DeckController.cs --- (Léger ajustement dans DiscardCardWithAnimation)
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
        public Transform deckPosition; // Gardé pour l'instant, mais pourrait être supprimé si DeckView gère le DrawFromDeck
        [Title("Composants")]
        [SerializeField, ReadOnly] private Deck deck;
        [SerializeField, ReadOnly] private DeckView deckView;
        [SerializeField, ReadOnly] private DiscardPile discardPile;
        public DiscardPile DiscardPile => discardPile;
        [SerializeField] public DiscardPileView discardPileView;
        public delegate void CardDrawAction(Card card);
        public static event CardDrawAction OnCardDrawnFromDeck;
        public static event CardDrawAction OnCardDrawnFromDiscard; // Pas utilisé pour l'instant, mais peut être utile
        public DeckView DeckView
        {
            get { return deckView; }
        }
        private void Start()
        {
            discardPile = new DiscardPile();
            if (discardPileView != null)
                discardPileView.discardPileModel = discardPile;
        }
        [Button("Initialiser le Deck")]
        public void InitializeDeck()
        {
            deck = new Deck(cardTemplates);
            deckView = GetComponent<DeckView>();
            if (deckView == null)
            {
                Debug.LogError("Pas de DeckView trouvé sur " + gameObject.name);
                return;
            }
            deckView.deckModel = deck;
            deckView.ClearDeckView();
            deckView.RefreshDeckView();
            discardPile.Clear();
            discardPileView?.Clear(); // S'assure que la vue est aussi vidée.
        }
        public CardController DrawFromDeck()
        {
            CardController topCardController = deckView.RemoveTopCardController(); // Retire la carte de la *vue*
            if (topCardController == null)
            {
                Debug.LogWarning("Deck vide !");
                return null;
            }
            // Pas besoin d'appeler deckModel.DrawCard() ici, c'est déjà fait dans RemoveTopCardController.
            OnCardDrawnFromDeck?.Invoke(topCardController.Card);
            return topCardController;
        }
        public CardController DrawFromDiscardPile()
        {
            CardController topCardController = discardPileView.DrawTopCardController(); // Retire la carte de la *vue*
            if (topCardController == null)
            {
                Debug.LogWarning("Impossible de piocher depuis la défausse (vide).");
                return null;
            }
            // Pas besoin d'appeler discardPile.DrawCard() ici, c'est déjà fait dans DrawTopCardController.
            OnCardDrawnFromDiscard?.Invoke(topCardController.Card);
            return topCardController;
        }
        public void DiscardCard(Card card)
        {
            if (card == null) return;
            card.IsFaceUp = true; // <- Assure que la carte est face visible.
            discardPile.AddCard(card);
            CardController cardController = CreateCardController(card);
            discardPileView?.AddCardToDiscardPile(cardController);
        }
        public void DiscardCardWithAnimation(CardController cardController, float duration, Ease ease)
        {
            if (cardController == null) return;
            Debug.Log("Mouvement de la carte (DiscardCardWithAnimation)");
            // Ajout : Retourner la carte si elle est face cachée.
            if (!cardController.Card.IsFaceUp)
            {
                cardController.Flip();
            }
            // MODIFICATION : OnComplete *avant* le DoMove, pour éviter la course critique.
            cardController.transform.DOMove(discardPileView.transform.position, duration)
              .SetEase(ease)
              .OnComplete(() =>
              {
                  discardPileView.AddCardToDiscardPile(cardController); // Ajoute à la *vue* et au modèle.
              });
        }
        public void MoveCardFromDeckToDiscard()
        {
            CardController cardToDiscard = DrawFromDeck(); // Réutilise DrawFromDeck, qui gère déjà le retrait de la vue.
            if (cardToDiscard != null)
            {
                cardToDiscard.Flip(); // Flip ici, avant la défausse.
                DiscardCardWithAnimation(cardToDiscard, 0.5f, Ease.OutQuad);
            }
        }
        private CardController CreateCardController(Card card)
        {
            GameObject cardObj = Instantiate(cardPrefab);
            CardController cardController = cardObj.GetComponent<CardController>();
            if (cardController == null)
            {
                cardController = cardObj.AddComponent<CardController>();
            }
            cardController.Initialize(card);
            return cardController;
        }
    }
}
