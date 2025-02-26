using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using com.hyminix.game.ojyx.Managers;
using com.hyminix.game.ojyx.Controllers;

public class PlayerPositionManager : MonoBehaviour
{
    [Title("Ancrages disponibles (8 maximum)")]
    [SerializeField] public Transform[] playerAnchors;
    // 0..7 dans la scène

    // Le dictionnaire : pour chaque nb de joueurs, quels anchors on veut utiliser
    private Dictionary<int, int[]> anchorMap = new Dictionary<int, int[]>
    {
        { 2, new int[] { 0, 4 } },
        { 3, new int[] { 0, 2, 4 } },
        { 4, new int[] { 0, 2, 4, 6 } },
        { 5, new int[] { 0, 1, 3, 5, 7 } },
        { 6, new int[] { 0, 1, 3, 4, 5, 7 } },
        { 7, new int[] { 0, 1, 2, 3, 4, 5, 7 } },
        { 8, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 } },
    };

    /// <summary>
    /// Place chaque joueur sur un anchor différent,
    /// de sorte que le joueur actif soit toujours anchor0.
    /// </summary>
    public void UpdatePlayerPositions(int currentPlayerIndex, int numberOfPlayers)
    {
        if (!anchorMap.ContainsKey(numberOfPlayers))
        {
            Debug.LogError($"[PlayerPositionManager] Pas de mapping pour {numberOfPlayers} joueurs !");
            return;
        }
        int[] anchorsForThisSetup = anchorMap[numberOfPlayers];

        if (anchorsForThisSetup.Length < numberOfPlayers)
        {
            Debug.LogError($"[PlayerPositionManager] anchorMap pour {numberOfPlayers} joueurs est trop court !");
            return;
        }

        // Utilisation de la soustraction circulaire pour inverser l'ordre
        for (int i = 0; i < numberOfPlayers; i++)
        {
            int playerIndex = (currentPlayerIndex - i + numberOfPlayers) % numberOfPlayers;
            PlayerController player = GameManager.Instance.players[playerIndex];

            if (player == null)
            {
                Debug.LogWarning($"[PlayerPositionManager] PlayerController manquant pour index {playerIndex}");
                continue;
            }

            int anchorIndex = anchorsForThisSetup[i];
            Transform anchor = playerAnchors[anchorIndex];

            // Détacher et positionner le joueur sur l'ancrage
            player.transform.SetParent(null);
            player.transform.position = anchor.position;
            // On laisse la rotation telle quelle pour ne pas faire tourner le plateau
        }
    }

}
