// DeckClickHandler.cs (Utilisation de ExecuteAction)

using UnityEngine;

using UnityEngine.Events;

using UnityEngine.EventSystems;

using com.hyminix.game.ojyx.Managers;

using com.hyminix.game.ojyx.Enums; // Importer l'enum





public class DeckClickHandler : MonoBehaviour, IPointerClickHandler

{

    public void OnPointerClick(PointerEventData eventData)

    {

        // Appelle ExecuteAction avec l'action DrawFromDeck.

        GameManager.Instance.ExecuteAction(GameManager.Instance.CurrentState, PlayerAction.DrawFromDeck);

    }

}
