using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isSelected = false;
    private Transform assignedSpot;
    private Room currentRoom;
    private OutlineEffect outlineEffect; // Reference to outline effect script

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        outlineEffect = GetComponent<OutlineEffect>(); // Assuming OutlineEffect is attached to the same GameObject

        if (animator == null)
        {
            Debug.LogError("Animator component is not assigned or found on this GameObject.");
        }
        else
        {
            Debug.Log("Animator component successfully found.");
        }
    }

    void Update()
    {
        if (isSelected && Input.GetMouseButtonDown(1)) // Right mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Room room = hit.collider.GetComponent<Room>();
                if (room != null)
                {
                    MoveToRoom(room);
                }
            }
        }

        if (assignedSpot != null)
        {
            navMeshAgent.SetDestination(assignedSpot.position);

            // Check if character has reached the assigned spot
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    navMeshAgent.isStopped = true; // Stop the agent
                }
            }
        }

        float speed = navMeshAgent.velocity.magnitude;
        if (animator != null)
        {
            if (animator.HasParameter("Speed"))
            {
                animator.SetFloat("Speed", speed);
            }
            else
            {
                Debug.LogError("Animator parameter 'Speed' does not exist.");
            }
        }

        // Ensure outline effect is applied
        if (outlineEffect != null)
        {
            outlineEffect.EnableOutline(isSelected);
        }
    }

    public void Select()
    {
        isSelected = true;
        Debug.Log(gameObject.name + " selected");
    }

    public void Deselect()
    {
        isSelected = false;
        Debug.Log(gameObject.name + " deselected");
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void MoveToRoom(Room room)
    {
        Transform spot = room.GetAvailableSpot();
        if (spot != null)
        {
            if (currentRoom != null)
            {
                currentRoom.ReleaseSpot(assignedSpot);
            }

            navMeshAgent.SetDestination(spot.position);
            assignedSpot = spot;
            navMeshAgent.isStopped = false; // Ensure the agent is not stopped
            currentRoom = room;
        }
        else
        {
            Debug.Log("No available spots in the room.");
        }
    }

    public void ClearAssignedSpot()
    {
        if (currentRoom != null)
        {
            currentRoom.ReleaseSpot(assignedSpot);
            currentRoom = null;
        }
        assignedSpot = null;
    }
}

public static class AnimatorExtensions
{
    public static bool HasParameter(this Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }
}
