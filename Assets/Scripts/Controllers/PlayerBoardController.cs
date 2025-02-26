// --- Controllers/PlayerBoardController.cs --- (Refonte de RemoveCardsAndDiscard)
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Views;
using com.hyminix.game.ojyx.Managers;

namespace com.hyminix.game.ojyx.Controllers
{
    [RequireComponent(typeof(PlayerBoardView))]
    public class PlayerBoardController : MonoBehaviour
    {
        [Title("Configuration du Plateau")]
        [SerializeField] public int columns = 4;
        [SerializeField] public int rows = 3;

        private PlayerBoard playerBoard;
        public PlayerBoard PlayerBoard => playerBoard;

        public PlayerBoardView playerBoardView;

        public void Initialize()
        {
            playerBoard = new PlayerBoard(columns, rows);
            playerBoardView = GetComponent<PlayerBoardView>();
            if (playerBoardView == null)
            {
                Debug.LogError("PlayerBoardView not found on " + gameObject.name);
                return;
            }
            GenerateBoardSlots();
        }

        [Button("Générer les Emplacements")]
        public void GenerateBoardSlots()
        {
            ClearBoard();

            foreach (CardSlot modelSlot in playerBoard.cardSlots)
            {
                Vector3 slotPos = transform.position + new Vector3(modelSlot.column * 2f, 0, -modelSlot.row * 2.5f); //Valeurs par défaut pour le moment.
                GameObject slotObj = Instantiate(playerBoardView.cardSlotPrefab, slotPos, Quaternion.identity, transform);
                CardSlotController slotController = slotObj.GetComponent<CardSlotController>();
                if (slotController == null)
                {
                    Debug.LogError("CardSlotController non trouvé sur le prefab du slot !");
                    continue;
                }

                slotController.Initialize(modelSlot);
                playerBoardView.cardSlots.Add(slotController);
            }
        }

        private void ClearBoard()
        {
            foreach (var slot in playerBoardView.cardSlots)
            {
                if (slot != null && slot.gameObject != null)
                {
                    Destroy(slot.gameObject);
                }
            }
            playerBoardView.cardSlots.Clear();

            if (playerBoard != null)
                playerBoard.Clear();
        }


        public bool AreAllCardsRevealed()
        {
            return playerBoard.AreAllCardsRevealed();
        }

        public CardController GetCardControllerAt(int row, int col)
        {
            var slot = playerBoardView.cardSlots.Find(s => s.cardSlot.row == row && s.cardSlot.column == col);
            return slot != null ? slot.CardController : null;
        }


        // MODIFICATION : CheckBoard retourne maintenant directement une liste de CardSlot.
        public void CheckBoard()
        {
            List<CardSlot> slotsToRemove = new List<CardSlot>();
            slotsToRemove.AddRange(GetCompletedColumns());
            slotsToRemove.AddRange(GetCompletedRows());


            if (slotsToRemove.Count > 0)
            {
                RemoveCardsAndDiscard(slotsToRemove); // On passe directement les CardSlots
            }
        }

        // NOUVELLE MÉTHODE : Récupère les CardSlots des colonnes complètes
        private List<CardSlot> GetCompletedColumns()
        {
            List<CardSlot> completedSlots = new List<CardSlot>();
            foreach (var column in playerBoard.CheckForCompletedColumns())
            {
                completedSlots.Add(playerBoard.GetCardSlotAt(column.row, column.col));
            }
            return completedSlots;
        }

        // NOUVELLE MÉTHODE : Récupère les CardSlots des lignes complètes
        private List<CardSlot> GetCompletedRows()
        {
            List<CardSlot> completedSlots = new List<CardSlot>();
            foreach (var row in playerBoard.CheckForCompletedRows())
            {
                completedSlots.Add(playerBoard.GetCardSlotAt(row.row, row.col));
            }
            return completedSlots;
        }


        // MODIFICATION : Prend une liste de CardSlot, gère tout le processus.
        public void RemoveCardsAndDiscard(List<CardSlot> slots)
        {
            foreach (CardSlot slot in slots)
            {
                if (slot.IsOccupied)
                {
                    //Trouver le slot controller associé à ce slot
                    CardSlotController slotController = playerBoardView.cardSlots.Find(s => s.cardSlot == slot);
                    // 1. Récupérer le CardController (pour l'animation).
                    CardController cardController = slotController.CardController;

                    // 2. Animer la défausse (si le CardController existe).
                    if (cardController != null)
                    {
                        GameManager.Instance.DeckController.DiscardCardWithAnimation(cardController, 0.5f, DG.Tweening.Ease.OutQuad);
                    }

                    // 3. Retirer la carte du slot (modèle ET vue).  Ceci met à jour *correctement* l'état du jeu.
                    slotController.RemoveCard();
                }
            }

        }



    }
}