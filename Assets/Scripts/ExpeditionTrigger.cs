using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpeditionTrigger : MonoBehaviour
{
    public float cooldownDuration = 20f; // Duration of the cooldown period
    private bool isCooldown = false;
    private float lastActivationTime;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCooldown && other.CompareTag("Player"))
        {
            ExpeditionManager.Instance.StartExpedition(other.gameObject);
            StartCooldown();
        }
    }

    private void StartCooldown()
    {
        isCooldown = true;
        lastActivationTime = Time.time;
        StartCoroutine(ResetCooldown());
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);
        isCooldown = false;
    }

    public bool IsOnCooldown()
    {
        return isCooldown;
    }

    public float RemainingCooldownTime()
    {
        if (!isCooldown)
        {
            return 0f; 
        }
        else
        {
            return Mathf.Max(0f, lastActivationTime + cooldownDuration - Time.time);
        }
    }
}