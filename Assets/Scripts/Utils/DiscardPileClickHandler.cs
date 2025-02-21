// DiscardPileClickHandler.cs
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using com.hyminix.game.ojyx.Managers; // Pour accéder au GameManager

public class DiscardPileClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.DrawFromDiscard(); // Appelle directement la méthode du GameManager
    }
}