// --- Views/DiscardPileView.cs --- (Correction de DrawTopCardController et AddCardToDiscardPile)

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



                        // 1. Attacher la carte à la vue *AVANT* d'ajouter au modèle.

                        cardController.transform.SetParent(discardPileContainer);

                        cardController.transform.localPosition = Vector3.zero; // Position relative au parent.

                        cardController.transform.localRotation = Quaternion.identity;



                        // 2.  Ajouter la carte au modèle *APRÈS* l'avoir attachée à la vue.

                        discardPileModel.AddCard(cardController.Card);



                        // 3. Ajuster la position Z *après* l'ajout, en utilisant discardPileModel.cards.Count.

                        cardController.transform.localPosition = new Vector3(0, 0, -offsetZ * (discardPileModel.cards.Count - 1));



                        Debug.Log($"DiscardPileView: Ajout de la carte {cardController.Card.Data.value} en position {discardPileModel.cards.Count} de la pile.");

                }





                public CardController DrawTopCardController()

                {

                        if (discardPileModel == null || discardPileModel.cards.Count == 0)

                        {

                                Debug.LogWarning("DiscardPileView: La défausse est vide !");

                                return null;

                        }



                        // 1. Récupérer le CardController *AVANT* de modifier le modèle.

                        CardController topCardController = GetTopCardController();



                        if (topCardController == null)

                        {

                                Debug.LogError("DiscardPileView: Impossible de trouver le CardController du dessus !");

                                return null;

                        }



                        // 2. Retirer la carte du modèle.

                        discardPileModel.DrawCard();



                        // 3. Détacher la carte de la vue.

                        topCardController.transform.SetParent(null);



                        Debug.Log($"DiscardPileView: Carte {topCardController.Card.Data.value} retirée de la défausse.");

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
