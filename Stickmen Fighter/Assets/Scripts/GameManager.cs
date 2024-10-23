using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private PlayerController player1;
    private PlayerController player2;
    public PlayerHealth playerHealth;

    private bool isRoundActive = false;

    public HealthBar player1HealthBar;
    public HealthBar player2HealthBar;

    

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

        player1.enabled = false;
        player2.enabled = false;

        ResetPlayers();

        StartCoroutine(StartCountdown());
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
        if (playerIndex == 0)
        {
            playerHealth.healthBar = player1HealthBar;
        }
        else if (playerIndex == 1)
        {
            playerHealth.healthBar = player2HealthBar;
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

        while(countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownDisplay.text = "Fight!";
        yield return new WaitForSeconds(1f);

        countdownDisplay.text = null;

        player1.enabled = true;
        player2.enabled = true;

        isRoundActive = true;
    }

    public void OnPlayerDefeated(PlayerController defeatedPlayer)
    {
        if (!isRoundActive) return;

        if(defeatedPlayer == player1)
        {
            player2Wins++;
        }
        else if (defeatedPlayer == player2)
        {
            player1Wins++;
        }

        isRoundActive = false;

        if (player1Wins == totalRounds || player2Wins == totalRounds)
        {
            EndGame();
        }
        else
        {
            currentRound++;
            StartRound();
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
    }


}
