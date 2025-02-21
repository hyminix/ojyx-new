// --- Controllers/CardSlotController.cs ---
using UnityEngine;
//SUPPRIMER UnityEngine.EventSystems;
using DG.Tweening;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Views;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.Controllers
{
    [RequireComponent(typeof(CardSlotView))]
    public class CardSlotController : MonoBehaviour //SUPPRIMER , IPointerClickHandler
    {
        //SUPPRIMER  public delegate void CardSlotAction(CardSlotController slotController);
        //SUPPRIMER  public event CardSlotAction OnCardPlaced;
        //SUPPRIMER  public event CardSlotAction OnCardRemoved;

        //SUPPRIMER  public delegate void CardSlotClickedAction(CardController cardController);
        //SUPPRIMER  public event CardSlotClickedAction OnCardClicked;

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

            // Ajout: On déclenche l'événement GameEvents.OnCardSlotClicked quand le slot est cliqué.
            GetComponent<Collider>().gameObject.AddComponent<ClickHandler>().OnClick += () =>
            {
                GameEvents.TriggerCardSlotClicked(this);
            };
        }

        public void PlaceCard(CardController newCardController)
        {
            Debug.Log($"PlaceCard called on slot: {cardSlot.row}, {cardSlot.column}, occupied: {cardSlot.IsOccupied}");

            if (newCardController.transform.parent != null && newCardController.transform.parent != transform)
            {
                Debug.Log("Détachement du nouveau CardController de son ancien parent.");
                newCardController.transform.SetParent(null);
            }

            if (cardSlot.IsOccupied)
            {
                Debug.Log("PlaceCard: Slot is occupied, swapping cards.");
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
                            //SUPPRIMER OnCardPlaced?.Invoke(this);
                        });
                    newCardController.Card.SetPosition(cardSlot.row, cardSlot.column);
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
                newCardController.transform.SetParent(transform);
                newCardController.transform.DOLocalMove(new Vector3(0, 0.1f, 0), animationDuration)
                    .SetEase(animationEase);

                cardSlot.PlaceCard(newCardController.Card);
                newCardController.Card.SetPosition(cardSlot.row, cardSlot.column);
                cardSlotView.UpdateVisual();
                //SUPPRIMER OnCardPlaced?.Invoke(this);
                currentCardController = newCardController;
            }
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
                removed.transform.SetParent(null);
                cardSlot.RemoveCard();
                cardSlotView.UpdateVisual();
                //SUPPRIMER OnCardRemoved?.Invoke(this);
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