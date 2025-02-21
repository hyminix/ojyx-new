// ClickHandler.cs
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public UnityAction OnClick; // Utilisation de UnityAction pour la flexibilité

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}