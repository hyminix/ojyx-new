// --- Views/CardSlotView.cs ---

using UnityEngine;



namespace com.hyminix.game.ojyx.Views

{

    public class CardSlotView : MonoBehaviour

    {

        [SerializeField] private Color emptyColor = Color.gray;

        [SerializeField] private Color occupiedColor = Color.green;

        [SerializeField] private Color hoverColor = Color.cyan;



        private Renderer slotRenderer;

        private Models.CardSlot cardSlot; // Référence au modèle



        private void Awake()

        {

            slotRenderer = GetComponent<Renderer>();

            // L'initialisation se fait maintenant dans Initialize().

        }



        //Initialisation du slot

        public void Initialize(Models.CardSlot slot)

        {

            cardSlot = slot;

            UpdateVisual();

        }



        //Fonction de mise a jour de l'affichage

        public void UpdateVisual()

        {

            if (slotRenderer != null)

            {

                slotRenderer.material.color = cardSlot.IsOccupied ? occupiedColor : emptyColor;

            }

        }



        //Fonction pour le highlight

        public void SetHighlight(bool active)

        {

            if (slotRenderer != null)

            {

                slotRenderer.material.color = active ? hoverColor : (cardSlot.IsOccupied ? occupiedColor : emptyColor);

            }

        }



        //Fonction pour le hover

        private void OnMouseEnter()

        {

            if (!cardSlot.IsOccupied && slotRenderer != null)

                slotRenderer.material.color = hoverColor;

        }

        //Fonction pour la sortie du hover

        private void OnMouseExit()

        {

            UpdateVisual();

        }

    }

}

