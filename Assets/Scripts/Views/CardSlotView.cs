// --- Views/CardSlotView.cs --- (SIMPLIFIÉ)

using UnityEngine;



namespace com.hyminix.game.ojyx.Views

{

    public class CardSlotView : MonoBehaviour

    {

        [SerializeField] private Color emptyColor = Color.gray;

        [SerializeField] private Color occupiedColor = Color.green;

        [SerializeField] private Color hoverColor = Color.cyan; // Garde ça pour le highlight

        private Renderer slotRenderer;

        private Models.CardSlot cardSlot;



        private void Awake()

        {

            slotRenderer = GetComponent<Renderer>();

        }



        public void Initialize(Models.CardSlot slot)

        {

            cardSlot = slot;

            UpdateVisual();

        }



        public void UpdateVisual()

        {

            if (slotRenderer != null)

            {

                // Simplifié: utilise directement les couleurs empty/occupied.

                slotRenderer.material.color = cardSlot.IsOccupied ? occupiedColor : emptyColor;

            }

        }



        // Simplifié: une seule méthode SetHighlight

        public void SetHighlight(bool active)

        {

            if (slotRenderer != null)

            {

                slotRenderer.material.color = active ? hoverColor : (cardSlot.IsOccupied ? occupiedColor : emptyColor); //On utilise la couleur de base si on desactive le highlight

            }

        }





    }

}
