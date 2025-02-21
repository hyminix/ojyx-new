// --- Models/CardSlot.cs ---
using UnityEngine;
using Sirenix.OdinInspector;

namespace com.hyminix.game.ojyx.Models
{
    public class CardSlot
    {
        [ShowInInspector, ReadOnly] public int row;
        [ShowInInspector, ReadOnly] public int column;
        [ShowInInspector, ReadOnly] public Card card; // Référence au *modèle* de carte.
        public bool IsOccupied => card != null;

        public CardSlot(int row, int column)
        {
            this.row = row;
            this.column = column;
            this.card = null;
        }

        public void PlaceCard(Card card)
        {
            this.card = card;
        }

        public Card RemoveCard()
        {
            Card temp = this.card;
            this.card = null;
            return temp;
        }

        public void Clear()
        {
            card = null;
        }
    }
}