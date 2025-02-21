// --- Views/DiscardPileView.cs ---
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
                [SerializeField] private Transform discardPileContainer; // Le conteneur *parent* de TOUTES les cartes.
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

                        // Pas besoin de récupérer l'ancienne carte ici !

                        discardPileModel.AddCard(cardController.Card); // Ajoute au *modèle*.
                        cardController.transform.SetParent(discardPileContainer); // Attache à la *vue* (au conteneur).

                        // Positionne la carte correctement.  Le z est négatif pour que les cartes s'empilent correctement.
                        // La carte du dessus a le z le plus grand (le plus proche de zéro).
                        int count = discardPileModel.cards.Count;
                        cardController.transform.localPosition = new Vector3(0, 0, -offsetZ * (count - 1));
                        cardController.transform.localRotation = Quaternion.identity;
                        Debug.Log($"DiscardPileView: Ajout de la carte {cardController.Card.Data.value} en position {count} de la pile.");

                        // Masque toutes les cartes SAUF la dernière.
                        // HideAllButTopCard();
                }

                // Nouvelle méthode pour masquer les cartes.
                private void HideAllButTopCard()
                {
                        for (int i = 0; i < discardPileContainer.childCount - 1; i++)
                        {
                                discardPileContainer.GetChild(i).gameObject.SetActive(false);
                        }
                        // S'assure que la dernière carte est visible
                        if (discardPileContainer.childCount > 0)
                        {
                                discardPileContainer.GetChild(discardPileContainer.childCount - 1).gameObject.SetActive(true);
                        }
                }


                public CardController DrawTopCardController()
                {
                        if (discardPileModel == null || discardPileModel.cards.Count == 0)
                        {
                                Debug.LogWarning("DiscardPileView: La défausse est vide !");
                                return null;
                        }

                        Card topCard = discardPileModel.DrawCard(); // Retire du *modèle*.
                        CardController topCardController = GetTopCardController(); // Récupère le *CardController* correspondant.

                        if (topCardController == null)
                        {
                                Debug.LogError("DiscardPileView: Impossible de trouver le CardController du dessus !");
                                return null;
                        }

                        topCardController.transform.SetParent(null); // Détache de la vue.
                                                                     // topCardController.Initialize(topCard); // Inutile : la carte est déjà initialisée

                        // Affiche la nouvelle carte du dessus (si elle existe)
                        if (discardPileModel.cards.Count > 0)
                        {
                                discardPileContainer.GetChild(discardPileContainer.childCount - 1).gameObject.SetActive(true);
                        }

                        Debug.Log($"DiscardPileView: Carte {topCard.Data.value} retirée de la défausse.");
                        return topCardController;
                }


                public CardController GetTopCardController()
                {
                        //On récupère la dernière carte
                        int lastIndex = discardPileContainer.childCount - 1;
                        if (lastIndex < 0) return null; // Aucune carte
                        return discardPileContainer.GetChild(lastIndex).GetComponent<CardController>(); // Renvoie le CardController.
                }


                [Button("Clear Discard Pile")]
                public void Clear()
                {
                        // Destruction propre de tous les GameObjects enfants.
                        for (int i = discardPileContainer.childCount - 1; i >= 0; i--)
                        {
                                Destroy(discardPileContainer.GetChild(i).gameObject);
                        }
                        discardPileModel.Clear(); // Vide aussi le modèle.
                }
        }
}