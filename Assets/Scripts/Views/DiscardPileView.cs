using UnityEngine;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Models;
using com.hyminix.game.ojyx.Controllers;
using System.Collections.Generic;

namespace com.hyminix.game.ojyx.Views
{
        public class DiscardPileView : MonoBehaviour
        {
                [Title("Conteneur de la défausse (pile)")]
                [SerializeField] private Transform discardPileContainer;
                [SerializeField] private float offsetZ = 0.02f;

                [Title("Référence Modèle")]
                [ReadOnly, SerializeField] public DiscardPile discardPileModel;

                public void AddCardToDiscardPile(CardController cardController)
                {
                        if (discardPileModel == null)
                        {
                                Debug.LogError("DiscardPileView: discardPileModel n'est pas assigné !");
                                return;
                        }
                        discardPileModel.AddCard(cardController.Card);
                        cardController.transform.SetParent(discardPileContainer);
                        int count = discardPileModel.cards.Count;
                        cardController.transform.localPosition = new Vector3(0, 0, -offsetZ * (count - 1));
                        cardController.transform.localRotation = Quaternion.identity;
                        Debug.Log($"DiscardPileView: Ajout de la carte {cardController.Card.Data.value} en position {count} de la pile.");
                }

                public CardController DrawTopCardController()
                {
                        if (discardPileModel == null)
                        {
                                Debug.LogError("DiscardPileView: discardPileModel n'est pas assigné !");
                                return null;
                        }
                        if (discardPileModel.cards.Count == 0)
                        {
                                Debug.LogWarning("DiscardPileView: La défausse est vide !");
                                return null;
                        }
                        Card topCard = discardPileModel.DrawCard();
                        CardController topCardController = GetTopCardController();
                        if (topCardController == null)
                        {
                                Debug.LogError("DiscardPileView: Impossible de trouver le CardController du dessus !");
                                return null;
                        }
                        topCardController.transform.SetParent(null);
                        topCardController.Initialize(topCard);
                        Debug.Log($"DiscardPileView: Carte {topCard.Data.value} retirée de la défausse.");
                        return topCardController;
                }

                public CardController GetTopCardController()
                {
                        int lastIndex = discardPileContainer.childCount - 1;
                        if (lastIndex < 0) return null;
                        return discardPileContainer.GetChild(lastIndex).GetComponent<CardController>();
                }

                [Button("Clear Discard Pile")]
                public void Clear()
                {
                        for (int i = discardPileContainer.childCount - 1; i >= 0; i--)
                        {
                                Destroy(discardPileContainer.GetChild(i).gameObject);
                        }
                }
        }
}
