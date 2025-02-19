// --- Controllers/PlayerBoardController.cs ---
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
        // Ajout du getter PUBLIC.  C'est le modèle, il DOIT être accessible.
        public PlayerBoard PlayerBoard => playerBoard;

        public PlayerBoardView playerBoardView;

        public void Initialize()
        {
            playerBoard = new PlayerBoard(columns, rows);
            playerBoardView = GetComponent<PlayerBoardView>();
            GenerateBoardSlots();
        }

        // ... (le reste du code de PlayerBoardController est inchangé) ...
        [Button("Générer les Emplacements")]
        public void GenerateBoardSlots()
        {
            ClearBoard();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Vector3 slotPos = playerBoardView.transform.position + new Vector3(col * playerBoardView.slotSpacing.x, 0, -row * playerBoardView.slotSpacing.y);
                    GameObject slotObj = Instantiate(playerBoardView.cardSlotPrefab, slotPos, Quaternion.identity, transform);
                    CardSlotController slot = slotObj.GetComponent<CardSlotController>();
                    slot.Initialize(row, col);
                    playerBoardView.cardSlots.Add(slot);
                }
            }
        }

        // Ancienne méthode PlaceCard, renommée et conservée pour le drag & drop
        public bool PlaceCard(CardController cardController)
        {
            //Trouve un slot de libre dans le MODELE
            CardSlot slotToFill = null;
            foreach (var slot in playerBoard.cardSlots) // playerBoard.cardSlots est la liste des MODELES de slot.
            {
                if (!slot.IsOccupied)
                {
                    slotToFill = slot;
                    break;
                }
            }

            //Si il n'y a plus de slot de libre
            if (slotToFill == null)
            {
                return false;
            }

            // Place la carte dans le *bon* slot (celui trouvé)
            foreach (var slotView in playerBoardView.cardSlots)
            {
                if (slotView.cardSlot.row == slotToFill.row && slotView.cardSlot.column == slotToFill.column)
                {
                    slotView.PlaceCard(cardController);  // Place le CardController
                    return true;
                }
            }

            return false; // Ne devrait jamais arriver, mais c'est une sécurité.
        }

        // NOUVELLE MÉTHODE pour le placement initial
        public void PlaceCardInitial(Card card, CardController cardController)
        {
            // Trouver le slot *vue* correspondant aux coordonnées de la carte (modèle).
            foreach (var slotView in playerBoardView.cardSlots)
            {
                if (slotView.cardSlot.row == card.row && slotView.cardSlot.column == card.column)
                {
                    // Placement *direct* de la carte (pas de drag & drop)
                    slotView.PlaceCard(cardController);
                    return; // Important:  Sortir après avoir placé la carte
                }
            }

            Debug.LogError($"Impossible de trouver le slot pour la carte à la position ({card.row}, {card.column})");
        }

        public CardController GetCardControllerAt(int row, int col)
        {
            foreach (var slot in playerBoardView.cardSlots)
            {
                if (slot.cardSlot.row == row && slot.cardSlot.column == col)
                {
                    return slot.GetComponentInChildren<CardController>();
                }
            }
            return null;
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
    }
}
