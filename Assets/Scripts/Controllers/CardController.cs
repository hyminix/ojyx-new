// --- Controllers/CardController.cs ---

using UnityEngine;

using UnityEngine.EventSystems;

using com.hyminix.game.ojyx.Views;

using com.hyminix.game.ojyx.Models;

using com.hyminix.game.ojyx.Managers;

using MoreMountains.Feedbacks; // Ajout du namespace Managers

using DG.Tweening; // Assure-toi que cet using est présent



namespace com.hyminix.game.ojyx.Controllers

{

    public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler

    {

        public delegate void CardAction(CardController cardController);

        public event CardAction OnCardClicked;

        public event CardAction OnCardDragStarted;

        public event CardAction OnCardDragEnded;



        [SerializeField] private float hoverHeight = 0.5f;

        [SerializeField] private LayerMask slotLayerMask; // Ajout du LayerMask

        private CardView cardView;

        private Card card;

        public Card Card => card;



        private Vector3 originalPosition;

        private bool isDragging = false;

        // Pour différencier clic et drag

        private bool hasDragged = false;

        private Vector3 dragStartPos;

        private const float dragThreshold = 0.1f;



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

        }



        private void Start()

        {

            originalPosition = transform.position;

            EnsureColliderExists();

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

            isDragging = true;

            hasDragged = false;

            dragStartPos = transform.position;

            transform.position += Vector3.up * hoverHeight;

            OnCardDragStarted?.Invoke(this);

        }



        public void OnDrag(PointerEventData eventData)

        {

            if (!isDragging) return;

            if (Camera.main == null) return;



            Ray ray = Camera.main.ScreenPointToRay(eventData.position);

            if (Physics.Raycast(ray, out RaycastHit hit))

            {

                transform.position = new Vector3(hit.point.x, originalPosition.y + hoverHeight, hit.point.z);



                if (Vector3.Distance(dragStartPos, transform.position) > dragThreshold)

                {

                    hasDragged = true;

                }

            }

        }



        public void OnEndDrag(PointerEventData eventData)

        {

            if (!isDragging) return;

            isDragging = false;

            OnCardDragEnded?.Invoke(this);



            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Utilisation correcte du layerMask *AVANT* les autres vérifications.

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, slotLayerMask))

            {

                CardSlotController slot = hit.collider.GetComponent<CardSlotController>();

                if (slot != null)

                {

                    // La logique de placement est gérée par l'état, pas par la carte elle-même.

                    // On ne fait *rien* ici directement.  L'événement OnCardDragEnded

                    // sera géré par PlayerTurnState, qui appellera GameManager.OnCardPlaced.

                    return;

                }

            }



            // Vérification de la défausse *APRÈS* avoir vérifié les slots.

            if (hit.collider != null && hit.collider.GetComponent<DiscardPileView>() != null)

            {

                GameManager.Instance.CurrentPlayer.DiscardCard(this); // Utilisation de GameManager.Instance

                return;

            }

        }





        public void OnPointerClick(PointerEventData eventData)

        {

            if (!hasDragged)

            {

                OnCardClicked?.Invoke(this);

            }

        }

    }

}
