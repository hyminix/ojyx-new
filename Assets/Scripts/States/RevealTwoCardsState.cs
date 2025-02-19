// --- States/RevealTwoCardsState.cs --- (Version Complète)
using UnityEngine;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.States
{
    public class RevealTwoCardsState : IGameState, ICardClickHandler
    {
        private int playersRevealedCount = 0; // Compteur de joueurs ayant révélé leurs cartes

        public void EnterState(GameManager manager)
        {
            Debug.Log("RevealTwoCardsState: Chaque joueur révèle deux cartes.");
            playersRevealedCount = 0; // Initialisation du compteur
            SubscribeToEvents(manager); // S'abonner aux événements de clic
            //On affiche le joueur actuel
            Debug.Log("Joueur " + manager.CurrentPlayer.playerID + " veuillez révéler deux cartes");
        }

        public void ExecuteState(GameManager manager)
        {
            //Tout est géré dans le OnCardClick
        }

        public void ExitState(GameManager manager)
        {
            UnsubscribeFromEvents(manager); // Se désabonner
            Debug.Log("RevealTwoCardsState: Fin de la révélation des cartes.");
        }

        //Fonction appelé au clic de la carte
        public void HandleCardClick(GameManager manager, CardController cardController)
        {
            // Vérifie si la carte cliquée appartient au joueur actuel *ET* si elle est face cachée.
            //Si la carte est déjà face visible ou n'appartient pas au joueur, on ne fait rien
            if (cardController.Card.IsFaceUp || !IsCardBelongToPlayer(manager.CurrentPlayer, cardController.Card))
            {
                return;
            }

            //Si le joueur a déjà revelé deux cartes, on ne fait rien
            if (manager.CurrentPlayer.Player.revealedCardCount >= manager.CurrentPlayer.Player.maxRevealedCards)
            {
                return;
            }

            //Revele la carte visuellement et au niveau du model
            cardController.Flip();

            //Incrémente le compteur de carte révélé par le joueur
            manager.CurrentPlayer.Player.revealedCardCount++;

            //Si le joueur a revelé deux cartes
            if (manager.CurrentPlayer.Player.revealedCardCount >= manager.CurrentPlayer.Player.maxRevealedCards)
            {
                playersRevealedCount++; //On incrémente le compteur de joueur

                //Si tous les joueurs ont revelé leur carte
                if (playersRevealedCount >= manager.players.Count)
                {
                    DetermineFirstPlayer(manager); //On determine le premier joueur
                    manager.TransitionToState(new DiscardFirstCardState()); //On passe a l'état suivant
                    return; // Important : on quitte la méthode ici.
                }
                else
                {
                    //On passe au joueur suivant
                    manager.NextPlayer();
                    Debug.Log("Joueur " + manager.CurrentPlayer.playerID + " veuillez révéler deux cartes");
                }
            }
        }

        //Fonction pour verifier si la carte appartient bien au joueur
        private bool IsCardBelongToPlayer(PlayerController playerController, Models.Card card)
        {
            foreach (var slot in playerController.PlayerBoardController.playerBoardView.cardSlots)
            {
                //Si le slot n'est pas vide
                if (slot.cardSlot.card != null)
                {
                    //Si la carte du slot correspond a la carte cliqué
                    if (slot.cardSlot.card == card)
                    {
                        return true; //On retourne true
                    }
                }
            }
            return false;
        }


        private void SubscribeToEvents(GameManager manager)
        {
            //On s'abonne aux evenements des cartes du joueur
            // OPTIMISATION:  On s'abonne SEULEMENT aux cartes du joueur courant.
            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)
            {
                if (slot.cardSlot.card != null) // S'il y a une carte
                {
                    CardController cardController = slot.GetComponentInChildren<CardController>();
                    if (cardController != null)
                    {
                        cardController.OnCardClicked += manager.OnCardClicked;
                    }
                }
            }
        }

        private void UnsubscribeFromEvents(GameManager manager)
        {
            //On se désabonne
            foreach (var slot in manager.CurrentPlayer.PlayerBoardController.playerBoardView.cardSlots)
            {
                if (slot.cardSlot.card != null) // S'il y a une carte
                {
                    CardController cardController = slot.GetComponentInChildren<CardController>();
                    if (cardController != null)
                    {
                        cardController.OnCardClicked -= manager.OnCardClicked;
                    }
                }
            }
        }



        private void DetermineFirstPlayer(GameManager manager)
        {
            // TODO: Implémenter la logique pour déterminer le premier joueur
            // (par exemple, en comparant la somme des valeurs des cartes révélées).
            // Pour l'instant, on commence par le joueur 0.
            manager.currentPlayerIndex = 0;
        }
    }
}
