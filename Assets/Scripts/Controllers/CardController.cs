using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using com.hyminix.game.ojyx.Views;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.Controllers
{
    public class CardController : MonoBehaviour, IPointerClickHandler
    {
        private CardView cardView;
        private Card card;
        public Card Card => card;

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
            card.Flip();
            transform.DORotate(new Vector3(0, card.IsFaceUp ? 180 : 0, 0), 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => cardView.UpdateVisual());
        }

        // Nouvelle implémentation pour gérer le clic directement sur la carte
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"CardController: {gameObject.name} cliquée.");
            // Propagation du clic vers l'état courant du GameManager
            if (GameManager.Instance != null && GameManager.Instance.CurrentState != null)
            {
                GameManager.Instance.CurrentState.HandleCardClick(GameManager.Instance, this, eventData);
            }
            else
            {
                Debug.LogWarning("GameManager ou CurrentState non défini !");
            }
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
