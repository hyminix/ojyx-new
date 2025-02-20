using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Views;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.Controllers
{
    [RequireComponent(typeof(CardSlotView))]
    public class CardSlotController : MonoBehaviour, IPointerClickHandler
    {
        public delegate void CardSlotAction(CardSlotController slotController);
        public event CardSlotAction OnCardPlaced;
        public event CardSlotAction OnCardRemoved;

        public delegate void CardSlotClickedAction(CardController cardController);
        public event CardSlotClickedAction OnCardClicked;

        public CardSlot cardSlot;
        private CardSlotView cardSlotView;
        // Nouveau champ pour stocker la carte visuelle présente dans ce slot
        private CardController currentCardController;
        public CardController CardController => currentCardController;


        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private Ease animationEase = Ease.OutQuad;

        // Initialise le slot en lui passant l'instance existante du modèle
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
        }

        public void PlaceCard(CardController newCardController)
        {
            Debug.Log($"PlaceCard called on slot: {cardSlot.row}, {cardSlot.column}, occupied: {cardSlot.IsOccupied}");

            // Assurer que le nouveau CardController est détaché de tout parent indésirable (par ex. de la défausse)
            if (newCardController.transform.parent != null && newCardController.transform.parent != transform)
            {
                Debug.Log("Détachement du nouveau CardController de son ancien parent.");
                newCardController.transform.SetParent(null);
            }

            if (cardSlot.IsOccupied)
            {
                Debug.Log("PlaceCard: Slot is occupied, swapping cards.");
                // Récupération de l'ancienne carte déjà placée
                CardController oldCardController = currentCardController;
                Debug.Log($"PlaceCard: Old CardController present: {oldCardController != null}, New: {newCardController != null}");
                if (oldCardController != null && newCardController != null)
                {
                    // Déplacer l'ancienne carte vers la défausse
                    GameManager.Instance.DeckController.DiscardCardWithAnimation(oldCardController, animationDuration, animationEase);

                    // Mise à jour du modèle : retirer l'ancienne carte et placer la nouvelle
                    cardSlot.RemoveCard();
                    cardSlot.PlaceCard(newCardController.Card);
                    cardSlotView.UpdateVisual();

                    // Placer la nouvelle carte dans ce slot en tant qu'enfant
                    newCardController.transform.SetParent(transform);
                    newCardController.transform.DOLocalMove(new Vector3(0, 0.1f, 0), animationDuration)
                        .SetEase(animationEase)
                        .OnComplete(() =>
                        {
                            Debug.Log($"PlaceCard: New card {newCardController.Card.Data.value} placed in slot ({cardSlot.row}, {cardSlot.column}).");
                            OnCardPlaced?.Invoke(this);
                        });
                    newCardController.Card.SetPosition(cardSlot.row, cardSlot.column);

                    // Stocker la référence à la nouvelle carte
                    currentCardController = newCardController;
                }
                else
                {
                    Debug.LogError("PlaceCard: oldCardController or newCardController is null!");
                }
            }
            else
            {
                Debug.Log("CardSlotController.PlaceCard: Slot vide, placement simple.");
                // Placement simple
                newCardController.transform.SetParent(transform);
                newCardController.transform.DOLocalMove(new Vector3(0, 0.1f, 0), animationDuration)
                    .SetEase(animationEase);
                cardSlot.PlaceCard(newCardController.Card);
                newCardController.Card.SetPosition(cardSlot.row, cardSlot.column);
                cardSlotView.UpdateVisual();
                OnCardPlaced?.Invoke(this);
                currentCardController = newCardController;
            }
        }


        public CardController RemoveCard()
        {
            if (!cardSlot.IsOccupied)
            {
                return null;
            }
            // Utiliser la référence stockée
            CardController removed = currentCardController;
            if (removed != null)
            {
                removed.transform.SetParent(null);
                cardSlot.RemoveCard();
                cardSlotView.UpdateVisual();
                OnCardRemoved?.Invoke(this);
                currentCardController = null;
                return removed;
            }
            return null;
        }

        public void SetHighlight(bool active)
        {
            cardSlotView.SetHighlight(active);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"CardSlotController.OnPointerClick: Slot ({cardSlot.row}, {cardSlot.column}) cliqué.");
            GameEvents.TriggerCardSlotClicked(this, eventData);
        }
    }
}
