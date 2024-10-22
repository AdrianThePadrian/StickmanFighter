using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;


public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;  //Rigidbody component
    public float moveSpeed = 5f;  //Movement Speed
    public int highAttackDamage = 1;  // Damage dealt by the high attack
    public int lowAttackDamage = 1;   // Damage dealt by the low attack
    public Transform highAttackPoint;   // Point from where the high attack will be cast
    public Transform lowAttackPoint;    // Point from where the low attack will be cast
    public float highAttackRange = 1f;  // Range of the high attack
    public float lowAttackRange = 0.5f; // Range of the low attack
    public LayerMask playerLayer;       // Layer for detecting the other player
    public float knockbackForce = 5f;   // Knockback force applied when hit
    private int playerIndex;

    private bool isHurt = false;

    // Cooldown settings
    public float highAttackCooldown = 1f; // Cooldown duration for high attack
    public float lowAttackCooldown = 0.5f; // Cooldown duration for low attack

    private bool canAttack = true; // Track if player can attack
    private float attackTimer; // Timer to track cooldown

    //Sprites
    public Sprite idleSprite;
    public Sprite highAttackSprite;
    public Sprite lowAttackSprite;
    public Sprite hurtSprite;
    public Sprite victorySprite;
    public Sprite defeatSprite;

    public SpriteRenderer spriteRenderer;
    public PlayerHealth playerHealth;



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
        playerIndex = GetComponent<PlayerInput>().playerIndex;
        spriteRenderer.sprite = idleSprite; // Set initial sprite to idle
        rb = GetComponent<Rigidbody2D>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component is missing on " + gameObject.name);
        }
    }

    private void Update()
    {
        // Update the attack timer if not ready to attack
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                canAttack = true;
                Debug.Log("Player can attack again.");
            }
        }
    }



    // This method is called by the Input System when the "Move" action is triggered
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // High attack input from the new input system
    public void OnHighAttack(InputValue value)
    {
        if (value.isPressed && canAttack && !isHurt)
        {
            Debug.Log("High Attack Triggered");
            Attack(true); // true for high attack
            spriteRenderer.sprite = highAttackSprite; // Change Sprite
            Invoke("ResetToIdle", 0.5f); // Reset Sprite
        }
    }

    // Low attack input from the new input system
    public void OnLowAttack(InputValue value)
    {
        if (value.isPressed && canAttack && !isHurt)
        {
            Debug.Log("Low Attack Triggered");
            Attack(false); // false for low attack
            spriteRenderer.sprite = lowAttackSprite; // Change Sprite
            Invoke("ResetToIdle", 0.5f); // Reset Sprite
        }
    }

    // This method is called by the Input System when the "Dodge" action is triggered
    public void OnDodge(InputValue value)
    {
        if (value.isPressed)
        {
            // Implement dodge logic here
            StartCoroutine(Dodge());
        }
    }

    private void FixedUpdate()
    {
        // Move the player on the X axis using the input
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
    }

    // Method to handle attacking the other player
    private void Attack(bool isHighAttack)
    {
        Transform attackPoint = isHighAttack ? highAttackPoint : lowAttackPoint;
        float attackRange = isHighAttack ? highAttackRange : lowAttackRange;
        int damage = isHighAttack ? highAttackDamage : lowAttackDamage;

        // Detect players in range of the attack
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        Debug.Log($"Attack Point: {attackPoint.position}, Attack Range: {attackRange}, Hit Players: {hitPlayers.Length}");

        // Damage and knockback each player hit
        foreach (Collider2D player in hitPlayers)
        {
            Debug.Log("Hit Player: " + player.name); // Log the hit player

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            Rigidbody2D otherPlayerRb = player.GetComponent<Rigidbody2D>();

            if (playerHealth != null && otherPlayerRb != null)
            {
                Debug.Log($"Applying {damage} damage to {player.name}");

                // Determine if it's a dodge scenario
                if (isHighAttack)
                {
                    // If this is a high attack, check if the other player is dodging (low attack)
                    PlayerController otherPlayerController = player.GetComponent<PlayerController>();
                    if (otherPlayerController.IsDodging())
                    {
                        Debug.Log(player.name + " dodged the high attack!");
                        return; // The attack misses due to dodge
                    }
                }

                // Apply damage to the player
                playerHealth.TakeDamage(damage);

                if (!isHurt)
                {
                    isHurt = true;
                    spriteRenderer.sprite = hurtSprite;
                    Invoke("ResetToIdle", 0.5f);
                }
                
                // Apply knockback to the hit player
                Vector2 knockbackDirection = (otherPlayerRb.transform.position - transform.position).normalized;
                otherPlayerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

                // Apply knockback to the attacker (this player)
                Vector2 selfKnockbackDirection = (transform.position - otherPlayerRb.transform.position).normalized;
                rb.AddForce(selfKnockbackDirection * knockbackForce, ForceMode2D.Impulse);

                Debug.DrawLine(otherPlayerRb.position, otherPlayerRb.position + knockbackDirection * knockbackForce, Color.red, 2f);
                Debug.Log("Knockback applied to: " + player.name + " with force: " + knockbackDirection * knockbackForce);
            }
        }

        // Set attack cooldown
        canAttack = false;
        attackTimer = isHighAttack ? highAttackCooldown : lowAttackCooldown; // Set cooldown duration based on attack type
    }

    // Method to check if the player is dodging
    public bool IsDodging()
    {
        return false; // Placeholder, replace with actual dodge logic
    }

    // Example dodge coroutine
    private IEnumerator Dodge()
    {
        yield return new WaitForSeconds(0.5f); // Duration of the dodge
    }
    public void ResetPlayer()
    {
        Transform spawnPoint = PlayerSpawner.instance.GetSpawnPoint(playerIndex);
        if(spawnPoint != null) 
        {
            transform.position = spawnPoint.position;
        }

        playerHealth.ResetHealth();
        spriteRenderer.sprite = idleSprite;
    }
    void ResetToIdle()
    {
        isHurt = false;
        spriteRenderer.sprite = idleSprite;
    }

    // Draw the attack range in the editor for visualization
    private void OnDrawGizmosSelected()
    {
        if (highAttackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(highAttackPoint.position, highAttackRange);
        }
        if (lowAttackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(lowAttackPoint.position, lowAttackRange);
        }
    }
}
