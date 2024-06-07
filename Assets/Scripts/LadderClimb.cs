using UnityEngine;
using UnityEngine.AI;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 3.0f;
    private bool isClimbing = false;
    private Vector3 ladderTopPosition;
    private NavMeshAgent agent;
    private CharacterMovement characterMovement;
    private Transform ladder;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterMovement = GetComponent<CharacterMovement>();
        Debug.Log("NavMeshAgent and CharacterMovement found");
    }

    void Update()
    {
        if (isClimbing)
        {
            ClimbLadder();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LadderTop"))
        {
            Debug.Log("Ladder top detected");
            ladder = other.transform.parent; // Assuming ladder is a child of the ladder top trigger
            ladderTopPosition = ladder.position + new Vector3(0, ladder.localScale.y + 1.0f, 0); // Adding 1.0f to ensure reaching the top
            Debug.Log("Calculated ladderTopPosition: " + ladderTopPosition);
            StartClimbing();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LadderTop"))
        {
            Debug.Log("Exited Ladder top");
            StopClimbing();
        }
    }

    void StartClimbing()
    {
        isClimbing = true;
        agent.enabled = false;
        characterMovement.enabled = false; // Disable character movement while climbing
        Debug.Log("Started Climbing");
        // Optional: set animation state to climbing if you have animations
    }

    void ClimbLadder()
    {
        Vector3 direction = Vector3.up; // Moving directly upwards
        transform.position += direction * climbSpeed * Time.deltaTime;
        Debug.Log("Climbing... Current Position: " + transform.position);

        if (Vector3.Distance(transform.position, ladderTopPosition) < 0.1f)
        {
            Debug.Log("Reached top of the ladder");
            transform.position = ladderTopPosition; // Ensure precise position
            StopClimbing();
        }
    }

    void StopClimbing()
    {
        isClimbing = false;
        agent.enabled = true;
        characterMovement.enabled = true; // Re-enable character movement
        Debug.Log("Stopped Climbing");
        // Optional: set animation state back to walking
    }
}