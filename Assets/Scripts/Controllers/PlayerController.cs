// --- Controllers/PlayerController.cs --- (Modifié)
using UnityEngine;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.Controllers
{
        public class PlayerController : MonoBehaviour
        {
                [Title("Informations Joueur")]
                [ReadOnly, ShowInInspector]
                public int playerID;

                [Title("Composants")]
                [SerializeField, ReadOnly]
                private PlayerBoardController playerBoardController;  // Référence au *contrôleur* du plateau.
                public PlayerBoardController PlayerBoardController => playerBoardController; //Accesseur public

                [ShowInInspector, ReadOnly]
                private Player player; // Référence au modèle Player.
                public Player Player => player; //Accesseur public

                //Initialisation du joueur
                public void Initialize(int id)
                {
                        playerID = id;
                        player = new Player(playerID); // Crée le modèle Player.

                        playerBoardController = GetComponent<PlayerBoardController>(); //On récupere le controller
                        if (playerBoardController == null)
                        {
                                Debug.LogError("PlayerBoardController not found on " + gameObject.name);
                                return;
                        }
                        playerBoardController.Initialize(); // Initialise le contrôleur du plateau.
                }

                //Fonction de début de tour
                public void StartTurn()
                {
                        Debug.Log("Tour du joueur " + playerID + " démarré.");
                        player.revealedCardCount = 0; // On remet à 0 au début du tour.
                }

                //Fonction de fin de tour
                public void EndTurn()
                {
                        Debug.Log("Tour du joueur " + playerID + " terminé.");
                }

                //Fonction pour vérifier si le tour est terminé
                public bool IsTurnComplete()
                {
                        // TODO:  Implémenter la logique de fin de tour.
                        return false;
                }

                // --- DistributeInitialCards : CORRIGÉ pour utiliser DrawFromDeck ---
                public void DistributeInitialCards(DeckController deckController)
                {
                        for (int i = 0; i < playerBoardController.rows * playerBoardController.columns; i++)
                        {
                                CardController drawnCardController = deckController.DrawFromDeck(); // Utilise DrawFromDeck
                                if (drawnCardController != null)
                                {
                                        Card drawnCard = drawnCardController.Card; // Récupère le modèle.
                                        player.AddCardToHand(drawnCard);  // Ajoute au modèle (pourrait être supprimé si pas nécessaire)

                                        // Placement de la carte (logique simplifiée)
                                        foreach (var slotView in playerBoardController.playerBoardView.cardSlots)
                                        {
                                                if (!slotView.cardSlot.IsOccupied) // Trouve le premier slot *vue* vide
                                                {
                                                        drawnCard.SetPosition(slotView.cardSlot.row, slotView.cardSlot.column);
                                                        playerBoardController.PlayerBoard.PlaceCard(drawnCard); // Place dans le modèle
                                                        slotView.PlaceCard(drawnCardController); // Place le CardController dans la vue
                                                        break; // Important: Sortir de la boucle après avoir placé la carte.
                                                }
                                        }
                                }
                        }
                }
                // --- Fin de DistributeInitialCards ---


                //Fonction pour révéler une carte
                public void RevealCard(CardController cardController)
                {
                        if (player.RevealCard(cardController.Card)) //On revele la carte
                        {
                                cardController.Flip(); //On retourne visuellement la carte
                        }
                }

                //Fonction pour défausser une carte
                public void DiscardCard(CardController cardController)
                {
                        //Retire la carte de la main (si elle y est)
                        player.RemoveCardFromHand(cardController.Card);
                        GameManager.Instance.DeckController.DiscardCard(cardController.Card); //On défausse, et on utilise le .Card
                }
        }
}
