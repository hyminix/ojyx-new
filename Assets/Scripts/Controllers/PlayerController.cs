// --- Controllers/PlayerController.cs ---

using UnityEngine;

using Sirenix.OdinInspector;

using com.hyminix.game.ojyx.Models;

using com.hyminix.game.ojyx.Managers;



namespace com.hyminix.game.ojyx.Controllers

{

    public class PlayerController : MonoBehaviour

    {

        [Title("Informations Joueur")]

        [ReadOnly, ShowInInspector]

        public int playerID;



        [Title("Composants")]

        [SerializeField, ReadOnly] private PlayerBoardController playerBoardController;  // Référence au *contrôleur* du plateau.

        public PlayerBoardController PlayerBoardController => playerBoardController; //Accesseur public



        [ShowInInspector, ReadOnly]

        private Player player; // Référence au modèle Player.

        public Player Player => player; //Accesseur public



        //Initialisation du joueur

        public void Initialize(int id)

        {

            playerID = id;

            player = new Player(playerID); // Crée le modèle Player.



            playerBoardController = GetComponent<PlayerBoardController>(); //On récupere le controller

            if (playerBoardController == null)

            {

                Debug.LogError("PlayerBoardController not found on " + gameObject.name);

                return;

            }

            playerBoardController.Initialize(); // Initialise le contrôleur du plateau.

        }

        //Fonction de début de tour

        public void StartTurn()

        {

            Debug.Log("Tour du joueur " + playerID + " démarré.");

        }



        //Fonction de fin de tour

        public void EndTurn()

        {

            Debug.Log("Tour du joueur " + playerID + " terminé.");

            player.revealedCardCount = 0; // Réinitialise le compteur de cartes révélées.

        }



        //Fonction pour vérifier si le tour est terminé

        public bool IsTurnComplete()

        {

            // TODO:  Implémenter la logique de fin de tour.

            return false;

        }



        //Fonction pour distribuer les cartes initiales

        public void DistributeInitialCards(DeckController deckController)

        {

            //On s'abonne a l'evenement

            DeckController.OnCardDrawnFromDeck += ReceiveInitialCard;

            //On pioche le nombre de carte requis

            for (int i = 0; i < playerBoardController.rows * playerBoardController.columns; i++)

            {

                deckController.DrawFromDeckInitial(); // Utilisation d'une nouvelle fonction

            }

            DeckController.OnCardDrawnFromDeck -= ReceiveInitialCard; //On se désabonne

        }



        //Fonction de reception des cartes initiales, appelé par l'evenement OnCardDrawFromDeck

        private void ReceiveInitialCard(Card card)

        {

            player.AddCardToHand(card);  // Ajoute la carte au modèle Player.

            //On place directement la carte, pas besoin du controller

            playerBoardController.PlaceCard(card); // Place la carte sur le plateau.

        }



        //Fonction pour révéler une carte

        public void RevealCard(CardController cardController)

        {

            if (player.RevealCard(cardController.Card)) //On revele la carte

            {

                cardController.Flip(); //On retourne visuellement la carte

            }

        }

        //Fonction pour défausser une carte

        public void DiscardCard(CardController cardController)

        {

            //Retire la carte de la main (si elle y est)

            player.RemoveCardFromHand(cardController.Card); // On utilise .Card ici

            GameManager.Instance.DeckController.DiscardCard(cardController.Card); // On utilise .Card ici

        }

    }

}
