// DiscardPileClickHandler.cs
using UnityEngine;
using UnityEngine.EventSystems;
using com.hyminix.game.ojyx.Managers; // Pour accéder au GameManager
using com.hyminix.game.ojyx.States; // Pour accéder à CardSelectedState

public class DiscardPileClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Vérifie si l'état actuel est CardSelectedState
        if (GameManager.Instance.CurrentState is CardSelectedState)
        {
            // Récupère la carte sélectionnée depuis l'état
            CardSelectedState currentState = (CardSelectedState)GameManager.Instance.CurrentState;

            // Appelle la méthode DiscardSelectedCard du GameManager
            GameManager.Instance.DiscardSelectedCard(currentState.selectedCard);
        }
        else
        {
            // Si l'état n'est pas CardSelectedState, on essaie de piocher depuis la défausse
            GameManager.Instance.DrawFromDiscard();
        }
    }
}