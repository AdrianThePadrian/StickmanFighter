using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing from PlayerAnimationController.");
        }
    }

    public IEnumerator InitializeAnimator()
    {
        // Initialize or reset animator parameters if needed
        Debug.Log("Animator initialized.");
        yield return null; // This allows the method to be used with StartCoroutine
    }

    public void PlayHighAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("HighAttack");
        }
    }

    public void PlayLowAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("LowAttack");
        }
    }

    public void TriggerVictory()
    {
        if (animator != null)
        {
            animator.SetTrigger("Victory");
        }
    }

    public void TriggerDefeat()
    {
        if (animator != null)
        {
            animator.SetTrigger("Defeat");
        }
    }

    public void SetMoving(bool isMoving)
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }
    }
}