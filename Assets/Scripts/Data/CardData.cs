// --- Data/CardData.cs ---
using UnityEngine;
using Sirenix.OdinInspector;
namespace com.hyminix.game.ojyx.Data
{
    [CreateAssetMenu(fileName = "NewCardData", menuName = "Skyjo/Card Data")]
    public class CardData : ScriptableObject
    {
        [Title("Informations sur la carte")]
        [LabelText("Valeur de la carte")]
        public int value;
        [Title("Visuels")]
        [PreviewField(75, ObjectFieldAlignment.Center)]
        public Texture2D cardTexture; //Texture de la carte
    }
}
