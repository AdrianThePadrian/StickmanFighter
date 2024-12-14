using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    private Animator animator;
    private PlayerAnimationController animController;
    public PlayerHealth playerHealth;
    private PlayerInput playerInput;
    private int playerIndex;

    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    public bool canMove = true;

    [Header("Combat")]
    public int highAttackDamage = 1;
    public int lowAttackDamage = 1;
    public Transform highAttackPoint;
    public Transform lowAttackPoint;
    public float highAttackRange = 1f;
    public float lowAttackRange = 0.5f;
    public LayerMask playerLayer;
    public float knockbackForce = 5f;
    public float highAttackCooldown = 1f;
    public float lowAttackCooldown = 0.5f;

    private bool canAttack = true;
    private float attackTimer;
    private bool isHurt = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        animController = GetComponent<PlayerAnimationController>();
        
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component missing!");
            return;
        }
        
        playerIndex = playerInput.playerIndex;
        Debug.Log($"PlayerController Awake - Player {playerIndex} initialized");
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log($"Move Input received: {moveInput}");
    }

    public void OnHighAttack(InputValue value)
    {
        Debug.Log("High Attack Input received");
        if (canAttack && !isHurt)
        {
            canMove = false;
            Attack(true);
            animController.PlayHighAttack();
            StartCoroutine(ResetMoveAfterDelay(0.5f));
        }
    }

    public void OnLowAttack(InputValue value)
    {
        Debug.Log("Low Attack Input received");
        if (canAttack && !isHurt)
        {
            canMove = false;
            Attack(false);
            animController.PlayLowAttack();
            StartCoroutine(ResetMoveAfterDelay(0.5f));
        }
    }

    private void FixedUpdate()
    {
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                canAttack = true;
            }
        }

        if (canMove && !rb.isKinematic)
        {
            Vector3 movement = new Vector3(moveInput.x, 0f, 0f) * moveSpeed;
            rb.linearVelocity = movement;

            if (animController != null)
            {
                animController.SetMoving(Mathf.Abs(movement.x) > 0.1f);
            }
        }
        else if (!rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    private IEnumerator ResetMoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    public bool IsDodging()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("LowAttack");
    }

    private void Attack(bool isHighAttack)
    {
        if (isHurt) return;

        Transform attackPoint = isHighAttack ? highAttackPoint : lowAttackPoint;
        float attackRange = isHighAttack ? highAttackRange : lowAttackRange;
        int damage = isHighAttack ? highAttackDamage : lowAttackDamage;

        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        foreach (Collider player in hitPlayers)
        {
            if (player.gameObject != gameObject)
            {
                PlayerHealth enemyHealth = player.GetComponent<PlayerHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    
                    // Add knockback
                    Rigidbody enemyRb = player.GetComponent<Rigidbody>();
                    if (enemyRb != null)
                    {
                        Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
                        enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                    }
                }
            }
        }

        attackTimer = isHighAttack ? highAttackCooldown : lowAttackCooldown;
        canAttack = false;
    }

    public void SetVictoryPose()
    {
        animController.TriggerVictory();
    }

    public void SetDefeatPose()
    {
        animController.TriggerDefeat();
    }

    public void ResetPlayer()
    {
        Debug.Log($"ResetPlayer called for player {playerIndex}");
        // Reset state
        canMove = true;
        canAttack = true;
        isHurt = false;
        attackTimer = 0;
        moveInput = Vector2.zero;
        
        // Reset position
        Transform spawnPoint = PlayerSpawner.instance.GetSpawnPoint(playerIndex);
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = playerIndex == 0 ? 
                Quaternion.Euler(0, 90, 0) : 
                Quaternion.Euler(0, -90, 0);
                
            // Reset physics
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            Debug.LogError($"Spawn point not found for player {playerIndex}");
        }
    }
}
