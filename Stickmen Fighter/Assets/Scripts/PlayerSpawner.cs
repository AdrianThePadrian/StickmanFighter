using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    public static PlayerSpawner instance { get; private set; }

    public Transform player1Spawn;
    public Transform player2Spawn;

    // Player 1's sprites
    public Sprite player1IdleSprite;
    public Sprite player1HighAttackSprite;
    public Sprite player1LowAttackSprite;
    public Sprite player1HurtSprite;
    public Sprite player1Victory;
    public Sprite player1Defeat;

    // Player 2's sprites
    public Sprite player2IdleSprite;
    public Sprite player2HighAttackSprite;
    public Sprite player2LowAttackSprite;
    public Sprite player2HurtSprite;
    public Sprite player2Victory;
    public Sprite player2Defeat;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // Check player index and set their spawn position accordingly
        if (playerInput.playerIndex == 0)
        {
            // Player 1
            playerInput.transform.position = player1Spawn.position;
            AssignPlayerSprites(playerInput.gameObject, player1IdleSprite, player1HighAttackSprite, player1LowAttackSprite, player1HurtSprite, player1Victory, player1Defeat);
            playerInput.transform.rotation = Quaternion.identity;
        }
        else if (playerInput.playerIndex == 1)
        {
            // Player 2
            playerInput.transform.position = player2Spawn.position;
            AssignPlayerSprites(playerInput.gameObject, player2IdleSprite, player2HighAttackSprite, player2LowAttackSprite, player2HurtSprite, player2Victory, player2Defeat);
            playerInput.transform.rotation = Quaternion.Euler(0, 180, 0);

        }
    }
    private void ScalePlayerToCollider(GameObject player)
    {
        // Get the capsule collider component from the player
        CapsuleCollider2D capsuleCollider = player.GetComponent<CapsuleCollider2D>();
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();

        if (capsuleCollider != null && spriteRenderer != null)
        {
            // Get the radius and height of the capsule collider
            float radius = capsuleCollider.size.x * 0.5f; // Capsule width is twice the radius
            float height = capsuleCollider.size.y;

            // Get the dimensions of the sprite in units
            float spriteWidthInUnits = spriteRenderer.bounds.size.x; // Width of the sprite
            float spriteHeightInUnits = spriteRenderer.bounds.size.y; // Height of the sprite

            // Calculate scale factors
            float scaleX = (2 * radius) / spriteWidthInUnits; // Scale based on the collider radius and sprite width
            float scaleY = height / spriteHeightInUnits; // Scale based on the collider height and sprite height

            // Apply scaling
            player.transform.localScale = new Vector3(scaleX, scaleY, 1);
        }
        else
        {
            Debug.LogWarning("Player does not have a CapsuleCollider2D or SpriteRenderer component.");
        }
    }

    private void AssignPlayerSprites (GameObject player, Sprite idle, Sprite highAttack, Sprite lowAttack, Sprite hurt, Sprite victory, Sprite defeat) 
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerController != null)
        {
            playerController.idleSprite = idle;
            playerController.highAttackSprite = highAttack;
            playerController.lowAttackSprite = lowAttack;
            playerController.hurtSprite = hurt;
            playerController.victorySprite = victory;

            
        }
    }

    public Transform GetSpawnPoint(int playerIndex)
    {
        if (playerIndex == 0)
        {
            return player1Spawn;
        }
        else if (playerIndex == 1)
        {
            return player2Spawn;
        }

        return null;
    }

}
