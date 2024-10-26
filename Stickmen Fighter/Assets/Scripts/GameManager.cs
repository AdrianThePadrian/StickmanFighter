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
    public float roundTransitionDelay = 2f;

    private PlayerController player1;
    private PlayerController player2;
    public PlayerHealth playerHealth;

    private bool isRoundActive = false;

    //UI
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

        ResetPlayers();
        AssignHealthBarsToPlayers(player1.GetComponent<PlayerHealth>(), 0);
        AssignHealthBarsToPlayers(player2.GetComponent<PlayerHealth>(), 1);

        StartCoroutine(StartCountdown());
    }

    private IEnumerator RoundTransition()
    {
        // stop players from moving
        player1.enabled = false;
        player2.enabled = false;

        // Pause for a few seconds to display victory and defeat poses
        yield return new WaitForSeconds(3f);

        // Reset the round, players, and health for the next round
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
            player1.enabled = false;
            player2.enabled = false;
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

        while(countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownDisplay.text = "Fight!";

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
            EndRound(winningPlayerIndex: 1);
        }
        else if (defeatedPlayer == player2)
        {
            EndRound(winningPlayerIndex: 0);
        }

        isRoundActive = false;
    }

    private void EndRound(int winningPlayerIndex)
    {
        if (winningPlayerIndex == 0)
        {
            player1Wins++;
            UpdateWinIcons(player1WinIcons, player1Wins);
            player1.SetVictoryPose();
            player2.SetDefeatPose();

            if (player1Wins >= totalRounds)
            {
                player1.enabled = false;
                player2.enabled = false;

                Debug.Log("Player 1 Wins the Game!");
                EndGame();
            }
            else
            {
                StartCoroutine(RoundTransition());
            }
        }
        else if (winningPlayerIndex == 1)
        {
            player2Wins++;
            UpdateWinIcons(player2WinIcons, player2Wins);
            player2.SetVictoryPose();
            player1.SetDefeatPose();

            if (player2Wins >= totalRounds)
            {
                player1.enabled = false;
                player2.enabled = false;

                Debug.Log("Player 2 Wins the Game!");
                EndGame();
            }
            else
            {
                StartCoroutine(RoundTransition());
            }
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
    }


}
