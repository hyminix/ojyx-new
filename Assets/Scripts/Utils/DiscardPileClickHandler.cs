// DiscardPileClickHandler.cs (Appel correct de ExecuteAction en fonction de l'état)
using UnityEngine;
using UnityEngine.EventSystems;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.States;
using com.hyminix.game.ojyx.Enums;

public class DiscardPileClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Vérifie si l'état actuel est CardSelectedState
        if (GameManager.Instance.CurrentState is CardSelectedState)
        {
            // Si oui, on défausse la carte sélectionnée.
            GameManager.Instance.ExecuteAction(GameManager.Instance.CurrentState, PlayerAction.DiscardDrawnCard);
        }
        else
        {
            // Sinon, on pioche depuis la défausse (comportement normal).
            GameManager.Instance.ExecuteAction(GameManager.Instance.CurrentState, PlayerAction.DrawFromDiscard);
        }
    }
}