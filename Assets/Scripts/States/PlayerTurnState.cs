// --- States/PlayerTurnState.cs ---

using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Models;

using com.hyminix.game.ojyx.Managers;

using com.hyminix.game.ojyx.Views;



namespace com.hyminix.game.ojyx.States

{

    //Implémentation des interfaces

    public class PlayerTurnState : IGameState

    {

        //Supprime le constructeur

        public void EnterState(GameManager manager)

        {

            Debug.Log("PlayerTurnState: Début du tour du joueur " + manager.CurrentPlayer.playerID);

            manager.CurrentPlayer.StartTurn(); // Démarre le tour du joueur. On remet a 0 le revealedCardCount

            //A l'entré de l'état, on passe directement au choix de la pioche

            manager.TransitionToState(new DrawChoiceState());



            // Plus besoin de s'abonner aux événements ici, ils seront gérés par les sous-états.

            // SubscribeToEvents(manager);

        }



        public void ExecuteState(GameManager manager)

        {

            //Plus de logique ici, tout est géré par les sous-état

        }



        public void ExitState(GameManager manager)

        {

            Debug.Log("PlayerTurnState: Fin du tour du joueur " + manager.CurrentPlayer.playerID);

            //Desabonnement aux evenements

            // UnsubscribeFromEvents(manager); Plus besoin ici

        }



        //Toutes les méthodes Handle... sont supprimé, car géré par les sous-état



        //Supprime les Subscribe/Unsubscribe

        //Supprime le HandleCardDraw

    }

}
