// --- Utils/GameEvents.cs ---
using System;
using com.hyminix.game.ojyx.Controllers;

// Pas de namespace pour les classes statiques globales
public static class GameEvents
{
    // Evénement pour quand un slot de carte est cliqué
    public static event Action<CardSlotController> OnCardSlotClicked; // MODIFIÉ : Plus besoin de PointerEventData

    public static void TriggerCardSlotClicked(CardSlotController slotController) // MODIFIÉ
    {
        OnCardSlotClicked?.Invoke(slotController); // MODIFIÉ
    }

    // Tu peux ajouter d'autres événements ici, par exemple:
    // public static event Action<PlayerController> OnTurnEnded;
    // public static event Action<int> OnScoreChanged;
    // ... etc.  Un événement par type d'action importante dans ton jeu.
}