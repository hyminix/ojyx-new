// Dans un nouveau fichier C#, par exemple : Enums/PlayerAction.cs

namespace com.hyminix.game.ojyx.Enums

{

    public enum PlayerAction

    {

        DrawFromDeck,

        DrawFromDiscard,

        PlaceCard,

        RevealCard,

        DiscardDrawnCard, // Défausser la carte piochée

        // D'autres actions pourront être ajoutées ici, comme CheckBoard

    }

}
