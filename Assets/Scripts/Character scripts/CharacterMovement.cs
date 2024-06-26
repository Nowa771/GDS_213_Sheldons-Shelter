using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isSelected = false;
    private Vector3 hitPoint;

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
                navMeshAgent.SetDestination(hit.point);
                hitPoint = hit.point;
            }
        }

        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitPoint, 1);
        Gizmos.DrawLine(Camera.main.transform.position, hitPoint);
    }

    public void Select()
    {
        isSelected = true;
    }

    public void Deselect()
    {
        isSelected = false;
    }
}