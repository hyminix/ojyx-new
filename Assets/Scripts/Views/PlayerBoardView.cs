// --- Views/PlayerBoardView.cs --- (Suppression du code de placement, géré par PlayerPositionManager)
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

        [Title("Emplacements du Plateau")]
        //On expose les slots au game manager
        [ShowInInspector, ReadOnly] public List<CardSlotController> cardSlots = new List<CardSlotController>();
    }
}