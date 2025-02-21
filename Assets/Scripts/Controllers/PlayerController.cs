// --- Controllers/PlayerController.cs ---

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

                private PlayerBoardController playerBoardController;

                public PlayerBoardController PlayerBoardController => playerBoardController;



                [ShowInInspector, ReadOnly]

                private Player player;

                public Player Player => player;



                public void Initialize(int id)

                {

                        playerID = id;

                        player = new Player(playerID);

                        playerBoardController = GetComponent<PlayerBoardController>();

                        if (playerBoardController == null)

                        {

                                Debug.LogError("PlayerBoardController not found on " + gameObject.name);

                                return;

                        }

                        playerBoardController.Initialize();

                }





                public void StartTurn()

                {

                        Debug.Log("Tour du joueur " + playerID + " démarré.");

                        player.revealedCardCount = 0;

                }



                public void EndTurn()

                {

                        Debug.Log("Tour du joueur " + playerID + " terminé.");

                }



                public bool IsTurnComplete()

                {

                        // TODO: Implémenter la logique de fin de tour.

                        return false;

                }



                public void DistributeInitialCards(DeckController deckController)

                {

                        for (int i = 0; i < playerBoardController.rows * playerBoardController.columns; i++)

                        {

                                CardController drawnCardController = deckController.DrawFromDeck();

                                if (drawnCardController != null)

                                {

                                        Card drawnCard = drawnCardController.Card;

                                        player.AddCardToHand(drawnCard); // Potentiellement inutile, à voir

                                        // Placement de la carte (logique simplifiée)

                                        foreach (var slotView in playerBoardController.playerBoardView.cardSlots)

                                        {

                                                if (!slotView.cardSlot.IsOccupied) // Trouve le premier slot *vide*

                                                {

                                                        drawnCard.SetPosition(slotView.cardSlot.row, slotView.cardSlot.column);

                                                        // playerBoardController.PlayerBoard.PlaceCard(drawnCard); // Plus besoin de ça

                                                        slotView.PlaceCard(drawnCardController); // Place le CardController

                                                        break;

                                                }

                                        }

                                }

                        }

                }



                public void RevealCard(CardController cardController)

                {

                        if (player.RevealCard(cardController.Card))

                        {

                                cardController.Flip();

                        }

                }





                public void DiscardCard(CardController cardController)

                {

                        player.RemoveCardFromHand(cardController.Card); // Potentiellement inutile

                        GameManager.Instance.DeckController.DiscardCard(cardController.Card); // Plus besoin de passer le CardController

                }

        }

}
