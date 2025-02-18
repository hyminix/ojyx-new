// --- Views/PlayerBoardView.cs ---

using UnityEngine;

using System.Collections.Generic;

using Sirenix.OdinInspector;

using com.hyminix.game.ojyx.Controllers; //Pour accéder au controller

using com.hyminix.game.ojyx.Models;



namespace com.hyminix.game.ojyx.Views

{

    public class PlayerBoardView : MonoBehaviour

    {

        [Title("Configuration du Plateau")]

        [SerializeField] public GameObject cardSlotPrefab; // Prefab du slot de carte

        [SerializeField] public Transform boardCenter;

        [SerializeField] public Vector2 slotSpacing = new Vector2(2f, 2.5f); // Espacement



        [Title("Emplacements du Plateau")]

        //On expose les slots au game manager

        [ShowInInspector, ReadOnly] public List<CardSlotController> cardSlots = new List<CardSlotController>();



    }

}
