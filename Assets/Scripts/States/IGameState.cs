// --- States/IGameState.cs --- (Ajout de GetActionForCardSlotClick)
using System.Collections.Generic;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Enums;

namespace com.hyminix.game.ojyx.States
{
    public interface IGameState
    {
        void EnterState(Managers.GameManager manager);
        void ExecuteState(Managers.GameManager manager);
        void ExitState(Managers.GameManager manager);
        List<PlayerAction> GetValidActions(Managers.GameManager manager);
        void ExecuteAction(Managers.GameManager manager, PlayerAction action, CardSlotController cardSlot = null, CardController card = null);
        // Nouvelle méthode : quelle action effectuer lors d'un clic sur un CardSlot ?
        PlayerAction? GetActionForCardSlotClick(Managers.GameManager manager);

    }
}