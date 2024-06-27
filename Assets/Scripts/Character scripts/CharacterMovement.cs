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
        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    private void OnDrawGizmos()
    {
        if (assignedSpot != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(assignedSpot.position, 1);
        }
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
            assignedSpot = spot;
            navMeshAgent.SetDestination(assignedSpot.position);
        }
        else
        {
            Debug.Log("No available spots in the room.");
        }
    }

    public void SetAssignedSpot(Transform spot)
    {
        assignedSpot = spot;
    }

    public Transform GetAssignedSpot()
    {
        return assignedSpot;
    }
}
