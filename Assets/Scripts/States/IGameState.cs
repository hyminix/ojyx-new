// --- States/IGameState.cs ---

//Ajout des interfaces pour les actions

namespace com.hyminix.game.ojyx.States

{

    public interface IGameState

    {

        void EnterState(Managers.GameManager manager);

        void ExecuteState(Managers.GameManager manager);

        void ExitState(Managers.GameManager manager);

    }

    public interface ICardClickHandler

    {

        void HandleCardClick(Managers.GameManager manager, Controllers.CardController cardController);

    }

    public interface ICardDragStartHandler

    {

        void HandleCardDragStart(Managers.GameManager manager, Controllers.CardController cardController);

    }

    public interface ICardDragEndHandler

    {

        void HandleCardDragEnd(Managers.GameManager manager, Controllers.CardController cardController);

    }

    public interface ICardPlacementHandler

    {

        void HandleCardPlacement(Managers.GameManager manager, Controllers.CardSlotController cardSlotController);

    }

    public interface ICardRemoveHandler

    {

        void HandleCardRemove(Managers.GameManager manager, Controllers.CardSlotController cardSlotController);

    }

}
