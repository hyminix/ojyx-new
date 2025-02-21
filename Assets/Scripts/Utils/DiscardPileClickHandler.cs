// --- Utils/DiscardPileClickHandler.cs ---
using UnityEngine;
using UnityEngine.EventSystems;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.States;
using com.hyminix.game.ojyx.Enums;

public class DiscardPileClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentState is CardSelectedState)
        {
            GameManager.Instance.ExecuteAction(GameManager.Instance.CurrentState, PlayerAction.DiscardDrawnCard);
        }
        else
        {
            GameManager.Instance.ExecuteAction(GameManager.Instance.CurrentState, PlayerAction.DrawFromDiscard);
        }
    }
}
