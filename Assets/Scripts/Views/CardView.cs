// --- Views/CardView.cs ---

using UnityEngine;

using Sirenix.OdinInspector;

namespace com.hyminix.game.ojyx.Views

{

    public class CardView : MonoBehaviour

    {

        [SerializeField] private Renderer cardRenderer;

        [SerializeField] private Material frontMaterial;

        [SerializeField] private Material backMaterial;

        [SerializeField, ReadOnly] private Models.Card card; //Référence au model

        //Initialisation de la vue

        public void Initialize(Models.Card card)

        {

            this.card = card;

            UpdateVisual(); // Met à jour l'apparence

        }

        //Fonction pour mettre à jour l'affichage de la carte

        public void UpdateVisual()

        {

            if (card == null || cardRenderer == null) return;

            if (card.IsFaceUp)

            {

                Material newMat = new Material(frontMaterial);

                newMat.mainTexture = card.Data.cardTexture;

                cardRenderer.material = newMat;

            }

            else

            {

                cardRenderer.material = backMaterial;

            }

        }

    }

}
