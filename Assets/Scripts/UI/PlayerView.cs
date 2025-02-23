using UnityEngine;
using TMPro;
using System.Collections.Generic;
using com.hyminix.game.ojyx.Controllers;
using com.hyminix.game.ojyx.Models;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private GameObject cardPlaceholderPrefab; // Le prefab "CardPlaceholder"

    private List<GameObject> placeholders = new List<GameObject>();

    public void Initialize(PlayerController playerController)
    {
        // 1) Nom du joueur
        if (playerNameText != null)
        {
            playerNameText.text = "Joueur " + playerController.playerID;
        }

        // 2) Nettoyer d’éventuels placeholders précédents
        foreach (var go in placeholders)
        {
            if (go != null) Destroy(go);
        }
        placeholders.Clear();

        // 3) Récupérer le plateau du joueur
        var board = playerController.PlayerBoardController.PlayerBoard;
        if (board == null)
        {
            Debug.LogWarning("PlayerBoard is null for player " + playerController.playerID);
            return;
        }

        // 4) Pour chaque slot du plateau, on crée un placeholder
        //    (12 slots si 4 colonnes × 3 lignes)
        foreach (var slot in board.cardSlots)
        {
            // Instancier le prefab "CardPlaceholder" dans cardsContainer
            GameObject phGO = Instantiate(cardPlaceholderPrefab, cardsContainer);
            CardPlaceholderView phView = phGO.GetComponent<CardPlaceholderView>();

            if (slot.IsOccupied)
            {
                // Slot a une carte
                phView.SetCard(slot.card);
            }
            else
            {
                // Slot vide
                phView.SetEmpty();
            }

            placeholders.Add(phGO);
        }
    }
}
