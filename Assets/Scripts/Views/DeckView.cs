// --- Views/DeckView.cs ---
using UnityEngine;
using Sirenix.OdinInspector;
namespace com.hyminix.game.ojyx.Views
{
    public class DeckView : MonoBehaviour
    {
        //Pas besoin de serialize field, on va directement acceder a ce script
        public GameObject cardPrefab; //Le prefab de la carte
        public Transform deckPosition; //Position du deck
    }
}
