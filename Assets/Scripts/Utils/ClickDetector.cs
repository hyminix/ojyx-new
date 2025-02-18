// --- Utils/ClickDetector.cs ---
using UnityEngine;
using UnityEngine.EventSystems;  // N'oublie pas cet using !

public class ClickDetector : MonoBehaviour, IPointerClickHandler
{
    public System.Action OnClicked; // Simplification : Utilisation de System.Action

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(); // Déclenche l'événement.  Le '?.' est important pour éviter les NullReferenceException.
    }
}