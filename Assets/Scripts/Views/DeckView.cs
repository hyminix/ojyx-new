// --- Views/DeckView.cs --- (Ordre de génération inversé)

using UnityEngine;

using Sirenix.OdinInspector;

using com.hyminix.game.ojyx.Models;

using com.hyminix.game.ojyx.Controllers;

using System.Collections.Generic;



namespace com.hyminix.game.ojyx.Views

{

    public class DeckView : MonoBehaviour

    {

        [Title("Conteneur du deck (pile)")]

        [SerializeField] private Transform deckContainer; // À assigner dans l'inspecteur



        [Title("Prefab de la carte")]

        public GameObject cardPrefab; // Doit être public pour être utilisé par le DeckController



        [Title("Référence Modèle")]

        [ReadOnly, SerializeField] public Deck deckModel; // Assigné par le DeckController



        // Liste interne de CardController affichés

        private List<CardController> deckCardControllers = new List<CardController>();



        [Button("Refresh Deck View")]

        public void RefreshDeckView()

        {

            ClearDeckView();

            if (deckModel == null || deckModel.cards.Count == 0) return;



            // AJOUT : Calcul du Z-offset dynamique.

            float maxOffset = 0.005f;  // Valeur maximale du décalage (à ajuster).

            float minOffset = 0.001f; // Valeur minimale (pour éviter le z-fighting).



            // Facteur d'interpolation linéaire basé sur le nombre de cartes.

            // On s'assure que le facteur ne dépasse pas 1 (pour éviter des valeurs de décalage trop grandes).

            float t = Mathf.Min(1f, (float)deckModel.cards.Count / 120f);



            // Décalage interpolé.

            float offset = Mathf.Lerp(maxOffset, minOffset, t);

            // MODIFICATION : Parcourir la liste à l'envers.

            for (int i = deckModel.cards.Count - 1; i >= 0; i--)

            {

                Card card = deckModel.cards[i];

                CardController cardController = CreateCardController(card);

                cardController.transform.SetParent(deckContainer);

                cardController.transform.localPosition = new Vector3(0, 0, 0); // Position de base (au lieu du z offset)

                cardController.transform.localRotation = Quaternion.identity;

                deckCardControllers.Add(cardController);

                //On force le flip des cartes pour les voir face caché

                if (cardController.Card.IsFaceUp)

                    cardController.Flip();

            }

        }





        public CardController RemoveTopCardController()

        {

            if (deckModel == null || deckModel.cards.Count == 0) return null;



            // Retire la carte du modèle (maintenant cohérent : dernière carte)

            Card topCard = deckModel.DrawCard();

            if (topCard == null) return null;



            // MODIFICATION : Retire le *dernier* CardController (visuellement au-dessus).

            CardController topCardController = deckCardControllers[deckCardControllers.Count - 1];

            deckCardControllers.RemoveAt(deckCardControllers.Count - 1);



            return topCardController;

        }







        [Button("Clear Deck View")]

        public void ClearDeckView()

        {

            // Destruction propre des GameObjects.

            for (int i = deckCardControllers.Count - 1; i >= 0; i--)

            {

                if (deckCardControllers[i] != null && deckCardControllers[i].gameObject != null)

                {

                    Destroy(deckCardControllers[i].gameObject);

                }

            }

            deckCardControllers.Clear();

        }



        private CardController CreateCardController(Card card)

        {

            GameObject cardObj = Instantiate(cardPrefab);

            CardController cc = cardObj.GetComponent<CardController>();

            if (cc == null)

                cc = cardObj.AddComponent<CardController>();

            cc.Initialize(card);

            return cc;

        }

    }

}