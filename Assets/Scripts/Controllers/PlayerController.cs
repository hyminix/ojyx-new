// --- Controllers/PlayerController.cs ---
//Suppression de la mise à jour des informations de Debug
using UnityEngine;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Managers;
using System.Collections.Generic;
using Obvious.Soap;

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

                [ShowInInspector, ReadOnly]
                public int InitialScore => player != null ? player.CalculateInitialScore() : 0;

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
                        return false;
                }

                public void DistributeInitialCards()
                {
                        DeckController deckController = GameManager.Instance.DeckController;
                        for (int i = 0; i < playerBoardController.rows * playerBoardController.columns; i++)
                        {
                                CardController drawnCardController = deckController.DrawFromDeck();
                                if (drawnCardController != null)
                                {
                                        Card drawnCard = drawnCardController.Card;

                                        foreach (var slotView in playerBoardController.playerBoardView.cardSlots)
                                        {
                                                if (!slotView.cardSlot.IsOccupied)
                                                {
                                                        drawnCard.SetPosition(slotView.cardSlot.row, slotView.cardSlot.column);
                                                        slotView.PlaceCard(drawnCardController);
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
                        GameManager.Instance.DeckController.DiscardCardWithAnimation(cardController, 0.5f, DG.Tweening.Ease.OutQuad);
                }

                // Cette méthode force la mise à jour visuelle de toutes les cartes du joueur
                public void RefreshCardsVisibility()
                {
                        if (playerBoardController == null || playerBoardController.playerBoardView == null)
                                return;

                        // Forcer la mise à jour de tous les slots de cartes
                        foreach (var slotController in playerBoardController.playerBoardView.cardSlots)
                        {
                                if (slotController != null && slotController.CardController != null)
                                {
                                        // Force la carte à se rafraîchir visuellement
                                        slotController.CardController.gameObject.SetActive(false);
                                        slotController.CardController.gameObject.SetActive(true);

                                        // Mettre à jour le visuel de la carte
                                        slotController.CardController.transform.localPosition = new Vector3(0, 0.1f, 0);

                                        // Au lieu d'accéder directement à cardSlotView, utiliser la méthode SetHighlight
                                        // pour forcer un rafraîchissement visuel
                                        slotController.SetHighlight(false);
                                }
                        }

                        // Force une mise à jour du plateau lui-même
                        playerBoardController.gameObject.SetActive(false);
                        playerBoardController.gameObject.SetActive(true);
                }
        }
}