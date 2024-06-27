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

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
<<<<<<< HEAD
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
        }

=======
>>>>>>> new-character-movement
        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

<<<<<<< HEAD
=======
    private void OnDrawGizmos()
    {
        if (assignedSpot != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(assignedSpot.position, 1);
        }
    }

>>>>>>> new-character-movement
    public void Select()
    {
        isSelected = true;
    }

    public void Deselect()
    {
        isSelected = false;
    }

    public void MoveToRoom(Room room)
    {
        Transform spot = room.GetAvailableSpot();
        if (spot != null)
        {
<<<<<<< HEAD
            navMeshAgent.SetDestination(spot.position);
            assignedSpot = spot;
=======
            assignedSpot = spot;
            navMeshAgent.SetDestination(assignedSpot.position);
>>>>>>> new-character-movement
        }
        else
        {
            Debug.Log("No available spots in the room.");
        }
    }

<<<<<<< HEAD
    public void ClearAssignedSpot()
    {
        assignedSpot = null;
=======
    public void SetAssignedSpot(Transform spot)
    {
        assignedSpot = spot;
    }

    public Transform GetAssignedSpot()
    {
        return assignedSpot;
>>>>>>> new-character-movement
    }
}
