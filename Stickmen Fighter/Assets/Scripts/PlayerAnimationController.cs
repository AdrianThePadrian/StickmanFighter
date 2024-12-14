using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component not found. Animations will be disabled.");
        }
    }

    public IEnumerator InitializeAnimator()
    {
        Debug.Log("Animator initialized.");
        yield return null;
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

    public void SetAnimator(Animator newAnimator)
    {
        animator = newAnimator;
    }
}