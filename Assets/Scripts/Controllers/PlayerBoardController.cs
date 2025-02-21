// --- Controllers/PlayerBoardController.cs --- (Ajout de RemoveCardsAndDiscard)

using UnityEngine;

using System.Collections.Generic;

using Sirenix.OdinInspector;

using com.hyminix.game.ojyx.Models;

using com.hyminix.game.ojyx.Views;

using com.hyminix.game.ojyx.Managers; //Pour GameManager



namespace com.hyminix.game.ojyx.Controllers

{

    [RequireComponent(typeof(PlayerBoardView))]

    public class PlayerBoardController : MonoBehaviour

    {

        [Title("Configuration du Plateau")]

        [SerializeField] public int columns = 4;

        [SerializeField] public int rows = 3;



        private PlayerBoard playerBoard;

        public PlayerBoard PlayerBoard => playerBoard;



        public PlayerBoardView playerBoardView;



        public void Initialize()

        {

            playerBoard = new PlayerBoard(columns, rows);

            playerBoardView = GetComponent<PlayerBoardView>();

            if (playerBoardView == null)

            {

                Debug.LogError("PlayerBoardView not found on " + gameObject.name);

                return;

            }

            GenerateBoardSlots();

        }



        [Button("Générer les Emplacements")]

        public void GenerateBoardSlots()

        {

            ClearBoard();



            foreach (CardSlot modelSlot in playerBoard.cardSlots)

            {

                Vector3 slotPos = playerBoardView.boardCenter.position + new Vector3(modelSlot.column * playerBoardView.slotSpacing.x, 0, -modelSlot.row * playerBoardView.slotSpacing.y);

                GameObject slotObj = Instantiate(playerBoardView.cardSlotPrefab, slotPos, Quaternion.identity, transform);

                CardSlotController slotController = slotObj.GetComponent<CardSlotController>();

                if (slotController == null)

                {

                    Debug.LogError("CardSlotController non trouvé sur le prefab du slot !");

                    continue;

                }



                slotController.Initialize(modelSlot);

                playerBoardView.cardSlots.Add(slotController);

            }

        }



        private void ClearBoard()

        {

            foreach (var slot in playerBoardView.cardSlots)

            {

                if (slot != null && slot.gameObject != null)

                {

                    Destroy(slot.gameObject);

                }

            }

            playerBoardView.cardSlots.Clear();



            if (playerBoard != null)

                playerBoard.Clear();

        }





        public bool AreAllCardsRevealed()

        {

            return playerBoard.AreAllCardsRevealed();

        }



        public CardController GetCardControllerAt(int row, int col)

        {

            var slot = playerBoardView.cardSlots.Find(s => s.cardSlot.row == row && s.cardSlot.column == col);

            return slot != null ? slot.CardController : null;

        }



        public void PlaceCardInitial(Card card, CardController cardController)

        {

            foreach (CardSlotController slotView in playerBoardView.cardSlots)

            {

                if (slotView.cardSlot.row == card.row && slotView.cardSlot.column == card.column)

                {

                    slotView.PlaceCard(cardController); // PlaceCard gère tout.

                    return;

                }

            }

            Debug.LogError($"Impossible de trouver le slot pour la carte à la position ({card.row}, {card.column})");

        }



        // AJOUT : Vérifie et supprime les lignes/colonnes complètes, et défausse les cartes.

        public void CheckBoard()

        {

            List<(int row, int col)> positionsToRemove = new List<(int row, int col)>();

            positionsToRemove.AddRange(playerBoard.CheckForCompletedColumns());

            positionsToRemove.AddRange(playerBoard.CheckForCompletedRows());



            if (positionsToRemove.Count > 0)

            {

                RemoveCardsAndDiscard(positionsToRemove);

            }

        }





        // AJOUT : Supprime les cartes de la vue et du modèle, et les défausse.

        public void RemoveCardsAndDiscard(List<(int row, int col)> positions)

        {

            List<Card> cardsToRemove = playerBoard.RemoveCardsAt(positions);



            foreach (Card card in cardsToRemove)

            {

                //Trouver le controller associé

                CardController cardController = GetCardControllerAt(card.row, card.column);



                if (cardController != null)

                {

                    GameManager.Instance.DeckController.DiscardCardWithAnimation(cardController, 0.5f, DG.Tweening.Ease.OutQuad);

                }

                else

                {

                    Debug.LogWarning("CardController non trouvé, la carte a peut-être déjà été retiré");

                }

            }

        }

    }

}
