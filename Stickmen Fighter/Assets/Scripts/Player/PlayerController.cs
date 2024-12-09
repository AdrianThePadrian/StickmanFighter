using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;


public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;  // Change to 3D Rigidbody
    private Animator animator;
    private PlayerAnimationController animController;
    public PlayerHealth playerHealth;
    private PlayerInput playerInput;

    [Header("Input Actions")]
    private InputAction moveAction;
    private InputAction highAttackAction;
    private InputAction lowAttackAction;

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
    private int playerIndex;

    [Header("Model")]
    private GameObject playerModel;
    public Transform modelRoot; // Reference to the root of the character model

    private void Awake()
    {
        // Get components
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animController = GetComponent<PlayerAnimationController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;

        if (playerHealth == null || animController == null)
        {
            Debug.LogError("Required components missing on " + gameObject.name);
        }

        // Configure Rigidbody
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnHighAttack(InputValue value)
    {
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

        if (canMove)
        {
            Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
            
            // Rotate character to face movement direction
            if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(movement);
                animController.SetMoving(true);
            }
            else
            {
                animController.SetMoving(false);
            }
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
        canMove = true;
        canAttack = true;
        isHurt = false;
        attackTimer = 0;
        
        Transform spawnPoint = PlayerSpawner.instance.GetSpawnPoint(playerIndex);
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = playerIndex == 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        }
    }

    // Add this method to initialize the model
    public void InitializeModel(GameObject model)
    {
        playerModel = model;
        modelRoot = model.transform;
        
        // Update attack points to use the new model's transforms if needed
        Transform attackPointsParent = modelRoot.Find("AttackPoints");
        if (attackPointsParent != null)
        {
            highAttackPoint = attackPointsParent.Find("HighAttackPoint");
            lowAttackPoint = attackPointsParent.Find("LowAttackPoint");
        }

        // Get animator from the model if it exists there
        animator = model.GetComponent<Animator>();
        if (animator == null)
        {
            animator = model.AddComponent<Animator>();
        }
        
        // Ensure AnimationController is updated with the new animator
        if (animController != null)
        {
            StartCoroutine(animController.InitializeAnimator());
        }
    }
}
