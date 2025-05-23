using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEditor;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance { get; private set; }

    [Header("Player Setup")]
    public GameObject playerPrefab;    // Your 3D model prefab with all components
    public RuntimeAnimatorController player1AnimatorController;
    public RuntimeAnimatorController player2AnimatorController;
    public BasicPlayerAnimatorSetup basicAnimatorSetup;

    [Header("References")]
    public GameManager gameManager;
    public Transform player1Spawn;
    public Transform player2Spawn;

    private HashSet<int> joinedPlayers = new HashSet<int>();
    private const int MAX_PLAYERS = 2;

    private PlayerInputManager inputManager;
    [SerializeField] private InputActionAsset inputActions;

    public bool useOverrideAnimations = false;  // Add this at the class level

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            inputManager = FindFirstObjectByType<PlayerInputManager>();
            if (inputManager == null)
            {
                inputManager = gameObject.AddComponent<PlayerInputManager>();
            }
            
            // Configure input manager
            inputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            inputManager.playerPrefab = playerPrefab;
            inputManager.enabled = true;
            inputManager.splitScreen = false;
            
            // Clear existing players
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

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (player1Spawn == null || player2Spawn == null)
        {
            Debug.LogError("Spawn points not set in PlayerSpawner!");
            return;
        }

        Debug.Log($"Player {playerInput.playerIndex} attempting to join.");

        // Validate player join
        if (playerInput.playerIndex < 0 || joinedPlayers.Count >= MAX_PLAYERS || 
            joinedPlayers.Contains(playerInput.playerIndex))
        {
            Debug.LogWarning($"Invalid join attempt for player {playerInput.playerIndex}. Destroying player.");
            Destroy(playerInput.gameObject);
            return;
        }

        joinedPlayers.Add(playerInput.playerIndex);
        Debug.Log($"Player {playerInput.playerIndex} successfully joined.");

        // Lock position until game starts
        Rigidbody rb = playerInput.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Setup components
        SetupPlayer(playerInput.gameObject);

        // Set position
        Transform spawnPoint = GetSpawnPoint(playerInput.playerIndex);
        playerInput.gameObject.transform.position = spawnPoint.position;
        playerInput.gameObject.transform.rotation = playerInput.playerIndex == 0 ? 
            Quaternion.Euler(0, 90, 0) : 
            Quaternion.Euler(0, -90, 0);

        // Verify position was set
        Debug.Log($"Player {playerInput.playerIndex} actual position after spawn: {playerInput.transform.position}");

        // Setup health
        PlayerHealth playerHealth = playerInput.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = playerInput.gameObject.AddComponent<PlayerHealth>();
        }
        gameManager.AssignHealthBarsToPlayers(playerHealth, playerInput.playerIndex);
    }

    private void SetupPlayer(GameObject player)
{
    PlayerInput playerInput = player.GetComponent<PlayerInput>();
    Animator animator = player.GetComponent<Animator>();
    
    if (animator != null)
    {
        // Setup base animator controller
        animator.runtimeAnimatorController = playerInput.playerIndex == 0 ? 
            player1AnimatorController : player2AnimatorController;
            
        // Setup custom animations
        SetupPlayerAnimations(animator, playerInput.playerIndex);
    }

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | 
                           RigidbodyConstraints.FreezeRotationZ | 
                           RigidbodyConstraints.FreezePositionY;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.useGravity = false;
        }
    }

    public Transform GetSpawnPoint(int playerIndex)
    {
        Debug.Log($"Getting spawn point for player {playerIndex}");
        Transform spawnPoint = playerIndex == 0 ? player1Spawn : player2Spawn;
        Debug.Log($"Spawn point position: {spawnPoint.position}");
        return spawnPoint;
    }

    public void SetupPlayerAnimations(Animator animator, int playerIndex)
    {
        if (!useOverrideAnimations)
        {
            animator.runtimeAnimatorController = playerIndex == 0 ? player1AnimatorController : player2AnimatorController;
            Debug.Log($"Using base animations for Player {playerIndex + 1}");
            return;
        }

        RuntimeAnimatorController baseController = playerIndex == 0 ? player1AnimatorController : player2AnimatorController;
        if (baseController == null)
        {
            Debug.LogError($"No base animator controller assigned for Player {playerIndex + 1}");
            return;
        }

        AnimatorOverrideController overrideController = new AnimatorOverrideController(baseController);
        string playerPrefix = playerIndex == 0 ? "P1" : "P2";
        
        // These animations will be loaded from custom recordings
        string[] customAnimationNames = { "HighAttack", "LowAttack", "Victory" };
        
        foreach (string animName in customAnimationNames)
        {
            string path = $"Animations/{playerPrefix}/{animName}";
            AnimationClip customAnim = Resources.Load<AnimationClip>(path);
            
            if (customAnim != null)
            {
                // Update the curve cleaning section
                EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(customAnim);
                List<EditorCurveBinding> bindingsToRemove = new List<EditorCurveBinding>();
                foreach (var binding in bindings)
                    {
                    bool isValidProperty = 
                        binding.propertyName.Contains("localPosition") ||
                        binding.propertyName.Contains("localRotation") ||
                        binding.propertyName.Contains("localScale") ||
                        binding.propertyName.StartsWith("RootT") ||
                        binding.propertyName.StartsWith("RootQ");
                        if (!isValidProperty)
                        {
                        bindingsToRemove.Add(binding);
                        }
                    }
                // Remove invalid curves in batch
                foreach (var binding in bindingsToRemove)
                {
                    AnimationUtility.SetEditorCurve(customAnim, binding, null);
                }

                
                // Find the matching base animation clip name
                string baseClipName = animName;
                overrideController[baseClipName] = customAnim;
                Debug.Log($"Successfully overrode {animName} for Player {playerIndex + 1}");
            }
            else
            {
                Debug.LogWarning($"Could not load custom {animName} for Player {playerIndex + 1}, using base animation");
            }
        }

        animator.runtimeAnimatorController = overrideController;
    }
}
