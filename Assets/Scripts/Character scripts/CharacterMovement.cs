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

        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

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
            navMeshAgent.SetDestination(spot.position);
            assignedSpot = spot;
        }
        else
        {
            Debug.Log("No available spots in the room.");
        }
    }

    public void ClearAssignedSpot()
    {
        assignedSpot = null;
    }
}
