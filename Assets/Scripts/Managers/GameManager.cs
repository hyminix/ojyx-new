// --- Managers/GameManager.cs --- (Modifié)

using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.States; // Assurez-vous que le namespace est correct



namespace com.hyminix.game.ojyx.Managers

{

    public class GameManager : MonoBehaviour

    {

        public static GameManager Instance;



        [Title("Références et Managers")]

        [SerializeField, ReadOnly] private DeckController deckController;

        public DeckController DeckController => deckController; //Accesseur public



        public List<PlayerController> players = new List<PlayerController>(); // Liste des *contrôleurs* de joueur.



        [Title("État du Jeu")]

        [ReadOnly, ShowInInspector]

        private IGameState currentState;  // L'état actuel du jeu.

        public IGameState CurrentState => currentState; //Accesseur public





        [Title("Debug")]

        [ReadOnly, ShowInInspector]

        public int currentPlayerIndex = 0; // Index du joueur actuel.



        public PlayerController CurrentPlayer => players[currentPlayerIndex];  // Le *contrôleur* du joueur actuel.

        private List<ClickDetector> addedClickDetectors = new List<ClickDetector>(); // Ajout : Liste des ClickDetector



        private void Awake()

        {

            if (Instance == null)

                Instance = this;

            else

                Destroy(gameObject);



            deckController = FindObjectOfType<DeckController>(); //On cherche le deck controller

        }



        private void Start()

        {

            //On initialise la pioche et la defausse

            deckController.InitializeDeck();

            TransitionToState(new SetupState()); // Commence par l'état Setup.

        }



        private void Update()

        {

            currentState?.ExecuteState(this); // Exécute l'état actuel.

        }



        //Fonction pour changer d'état

        public void TransitionToState(IGameState newState)

        {

            currentState?.ExitState(this);

            currentState = newState;

            currentState.EnterState(this);

        }



        //Fonction pour passer au joueur suivant

        public void NextPlayer()

        {

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count; //On incremente

            Debug.Log("Passage au joueur suivant : " + CurrentPlayer.playerID);



        }



        // Ajout: Méthode pour ajouter un ClickDetector et s'abonner à son événement

        public void AddClickListener(GameObject target, System.Action onClickAction)

        {

            if (target == null)

            {

                Debug.LogError("Target GameObject is null. Cannot add ClickDetector.");

                return;

            }



            ClickDetector clickDetector = target.AddComponent<ClickDetector>();

            clickDetector.OnClicked += onClickAction;

            addedClickDetectors.Add(clickDetector); // Garde la référence

        }



        // Ajout: Méthode pour supprimer *tous* les ClickDetector ajoutés.

        public void ClearClickListeners()

        {

            foreach (var detector in addedClickDetectors)

            {

                if (detector != null) // Vérification de sécurité

                {

                    Destroy(detector); // Utilisation de Destroy, car on est dans un MonoBehaviour

                }

            }

            addedClickDetectors.Clear(); // Vide la liste.

        }



        //Fonction de gestion des actions (remplace HandleCardAction)

        public void OnCardClicked(CardController cardController)

        {

            (currentState as ICardClickHandler)?.HandleCardClick(this, cardController);

        }

        public void OnCardDragStarted(CardController cardController)

        {

            (currentState as ICardDragStartHandler)?.HandleCardDragStart(this, cardController);

        }

        public void OnCardDragEnded(CardController cardController)

        {

            (currentState as ICardDragEndHandler)?.HandleCardDragEnd(this, cardController);

        }



        public void OnCardPlaced(CardSlotController cardSlotController)

        {

            (currentState as ICardPlacementHandler)?.HandleCardPlacement(this, cardSlotController);

        }

        public void OnCardRemoved(CardSlotController cardSlotController)

        {

            (currentState as ICardRemoveHandler)?.HandleCardRemove(this, cardSlotController);

        }



    }

}
