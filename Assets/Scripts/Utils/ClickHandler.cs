// ClickHandler.cs (sur les CardSlots, interrogation de l'état courant)
using UnityEngine;
using UnityEngine.EventSystems;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Enums;
public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    private CardSlotController cardSlotController;
    private void Awake()
    {
        cardSlotController = GetComponentInParent<CardSlotController>();
        if (cardSlotController == null)
        {
            Debug.LogError("ClickHandler: CardSlotController not found!");
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (cardSlotController != null)
        {
            // INTERROGE L'ÉTAT COURANT pour connaître l'action à effectuer.
            PlayerAction? action = GameManager.Instance.CurrentState.GetActionForCardSlotClick(GameManager.Instance);
            if (action.HasValue) // Important : vérifier si l'action est non-nulle.
            {
                GameManager.Instance.ExecuteAction(GameManager.Instance.CurrentState, action.Value, cardSlotController);
            }
        }
    }
}