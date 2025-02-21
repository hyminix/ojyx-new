// DeckClickHandler.cs
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using com.hyminix.game.ojyx.Managers;  // Pour accéder au GameManager

public class DeckClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.DrawFromDeck(); // Appelle directement la méthode du GameManager
    }
}