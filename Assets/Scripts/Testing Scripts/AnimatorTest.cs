using UnityEngine;

public class AnimatorTest : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component is not assigned or found on this GameObject.");
            return;
        }

        Debug.Log("Animator component successfully found.");
        Debug.Log("Setting Speed parameter to 1.0");
        animator.SetFloat("Speed", 1.0f);
    }
}
