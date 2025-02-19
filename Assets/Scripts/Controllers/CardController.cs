// --- Controllers/CardController.cs --- (Modifié)
using UnityEngine;
using UnityEngine.EventSystems;
using com.hyminix.game.ojyx.Views;
using com.hyminix.game.ojyx.Models;
using DG.Tweening;

namespace com.hyminix.game.ojyx.Controllers
{
    public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public delegate void CardAction(CardController cardController);
        public event CardAction OnCardClicked;
        public event CardAction OnCardDragStarted;
        public event CardAction OnCardDragEnded;

        [SerializeField] private float hoverHeight = 0.5f;
        [SerializeField] private LayerMask slotLayerMask; // Important pour le raycast
        private CardView cardView; // Référence à la vue
        private Card card;      // Référence au modèle
        public Card Card => card;     // Propriété pour accéder au modèle (lecture seule)

        private Vector3 originalPosition;
        private bool isDragging = false;
        private bool hasDragged = false; // Pour distinguer clic et drag
        private Vector3 dragStartPos;
        private const float dragThreshold = 0.1f;

        private bool draggable = false;

        public void Initialize(Card card)
        {
            this.card = card;
            cardView = GetComponent<CardView>();
            if (cardView == null)
            {
                Debug.LogError("CardView component not found on " + gameObject.name);
                return;
            }
            cardView.Initialize(card);
            SetDraggable(false); //Desactive le drag and drop
            EnsureColliderExists(); // On s'assure que le collider existe.
        }

        public void SetDraggable(bool draggable)
        {
            isDragging = false;
            hasDragged = false;
            this.draggable = draggable; // On met à jour la variable draggable.
        }


        private void EnsureColliderExists()
        {
            if (GetComponent<Collider>() == null)
            {
                gameObject.AddComponent<BoxCollider>();
                Debug.LogWarning("BoxCollider added to " + gameObject.name + " for interaction.");
            }
        }

        public void Flip()
        {
            card.Flip(); // Met à jour l'état de la carte (face visible/cachée).
            // Animation DOTween CORRECTE:
            transform.DORotate(new Vector3(0, card.IsFaceUp ? 180 : 0, 0), 0.5f) // Rotation *absolue*
                .SetEase(Ease.OutQuad)
                .OnComplete(() => cardView.UpdateVisual());
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!draggable) return; // On vérifie si le drag est autorisé.

            isDragging = true;
            hasDragged = false; // Reset le flag a chaque debut de drag
            dragStartPos = transform.position;
            originalPosition = transform.position; // Important:  Définir originalPosition ici, au début du drag.
            transform.position += Vector3.up * hoverHeight;
            OnCardDragStarted?.Invoke(this); //Déclenche l'évenement
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging || !draggable) return; // Double vérification.
            if (Camera.main == null) return;

            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                transform.position = new Vector3(hit.point.x, originalPosition.y + hoverHeight, hit.point.z);
                // Détection du seuil
                if (Vector3.Distance(dragStartPos, transform.position) > dragThreshold)
                {
                    hasDragged = true; //On met le flag a true
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isDragging || !draggable) return; // Double vérification.
            isDragging = false;
            OnCardDragEnded?.Invoke(this); //Déclenche l'évenement
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!hasDragged)
            {
                OnCardClicked?.Invoke(this); //Déclenche l'évenement
            }
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
