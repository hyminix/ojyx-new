// --- Managers/GameManager.cs --- (Utilisation de ExecuteAction)

using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.States;

using com.hyminix.game.ojyx.Enums;



namespace com.hyminix.game.ojyx.Managers

{

    public class GameManager : MonoBehaviour

    {

        public static GameManager Instance;



        [Title("Références et Managers")]

        [SerializeField, ReadOnly] private DeckController deckController;

        public DeckController DeckController => deckController;

        public List<PlayerController> players = new List<PlayerController>();



        [Title("État du Jeu")]

        [ReadOnly, ShowInInspector]

        private IGameState currentState;

        public IGameState CurrentState => currentState;



        [Title("Debug")]

        [ReadOnly, ShowInInspector]

        public int currentPlayerIndex = 0;

        public PlayerController CurrentPlayer => players[currentPlayerIndex];



        private void Awake()

        {

            if (Instance == null)

                Instance = this;

            else

                Destroy(gameObject);



            deckController = FindFirstObjectByType<DeckController>();



            PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();

            foreach (PlayerController pc in playerControllers)

            {

                pc.Initialize(players.Count);

                players.Add(pc);

            }

        }



        private void Start()

        {

            deckController.InitializeDeck();

            TransitionToState(new SetupState());

        }



        private void OnEnable()

        {

            // Plus besoin de s'abonner ici, les ClickHandlers appellent directement ExecuteAction.

        }



        private void OnDisable()

        {

            // Plus besoin de se désabonner.

        }



        private void Update()

        {

            currentState?.ExecuteState(this);

        }



        public void TransitionToState(IGameState newState)

        {

            currentState?.ExitState(this);

            currentState = newState;



            if (currentState != null)

            {

                Debug.Log("Current State: " + currentState.GetType().Name);

            }

            currentState?.EnterState(this);

        }



        public void NextPlayer()

        {

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            Debug.Log("Passage au joueur suivant : " + CurrentPlayer.playerID);

        }



        // MODIFICATION : Plus de HandleCardClicked, remplacé par ExecuteAction.

        public void ExecuteAction(IGameState currentState, PlayerAction action, CardSlotController cardSlot = null, CardController card = null)

        {

            currentState.ExecuteAction(this, action, cardSlot, card);

        }





        // Les méthodes DrawFromDeck, DrawFromDiscard, DiscardSelectedCard sont supprimées.

        // Elles sont remplacées par la logique dans les états eux-mêmes.

    }

}
