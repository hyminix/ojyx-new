// --- Controllers/CardSlotController.cs ---

using UnityEngine;

using com.hyminix.game.ojyx.Models;

using com.hyminix.game.ojyx.Views;



namespace com.hyminix.game.ojyx.Controllers

{

    [RequireComponent(typeof(CardSlotView))] //Assure que le controller est sur le même objet que la vue

    public class CardSlotController : MonoBehaviour

    {

        public delegate void CardSlotAction(CardSlotController slotController);

        public event CardSlotAction OnCardPlaced;

        public event CardSlotAction OnCardRemoved;



        public CardSlot cardSlot; //Accesseur public

        private CardSlotView cardSlotView; // Reference à la vue



        //Initialisation du slot

        public CardSlotController Initialize(int row, int column)

        {

            cardSlot = new CardSlot(row, column); // Initialisation du modèle

            cardSlotView = GetComponent<CardSlotView>(); //On récupere la vue

            cardSlotView.Initialize(cardSlot); // Initialisation de la vue

            return this; //On retourne le controller

        }



        //Fonction pour placer une carte

        public void PlaceCard(CardController cardController)

        {

            if (cardSlot.IsOccupied) //Si le slot est deja occupé

            {

                Debug.LogWarning("CardSlot is already occupied. Cannot place card.");

                return; //On ne fait rien

            }



            //On déplace la carte

            cardController.transform.position = transform.position + new Vector3(0, 0.1f, 0); // Décalage vertical

            cardController.transform.SetParent(transform); // Garder l'organisation dans la hiérarchie

            //On ajoute la carte

            cardSlot.PlaceCard(cardController.Card); // Utilise la propriété Card

            cardSlotView.UpdateVisual(); // Met à jour l'apparence

            OnCardPlaced?.Invoke(this); //Déclenche l'évenement

        }



        //Fonction pour supprimer une carte

        public CardController RemoveCard()

        {

            if (!cardSlot.IsOccupied)

            {

                return null; // Slot déjà vide

            }

            CardController card = GetComponentInChildren<CardController>();//Récupere le controller

            if (card != null)

            {

                card.transform.SetParent(null); // Détache la carte

                cardSlot.RemoveCard(); //Supprime la carte du model

                cardSlotView.UpdateVisual(); // Met à jour la vue

                OnCardRemoved?.Invoke(this); //Déclenche l'évenement

                return card;

            }

            return null;



        }

        //Fonction de hightlight du slot

        public void SetHighlight(bool active)

        {

            cardSlotView.SetHighlight(active); //Appel de la fonction de la vue

        }

    }

}
