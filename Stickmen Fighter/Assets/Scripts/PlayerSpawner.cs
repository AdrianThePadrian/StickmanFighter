using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform player1Spawn;
    public Transform player2Spawn;

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // Check player index and set their spawn position accordingly
        if (playerInput.playerIndex == 0)
        {
            // Player 1
            playerInput.transform.position = player1Spawn.position;
            playerInput.transform.rotation = Quaternion.identity;
        }
        else if (playerInput.playerIndex == 1)
        {
            // Player 2
            playerInput.transform.position = player2Spawn.position;
            playerInput.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
