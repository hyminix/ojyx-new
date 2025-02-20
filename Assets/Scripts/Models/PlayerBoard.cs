// --- Models/PlayerBoard.cs ---
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
            foreach (var slot in cardSlots)
            {
                if (slot.row == row && slot.column == col)
                {
                    return slot.card;
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
    }
}
