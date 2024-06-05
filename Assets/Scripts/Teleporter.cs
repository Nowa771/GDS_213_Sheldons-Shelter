using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform targetTeleporter;
    public bool teleportUp = false; // Indicates if teleportation should move upwards

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Teleport(other.transform);
        }
    }

    private void Teleport(Transform objectToTeleport)
    {
        Vector3 targetPosition = targetTeleporter.position;

        // If teleporting up, adjust the target position accordingly
        if (teleportUp)
        {
            float heightDifference = targetTeleporter.position.y - transform.position.y;
            targetPosition.y = objectToTeleport.position.y + heightDifference;
        }

        objectToTeleport.position = targetPosition;
    }
}
