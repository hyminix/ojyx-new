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
        [SerializeField] private float offsetZ = 0.02f;       // Décalage en Z entre les cartes

        [Title("Prefab de la carte")]
        public GameObject cardPrefab; // Doit être public pour être utilisé par le DeckController

        [Title("Référence Modèle")]
        [ReadOnly, SerializeField] public Deck deckModel; // Assigné par le DeckController

        // Liste interne de CardController affichés
        private List<CardController> deckCardControllers = new List<CardController>();

        /// <summary>
        /// Rafraîchit l'affichage complet du deck en pile.
        /// </summary>
        [Button("Refresh Deck View")]
        public void RefreshDeckView()
        {
            ClearDeckView();
            if (deckModel == null || deckModel.cards.Count == 0) return;

            for (int i = 0; i < deckModel.cards.Count; i++)
            {
                Card card = deckModel.cards[i];
                CardController cardController = CreateCardController(card);
                cardController.transform.SetParent(deckContainer);
                float zPos = -offsetZ * i;
                cardController.transform.localPosition = new Vector3(0, 0, zPos);
                cardController.transform.localRotation = Quaternion.identity;
                deckCardControllers.Add(cardController);
            }
        }

        /// <summary>
        /// Met à jour uniquement la position des cartes restantes dans le deck.
        /// </summary>
        public void UpdateDeckPositions()
        {
            for (int i = 0; i < deckCardControllers.Count; i++)
            {
                float zPos = -offsetZ * i;
                deckCardControllers[i].transform.localPosition = new Vector3(0, 0, zPos);
            }
        }

        /// <summary>
        /// Renvoie le CardController de la carte du dessus du deck sans le retirer.
        /// </summary>
        public CardController PeekTopCardController()
        {
            int count = deckContainer.childCount;
            if (count <= 0)
                return null;
            return deckContainer.GetChild(count - 1).GetComponent<CardController>();
        }


        /// <summary>
        /// Retire la carte du dessus du deck (modèle et vue).
        /// </summary>
        public CardController RemoveTopCardController()
        {
            if (deckModel == null || deckModel.cards.Count == 0) return null;
            Debug.Log("RemoveTopCardController: deckModel non nul, count = " + deckModel.cards.Count);
            // Retire la carte du modèle
            Card topCard = deckModel.DrawCard();
            if (topCard == null) return null;
            Debug.Log("RemoveTopCardController: topCard retirée du modèle");

            int lastIndex = deckCardControllers.Count - 1;
            if (lastIndex < 0)
            {
                Debug.LogWarning("RemoveTopCardController: aucune carte dans deckCardControllers");
                return null;
            }
            CardController topCardController = deckCardControllers[lastIndex];
            deckCardControllers.RemoveAt(lastIndex);
            topCardController.transform.SetParent(null);
            return topCardController;
        }

        /// <summary>
        /// Vide complètement la vue du deck.
        /// </summary>
        [Button("Clear Deck View")]
        public void ClearDeckView()
        {
            for (int i = deckContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(deckContainer.GetChild(i).gameObject);
            }
            deckCardControllers.Clear();
        }

        /// <summary>
        /// Crée un CardController pour une carte donnée.
        /// </summary>
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
