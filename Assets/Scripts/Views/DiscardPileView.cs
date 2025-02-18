// --- Views/DiscardPileView.cs ---

using UnityEngine;

using Sirenix.OdinInspector;

using com.hyminix.game.ojyx.Controllers; //Pour accéder au controller



namespace com.hyminix.game.ojyx.Views

{

    public class DiscardPileView : MonoBehaviour

    {

        [SerializeField] private GameObject cardSlotPrefab; //Prefab du slot

        [SerializeField] public Transform discardCenter; //Le centre de la défausse

        [ShowInInspector, ReadOnly] private CardSlotController discardSlot; //Référence au controller



        //Initialisation

        private void Start()

        {

            GenerateDiscardSlot(); //On genere le slot

        }



        [Button("Générer le Slot de Défausse")]

        public void GenerateDiscardSlot()

        {

            //Si il existe, on le détruit

            if (discardSlot != null) Destroy(discardSlot.gameObject);

            GameObject slotObject = Instantiate(cardSlotPrefab, discardCenter.position, Quaternion.identity, transform); //On instancie

            discardSlot = slotObject.GetComponent<CardSlotController>();

            discardSlot.Initialize(0, 0); // Initialise le slot (les coordonnées n'ont pas d'importance ici).

        }



        //Fonction pour placer la carte dans la défausse

        public void PlaceCardInDiscard(CardController card)

        {

            if (discardSlot.RemoveCard() != null) //Si il y a deja une carte

            {

                // discardSlot.RemoveCard(); //On la supprime

            }

            discardSlot.PlaceCard(card); //On place la nouvelle

        }



        //Fonction pour piocher dans la défausse

        public CardController DrawFromDiscard()

        {

            //Si il y a une carte

            return discardSlot.RemoveCard();

        }



        //Fonction pour mettre en surbrillance

        public void SetHighlight(bool active)

        {

            if (discardSlot != null)

            {

                discardSlot.SetHighlight(active); //Appel la fonction du controlleur

            }

        }

        //Fonction pour vider la défausse

        public void Clear()

        {

            discardSlot.RemoveCard();

        }

    }

}
