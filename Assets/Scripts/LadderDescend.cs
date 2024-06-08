using UnityEngine;
using UnityEngine.AI;

public class LadderDescend : MonoBehaviour
{
    public float descendSpeed = 3.0f;
    private bool isDescending = false;
    private Vector3 ladderBottomPosition;
    private NavMeshAgent agent;
    private CharacterMovement characterMovement;
    private Transform ladder;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterMovement = GetComponent<CharacterMovement>();
        Debug.Log("NavMeshAgent and CharacterMovement found");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LadderBottom"))
        {
            Debug.Log("Ladder bottom detected");
            ladder = other.transform.parent; // Assuming ladder is a child of the ladder bottom trigger
            if (ladder != null)
            {
                ladderBottomPosition = other.transform.position; // Get the bottom position of the trigger
                Debug.Log("Calculated ladderBottomPosition: " + ladderBottomPosition);
                StartDescending();
            }
            else
            {
                Debug.LogWarning("Ladder object is null.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LadderBottom"))
        {
            Debug.Log("Exited Ladder bottom");
            StopDescending();
        }
    }

    void StartDescending()
    {
        isDescending = true;
        agent.enabled = false; // Disable NavMeshAgent
        characterMovement.enabled = false; // Disable character movement while descending
        Debug.Log("Started Descending");
        // Optional: set animation state to descending if you have animations
    }

    void Update()
    {
        if (isDescending)
        {
            DescendLadder();
        }
    }

    void DescendLadder()
    {
        Vector3 direction = Vector3.down; // Moving directly downwards
        transform.position += direction * descendSpeed * Time.deltaTime;
        Debug.Log("Descending... Current Position: " + transform.position);

        // Check if the character has reached the bottom of the ladder
        if (transform.position.y <= ladderBottomPosition.y + 0.1f) // Adding a small offset for precision
        {
            Debug.Log("Reached bottom of the ladder");
            transform.position = ladderBottomPosition; // Ensure precise position
            StopDescending();
        }
    }

    void StopDescending()
    {
        isDescending = false;
        agent.enabled = true; // Re-enable NavMeshAgent
        characterMovement.enabled = true; // Re-enable character movement
        Debug.Log("Stopped Descending");
        // Optional: set animation state back to walking
    }
}