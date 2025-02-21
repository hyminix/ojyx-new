// --- Views/DeckView.cs ---
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
        //SUPPRIMER [SerializeField] private float offsetZ = 0.02f;

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

            // Calcule le décalage dynamique.
            float maxOffset = 0.02f;
            float minOffset = 0.005f;
            float offset = Mathf.Lerp(maxOffset, minOffset, (float)deckModel.cards.Count / 150f);


            for (int i = 0; i < deckModel.cards.Count; i++)
            {
                Card card = deckModel.cards[i];
                CardController cardController = CreateCardController(card);
                cardController.transform.SetParent(deckContainer);
                float zPos = -offset * i;  // Utilise le décalage dynamique
                cardController.transform.localPosition = new Vector3(0, 0, zPos);
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

            // Retire la carte du modèle *AVANT* de manipuler la vue.
            Card topCard = deckModel.DrawCard();
            if (topCard == null) return null;

            //Trouve le controller qui correspond a la carte
            CardController topCardController = null;
            foreach (CardController cardC in deckCardControllers)
            {
                if (cardC.Card == topCard)
                {
                    topCardController = cardC;
                    break;
                }
            }

            if (topCardController == null) return null; //Ne devrait jamais arriver

            deckCardControllers.Remove(topCardController); // Retire de la liste *locale*.
            // topCardController.transform.SetParent(null); // Détache la carte *avant* de la renvoyer.
            RefreshDeckView(); // On rafraichit la vue pour repositionner les cartes
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