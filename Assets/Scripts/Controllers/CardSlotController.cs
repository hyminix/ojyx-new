// --- Controllers/CardSlotController.cs --- (Simplification: plus besoin de s'abonner au ClickHandler)
using UnityEngine;
using DG.Tweening;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Views;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.Controllers
{
    [RequireComponent(typeof(CardSlotView))]
    public class CardSlotController : MonoBehaviour
    {
        public CardSlot cardSlot;
        private CardSlotView cardSlotView;

        private CardController currentCardController;
        public CardController CardController => currentCardController;

        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private Ease animationEase = Ease.OutQuad;

        public void Initialize(CardSlot slot)
        {
            cardSlot = slot;
            cardSlotView = GetComponent<CardSlotView>();
            if (cardSlotView == null)
            {
                Debug.LogError("CardSlotView non trouvé sur " + gameObject.name);
                return;
            }
            cardSlotView.Initialize(cardSlot);

            // SIMPLIFICATION : On ajoute le ClickHandler, c'est tout.
            GetComponent<Collider>().gameObject.AddComponent<ClickHandler>();
        }

        public void PlaceCard(CardController newCardController)
        {
            Debug.Log($"PlaceCard called on slot: {cardSlot.row}, {cardSlot.column}, occupied: {cardSlot.IsOccupied}");

            // Détacher la nouvelle carte de son parent actuel (si elle en a un).
            if (newCardController.transform.parent != null)
            {
                newCardController.transform.SetParent(null);
            }

            // S'il y a déjà une carte, la défausser *AVANT* de mettre à jour le modèle.
            if (cardSlot.IsOccupied && currentCardController != null)
            {
                GameManager.Instance.DeckController.DiscardCardWithAnimation(currentCardController, animationDuration, animationEase);
            }

            // Mettre à jour le modèle *APRÈS* avoir géré l'ancienne carte.
            cardSlot.PlaceCard(newCardController.Card);
            newCardController.Card.SetPosition(cardSlot.row, cardSlot.column); // Mettre à jour la position dans le modèle.

            // Mettre à jour la vue.
            currentCardController = newCardController;
            newCardController.transform.SetParent(transform);
            newCardController.transform.DOLocalMove(new Vector3(0, 0.1f, 0), animationDuration).SetEase(animationEase);
            cardSlotView.UpdateVisual();
        }


        public CardController RemoveCard()
        {
            if (!cardSlot.IsOccupied)
            {
                return null;
            }

            CardController removed = currentCardController;
            if (removed != null)
            {
                // Détacher la carte *AVANT* de mettre à jour le modèle.
                removed.transform.SetParent(null);
                cardSlot.RemoveCard(); // Mettre à jour le modèle.
                cardSlotView.UpdateVisual();
                currentCardController = null;
                return removed;
            }
            return null;
        }

        public void SetHighlight(bool active)
        {
            cardSlotView.SetHighlight(active);
        }
    }
}