// Dans un nouveau fichier C#, par exemple : Enums/PlayerAction.cs
namespace com.hyminix.game.ojyx.Enums
{
    public enum PlayerAction
    {
        DrawFromDeck,
        DrawFromDiscard,
        DrawFromAction,
        PlaceActionCard,
        DiscardActionCard,
        PlaceCard,
        RevealCard,
        DiscardDrawnCard, // Défausser la carte piochée
                          // D'autres actions pourront être ajoutées ici, comme CheckBoard
    }
}
