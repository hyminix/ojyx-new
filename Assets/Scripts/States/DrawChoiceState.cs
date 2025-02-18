// --- States/DrawChoiceState.cs ---

using UnityEngine;

using com.hyminix.game.ojyx.Controllers;

using com.hyminix.game.ojyx.Managers;

using UnityEngine.EventSystems;



namespace com.hyminix.game.ojyx.States

{

    public class DrawChoiceState : IGameState, IPointerClickHandler

    {

        private DeckController deckController;

        private GameManager gameManager;



        public void EnterState(GameManager manager)

        {

            Debug.Log("DrawChoiceState: Choisissez entre piocher ou prendre la défausse.");

            deckController = manager.DeckController;

            this.gameManager = manager;

            // Activer l'UI pour le choix (boutons, etc.).  Pour l'instant, on utilise des clics.

            SubscribeToEvents();

        }



        public void ExecuteState(GameManager manager) { }



        public void ExitState(GameManager manager)

        {

            UnsubscribeFromEvents();

        }



        public void OnPointerClick(PointerEventData eventData)

        {



            //Si on clique sur la pioche

            if (eventData.pointerPressRaycast.gameObject == deckController.deckPosition.gameObject)

            {

                Debug.Log("Pioche une carte");

                GameManager.Instance.TransitionToState(new DrawFromDeckState()); //Transition vers l'état de pioche

            }

            //Si on clique sur la défausse

            else if (eventData.pointerPressRaycast.gameObject == deckController.discardPileView.discardCenter.gameObject)

            {

                //Si la défausse n'est pas vide

                if (deckController.discardPileView.DrawFromDiscard() != null)

                {

                    Debug.Log("Pioche une carte de la défausse");

                    GameManager.Instance.TransitionToState(new DrawFromDiscardState()); //Transition vers l'état de pioche de la défausse

                }

                else

                {

                    Debug.Log("La défausse est vide, impossible de piocher");

                }

            }

        }

        private void SubscribeToEvents()

        {

            //On s'abonne a l'evenement de clic sur la pioche

            gameManager.AddClickListener(deckController.deckPosition.gameObject, HandleDeckClick);

            gameManager.AddClickListener(deckController.discardPileView.discardCenter.gameObject, HandleDiscardClick);

        }



        private void UnsubscribeFromEvents()

        {

            //On se désabonne

            gameManager.ClearClickListeners();

        }



        //Fonction pour gérer le clic sur la pioche

        private void HandleDeckClick()

        {

            GameManager.Instance.TransitionToState(new DrawFromDeckState()); //Transition vers l'état de pioche

        }

        //Fonction pour gérer le clic sur la défausse

        private void HandleDiscardClick()

        {

            GameManager.Instance.TransitionToState(new DrawFromDiscardState()); //Transition vers l'état de pioche de la défausse

        }

    }



    // Classe utilitaire pour simplifier la détection des clics (à mettre dans un fichier séparé si tu veux)

    public class ClickDetector : MonoBehaviour, IPointerClickHandler

    {

        public System.Action OnClicked; //Evenement



        public void OnPointerClick(PointerEventData eventData)

        {

            OnClicked?.Invoke();

        }

    }

}

