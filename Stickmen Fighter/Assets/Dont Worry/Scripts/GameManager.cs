using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text roundDisplay;
    public Text countdownDisplay;

    private int currentRound = 1;
    private int player1Wins = 0;
    private int player2Wins = 0;
    public int totalRounds = 3;
    public float roundTransitionDelay = 2f;

    private PlayerController player1;
    private PlayerController player2;
    public PlayerHealth playerHealth;

    private bool isRoundActive = false;

    // UI
    public HealthBar player1HealthBar;
    public HealthBar player2HealthBar;
    public GameObject[] player1WinIcons;
    public GameObject[] player2WinIcons;


    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();

        player1HealthBar.Initialize(3);
        player2HealthBar.Initialize(3);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void StartRound()
    {
        roundDisplay.text = "Round " + currentRound;

        AssignHealthBarsToPlayers(player1.GetComponent<PlayerHealth>(), 0);
        AssignHealthBarsToPlayers(player2.GetComponent<PlayerHealth>(), 1);

        StartCoroutine(StartCountdown());
    }

    private IEnumerator RoundTransition()
    {
        isRoundActive = false;
        
        // stop players from moving
        if (player1 != null)
        {
            player1.enabled = false;
            
            player1.SetVictoryPose();
        }
        
        if (player2 != null)
        {
            player2.enabled = false;
            
            player2.SetDefeatPose();
        }

        // Pause for a few seconds to display victory and defeat poses
        yield return new WaitForSeconds(3f);

        player1.ResetPlayer();
        player2.ResetPlayer();

        currentRound++;
        StartRound();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // Assign players based on their index
        if (playerInput.playerIndex == 0)
        {
            player1 = playerInput.GetComponent<PlayerController>();
        }
        else if (playerInput.playerIndex == 1)
        {
            player2 = playerInput.GetComponent<PlayerController>();
        }

        // If both players have joined, start the round
        if (player1 != null && player2 != null)
        {
            StartRound();
        }
    }

    public void AssignHealthBarsToPlayers(PlayerHealth playerHealth, int playerIndex)
    {
        if (playerHealth != null)
        {
            // Assign health bar based on player index
            if (playerIndex == 0)
            {
                playerHealth.healthBar = player1HealthBar;
                Debug.Log("Assigned Player 1's health bar.");
            }
            else if (playerIndex == 1)
            {
                playerHealth.healthBar = player2HealthBar;
                Debug.Log("Assigned Player 2's health bar.");
            }
        }
        else
        {
            Debug.LogError("PlayerHealth is null! Cannot assign health bar.");
        }
    }

    private void ResetPlayers()
    {
        if (player1 != null && player1.playerHealth != null)
        {
            player1.ResetPlayer();
        }
        else
        {
            Debug.LogError("Player 1's health component is not set.");
        }

        if (player2 != null && player2.playerHealth != null)
        {
            player2.ResetPlayer();
        }
        else
        {
            Debug.LogError("Player 2's health component is not set.");
        }
    }

    IEnumerator StartCountdown()
    {
        int countdownTime = 3;

        // Make sure players are disabled but components are ready
        if (player1 != null)
        {
            player1.enabled = false;  // This disables the PlayerController
            Rigidbody rb1 = player1.GetComponent<Rigidbody>();
            if (rb1 != null) 
            {
                rb1.isKinematic = true;
                rb1.linearVelocity = Vector3.zero;
            }
        }
        
        if (player2 != null)
        {
            player2.enabled = false;  // This disables the PlayerController
            Rigidbody rb2 = player2.GetComponent<Rigidbody>();
            if (rb2 != null) 
            {
                rb2.isKinematic = true;
                rb2.linearVelocity = Vector3.zero;
            }
        }

        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownDisplay.text = "Fight!";
        yield return new WaitForSeconds(1f);
        countdownDisplay.text = null;

        // Re-enable players
        if (player1 != null)
        {
            player1.enabled = true;  // This enables the PlayerController
            Rigidbody rb1 = player1.GetComponent<Rigidbody>();
            if (rb1 != null) 
            {
                rb1.isKinematic = false;
                rb1.WakeUp();
            }
        }
        
        if (player2 != null)
        {
            player2.enabled = true;  // This enables the PlayerController
            Rigidbody rb2 = player2.GetComponent<Rigidbody>();
            if (rb2 != null) 
            {
                rb2.isKinematic = false;
                rb2.WakeUp();
            }
        }

        isRoundActive = true;
    }

    public void OnPlayerDefeated(PlayerController defeatedPlayer)
    {
        if (!isRoundActive) return;

        if (defeatedPlayer == player1)
        {
            EndRound(winningPlayerIndex: 1);
        }
        else if (defeatedPlayer == player2)
        {
            EndRound(winningPlayerIndex: 0);
        }
    }

    private void EndRound(int winningPlayerIndex)
    {
        isRoundActive = false;

        if (winningPlayerIndex == 0)
        {
            player1Wins++;
            UpdateWinIcons(player1WinIcons, player1Wins);

            player1.SetVictoryPose();
            player2.SetDefeatPose();
        }
        else
        {
            player2Wins++;
            UpdateWinIcons(player2WinIcons, player2Wins);

            player1.SetDefeatPose();
            player2.SetVictoryPose();
        }

        if (player1Wins >= totalRounds || player2Wins >= totalRounds)
        {
            EndGame();
        }
        else
        {
            StartCoroutine(RoundTransition());
        }
    }

    private void UpdateWinIcons(GameObject[] winIcons, int winCount)
    {
        for (int i = 0; i < winIcons.Length; i++)
        {
            winIcons[i].SetActive(i < winCount);
        }
    }

    private void EndGame()
    {
        if (player1Wins > player2Wins)
        {
            roundDisplay.text = "Player 1 Wins!";
        }
        else
        {
            roundDisplay.text = "Player 2 Wins!";
        }

        // stop players from moving
        if (player1 != null)
        {
            player1.enabled = false;
            
            player1.SetVictoryPose();
        }
        
        if (player2 != null)
        {
            player2.enabled = false;
            
            player2.SetDefeatPose();
        }
    }
}
