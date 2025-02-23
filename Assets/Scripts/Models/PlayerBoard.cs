// --- Models/PlayerBoard.cs --- (Suppression de RemoveCardsAt, modification de CheckForCompletedRows/Columns)

using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace com.hyminix.game.ojyx.Models
{
    public class PlayerBoard
    {
        [ShowInInspector, ReadOnly]
        public List<CardSlot> cardSlots = new List<CardSlot>();
        public readonly int columns;
        public readonly int rows;

        public PlayerBoard(int columns, int rows)
        {
            this.columns = columns;
            this.rows = rows;
            cardSlots = new List<CardSlot>(); // Initialise la liste
            Initialize(); // Initialise le plateau lors de la construction.
        }

        private void Initialize()
        {
            //Crée les CardSlots.
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    cardSlots.Add(new CardSlot(row, col));
                }
            }
        }

        //Place une carte sur le plateau
        public bool PlaceCard(Card card)
        {
            //On cherche un slot vide
            foreach (var slot in cardSlots)
            {
                if (!slot.IsOccupied)
                {
                    slot.PlaceCard(card); //Place la carte
                    return true;
                }
            }
            return false; // Plateau plein
        }

        //Récupère une carte à une position donnée
        public Card GetCardAt(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= columns) //On vérifie les limites
            {
                return null;
            }
            foreach (var slot in cardSlots)
            {
                if (slot.row == row && slot.column == col)
                {
                    return slot.card;
                }
            }
            return null;
        }

        //Récupère un CardSlot à une position donnée
        public CardSlot GetCardSlotAt(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= columns)
            {
                return null;
            }
            foreach (var slot in cardSlots)
            {
                if (slot.row == row && slot.column == col)
                {
                    return slot;
                }
            }
            return null;
        }

        //Verifie si toutes les cartes sont révélées
        public bool AreAllCardsRevealed()
        {
            foreach (var slot in cardSlots)
            {
                if (slot.IsOccupied && !slot.card.IsFaceUp)
                {
                    return false; // Carte face cachée trouvée.
                }
            }
            return true;
        }

        //Vide le plateau
        public void Clear()
        {
            foreach (var slot in cardSlots)
            {
                slot.Clear();
            }
        }

        // AJOUT : Vérifie si une colonne est complète (3 cartes identiques face visible). Retourne les coordonées
        public List<(int row, int col)> CheckForCompletedColumns()
        {
            List<(int row, int col)> completedColumns = new List<(int row, int col)>();

            for (int col = 0; col < columns; col++)
            {
                Card card1 = GetCardAt(0, col);
                Card card2 = GetCardAt(1, col);
                Card card3 = GetCardAt(2, col);

                if (card1 != null && card2 != null && card3 != null &&
                    card1.IsFaceUp && card2.IsFaceUp && card3.IsFaceUp &&
                    card1.Data.value == card2.Data.value && card2.Data.value == card3.Data.value)
                {
                    completedColumns.Add((0, col));
                    completedColumns.Add((1, col));
                    completedColumns.Add((2, col));
                }
            }

            return completedColumns;
        }

        // AJOUT : Vérifie si une ligne est complète (4 cartes identiques face visible).  Retourne les coordonées
        public List<(int row, int col)> CheckForCompletedRows()
        {
            List<(int row, int col)> completedRows = new List<(int row, int col)>();

            for (int row = 0; row < rows; row++)
            {
                Card card1 = GetCardAt(row, 0);
                Card card2 = GetCardAt(row, 1);
                Card card3 = GetCardAt(row, 2);
                Card card4 = GetCardAt(row, 3);

                if (card1 != null && card2 != null && card3 != null && card4 != null &&
                    card1.IsFaceUp && card2.IsFaceUp && card3.IsFaceUp && card4.IsFaceUp &&
                    card1.Data.value == card2.Data.value && card2.Data.value == card3.Data.value && card3.Data.value == card4.Data.value)
                {
                    completedRows.Add((row, 0));
                    completedRows.Add((row, 1));
                    completedRows.Add((row, 2));
                    completedRows.Add((row, 3));
                }
            }

            return completedRows;
        }
    }
}