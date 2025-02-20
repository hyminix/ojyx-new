// --- Controllers/PlayerBoardController.cs ---
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Views;

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
            // Crée le modèle de plateau qui va contenir une liste de CardSlot
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
            // Pour chaque CardSlot du modèle, on instancie un slot visuel et on l'initialise avec l'instance du modèle
            foreach (CardSlot modelSlot in playerBoard.cardSlots)
            {
                // Calculer la position en fonction des coordonnées du modèle
                Vector3 slotPos = playerBoardView.boardCenter.position +
                    new Vector3(modelSlot.column * playerBoardView.slotSpacing.x, 0, -modelSlot.row * playerBoardView.slotSpacing.y);
                GameObject slotObj = Instantiate(playerBoardView.cardSlotPrefab, slotPos, Quaternion.identity, transform);
                CardSlotController slotController = slotObj.GetComponent<CardSlotController>();
                if (slotController == null)
                {
                    Debug.LogError("CardSlotController non trouvé sur le prefab du slot !");
                    continue;
                }
                // Initialiser le slot avec le modèle existant
                slotController.Initialize(modelSlot);
                // Enregistrer ce slot dans la vue pour qu'il soit accessible par le GameManager
                playerBoardView.cardSlots.Add(slotController);
            }
        }

        private void ClearBoard()
        {
            foreach (var slot in playerBoardView.cardSlots)
            {
                if (slot != null)
                    Destroy(slot.gameObject);
            }
            playerBoardView.cardSlots.Clear();
            if (playerBoard != null)
                playerBoard.Clear();
        }

        public bool AreAllCardsRevealed()
        {
            return playerBoard.AreAllCardsRevealed();
        }

        // Méthode simplifiée pour récupérer le CardController à une position donnée
        public CardController GetCardControllerAt(int row, int col)
        {
            var slot = playerBoardView.cardSlots.Find(s => s.cardSlot.row == row && s.cardSlot.column == col);
            return slot != null ? slot.CardController : null;
        }

        // Méthode pour forcer le placement initial d'une carte
        public void PlaceCardInitial(Card card, CardController cardController)
        {
            foreach (CardSlotController slotView in playerBoardView.cardSlots)
            {
                if (slotView.cardSlot.row == card.row && slotView.cardSlot.column == card.column)
                {
                    slotView.PlaceCard(cardController);
                    return;
                }
            }
            Debug.LogError($"Impossible de trouver le slot pour la carte à la position ({card.row}, {card.column})");
        }
    }
}
