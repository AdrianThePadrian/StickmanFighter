using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public int highAttackDamage = 1;  // Damage dealt by the high attack
    public int lowAttackDamage = 1;   // Damage dealt by the low attack
    public Transform highAttackPoint;   // Point from where the high attack will be cast
    public Transform lowAttackPoint;    // Point from where the low attack will be cast
    public float highAttackRange = 1f;  // Range of the high attack
    public float lowAttackRange = 0.5f; // Range of the low attack
    public LayerMask playerLayer;       // Layer for detecting the other player
    public float knockbackForce = 5f;   // Knockback force applied when hit

    // Cooldown settings
    public float highAttackCooldown = 1f; // Cooldown duration for high attack
    public float lowAttackCooldown = 0.5f; // Cooldown duration for low attack

    private bool canAttack = true; // Track if player can attack
    private float attackTimer; // Timer to track cooldown

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (value.isPressed && canAttack)
        {
            Debug.Log("High Attack Triggered");
            Attack(true); // true for high attack
        }
    }

    // Low attack input from the new input system
    public void OnLowAttack(InputValue value)
    {
        if (value.isPressed && canAttack)
        {
            Debug.Log("Low Attack Triggered");
            Attack(false); // false for low attack
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

                // Apply knockback to the hit player
                Vector2 knockbackDirection = (otherPlayerRb.transform.position - transform.position).normalized;
                otherPlayerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

                // Apply knockback to the attacker (this player)
                Vector2 selfKnockbackDirection = (transform.position - otherPlayerRb.transform.position).normalized;
                rb.AddForce(selfKnockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }

        // Set attack cooldown
        canAttack = false;
        attackTimer = isHighAttack ? highAttackCooldown : lowAttackCooldown; // Set cooldown duration based on attack type
    }

    // Method to check if the player is dodging
    public bool IsDodging()
    {
        // Return true if the player is currently dodging.
        // Implement your dodge state check here.
        return false; // Placeholder, replace with actual dodge logic
    }

    // Example dodge coroutine
    private IEnumerator Dodge()
    {
        // Here you might want to add invincibility frames or dodge movement
        // Example: Temporarily disable the player's collider or set an invincibility flag
        yield return new WaitForSeconds(0.5f); // Duration of the dodge
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
