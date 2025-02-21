// --- States/IGameState.cs ---
//Ajout des interfaces pour les actions
//SUPPRIMER using UnityEngine.EventSystems;

namespace com.hyminix.game.ojyx.States
{
    public interface IGameState
    {
        void EnterState(Managers.GameManager manager);
        void ExecuteState(Managers.GameManager manager);
        void ExitState(Managers.GameManager manager);
        void HandleCardClick(Managers.GameManager manager, Controllers.CardSlotController cardSlotController); // MODIFIÉ : CardSlotController au lieu de CardController
    }
}