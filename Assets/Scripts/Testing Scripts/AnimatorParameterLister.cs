using UnityEngine;

public class AnimatorParameterLister : MonoBehaviour
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

        Debug.Log("Animator parameters:");
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            Debug.Log($"Parameter: {param.name}, Type: {param.type}");
        }
    }
}
