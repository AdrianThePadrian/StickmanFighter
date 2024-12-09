using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public RuntimeAnimatorController player1AnimatorController;
    public RuntimeAnimatorController player2AnimatorController;
    public GameObject player1Model;
    public GameObject player2Model;

    public static PlayerSpawner instance { get; private set; }

    public GameManager gameManager;

    public Transform player1Spawn;
    public Transform player2Spawn;

    private HashSet<int> joinedPlayers = new HashSet<int>();
    private const int MAX_PLAYERS = 2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PlayerInput[] existingPlayers = FindObjectsOfType<PlayerInput>();
            foreach (PlayerInput player in existingPlayers)
            {
                Destroy(player.gameObject);
            }
            joinedPlayers.Clear();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Remove the event unsubscribe since we're using SendMessages
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (joinedPlayers.Contains(playerInput.playerIndex))
        {
            Debug.LogWarning($"Player {playerInput.playerIndex} already exists!");
            Destroy(playerInput.gameObject);
            return;
        }

        if (joinedPlayers.Count >= MAX_PLAYERS)
        {
            Debug.LogWarning("Maximum players reached!");
            Destroy(playerInput.gameObject);
            return;
        }

        joinedPlayers.Add(playerInput.playerIndex);
        
        GameObject player = playerInput.gameObject;
        playerInput.notificationBehavior = PlayerNotifications.SendMessages;
        
        Transform spawnPoint = GetSpawnPoint(playerInput.playerIndex);
        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            player.transform.rotation = playerInput.playerIndex == 0 ? 
                Quaternion.identity : Quaternion.Euler(0, 180, 0);
        }

        if (playerInput.playerIndex == 0)
        {
            SetupPlayerModel(player, player1Model, player1AnimatorController);
        }
        else if (playerInput.playerIndex == 1)
        {
            SetupPlayerModel(player, player2Model, player2AnimatorController);
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        gameManager.AssignHealthBarsToPlayers(playerHealth, playerInput.playerIndex);
    }

    private void SetupPlayerModel(GameObject player, GameObject modelPrefab, RuntimeAnimatorController animatorController)
    {
        Transform existingModel = player.transform.Find("PlayerModel");
        if (existingModel != null)
        {
            Destroy(existingModel.gameObject);
        }

        GameObject newModel = Instantiate(modelPrefab, player.transform);
        newModel.name = "PlayerModel";

        Animator animator = newModel.GetComponent<Animator>();
        if (animator == null)
        {
            animator = newModel.AddComponent<Animator>();
        }
        animator.runtimeAnimatorController = animatorController;

        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.InitializeModel(newModel);
        }
    }

    public Transform GetSpawnPoint(int playerIndex)
    {
        return playerIndex == 0 ? player1Spawn : player2Spawn;
    }

}
