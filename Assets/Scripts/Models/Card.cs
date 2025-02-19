// --- Models/Card.cs ---
namespace com.hyminix.game.ojyx.Models
{
    public class Card
    {
        public Data.CardData Data { get; private set; }
        public bool IsFaceUp { get; set; }
        public int row; // Ajout: Pour simplifier le placement
        public int column; // Ajout
        public Card(Data.CardData data)
        {
            Data = data;
            IsFaceUp = false;
        }
        public void Flip()
        {
            IsFaceUp = !IsFaceUp;
        }
        // Ajout: Méthode pour assigner la position
        public void SetPosition(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
}
