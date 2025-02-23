// --- Controllers/PlayerController.cs --- (Ajout des couleurs des cartes)

using UnityEngine;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Managers;
using System.Collections.Generic;

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

                [ShowInInspector, ReadOnly]
                private List<string> boardCards = new List<string>();

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
                        UpdateDebugInfo(); // Mis à jour ici !
                }

                public void RevealCard(CardController cardController)
                {
                        if (player.RevealCard(cardController.Card))
                        {
                                cardController.Flip();
                                UpdateDebugInfo(); // Met à jour l'affichage après avoir révélé une carte
                        }
                }

                public void DiscardCard(CardController cardController)
                {
                        GameManager.Instance.DeckController.DiscardCardWithAnimation(cardController, 0.5f, DG.Tweening.Ease.OutQuad);
                        UpdateDebugInfo(); // Met à jour l'affichage après avoir défaussé
                }

                public void UpdateDebugInfo()
                {
                        if (playerBoardController != null && playerBoardController.PlayerBoard != null)
                        {
                                boardCards.Clear();
                                foreach (var slot in playerBoardController.PlayerBoard.cardSlots)
                                {
                                        if (slot.IsOccupied)
                                        {
                                                boardCards.Add($"[{slot.row},{slot.column}] : {slot.card.Data.value} ({(slot.card.IsFaceUp ? "V" : "C")})");
                                        }
                                        else
                                        {
                                                boardCards.Add($"[{slot.row},{slot.column}] : Vide");
                                        }
                                }
                        }
                }
        }
}