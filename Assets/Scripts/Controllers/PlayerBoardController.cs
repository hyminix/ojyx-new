// --- Controllers/PlayerBoardController.cs ---

using UnityEngine;

using System.Collections.Generic;

using Sirenix.OdinInspector;

using com.hyminix.game.ojyx.Models;

using com.hyminix.game.ojyx.Views;

using com.hyminix.game.ojyx.Managers; // Assure-toi que ce using est présent





namespace com.hyminix.game.ojyx.Controllers

{

    [RequireComponent(typeof(PlayerBoardView))]

    public class PlayerBoardController : MonoBehaviour

    {

        [Title("Configuration du Plateau")]

        [SerializeField] public int columns = 4;

        [SerializeField] public int rows = 3;





        private PlayerBoard playerBoard;

        public PlayerBoardView playerBoardView;



        public void Initialize()

        {

            playerBoard = new PlayerBoard(columns, rows);

            playerBoardView = GetComponent<PlayerBoardView>();

            GenerateBoardSlots();

        }



        [Button("Générer les Emplacements")]

        public void GenerateBoardSlots()

        {

            ClearBoard();

            for (int row = 0; row < rows; row++)

            {

                for (int col = 0; col < columns; col++)

                {

                    Vector3 slotPos = playerBoardView.transform.position + new Vector3(col * playerBoardView.slotSpacing.x, 0, -row * playerBoardView.slotSpacing.y);

                    GameObject slotObj = Instantiate(playerBoardView.cardSlotPrefab, slotPos, Quaternion.identity, transform);

                    CardSlotController slot = slotObj.GetComponent<CardSlotController>();

                    slot.Initialize(row, col);

                    playerBoardView.cardSlots.Add(slot);

                }

            }

        }



        public bool PlaceCard(Card card)

        {

            //Trouve un slot de libre

            CardSlot slotToFill = null;

            foreach (var slot in playerBoard.cardSlots)

            {

                if (!slot.IsOccupied)

                {

                    slotToFill = slot;

                    break;

                }

            }



            //Si il n'y a plus de slot de libre

            if (slotToFill == null)

            {

                return false;

            }



            //Attribution des row/column a la carte

            card.SetPosition(slotToFill.row, slotToFill.column);

            //Place la carte dans le model

            bool placed = playerBoard.PlaceCard(card);

            if (placed)

            {

                //On cherche le slot correspondant, et on y place la carte

                foreach (var slot in playerBoardView.cardSlots)

                {

                    if (slot.cardSlot.row == slotToFill.row && slot.cardSlot.column == slotToFill.column) //On compare avec le model

                    {

                        //On utilise la fonction de création centralisé

                        CardController cardController = GameManager.Instance.DeckController.CreateCardController(card);

                        slot.PlaceCard(cardController);

                        break;

                    }

                }

            }

            return placed;

        }



        public CardController GetCardControllerAt(int row, int col)

        {

            foreach (var slot in playerBoardView.cardSlots)

            {

                if (slot.cardSlot.row == row && slot.cardSlot.column == col)

                {

                    return slot.GetComponentInChildren<CardController>();

                }

            }

            return null;

        }





        private void ClearBoard()

        {

            foreach (var slot in playerBoardView.cardSlots)

            {

                if (slot != null)

                    Destroy(slot.gameObject);

            }

            playerBoardView.cardSlots.Clear();

            if (playerBoard != null)

                playerBoard.Clear();

        }



        public bool AreAllCardsRevealed()

        {

            return playerBoard.AreAllCardsRevealed();

        }

    }

}
