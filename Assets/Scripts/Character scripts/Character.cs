using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public float hunger;
    public float thirst;
    public float health;
    public float baseHungerDecayRate = 1f;  // Base decay rate
    public float baseThirstDecayRate = 1f;  // Base decay rate
    public float healthDecayRate = 1f;

    public float maxHunger = 100f;
    public float maxThirst = 100f;
    public float maxHealth = 100f;

    public AudioClip deathSound;
    private AudioSource audioSource;
    private bool isDead = false;

    private World world;
    private Shelter shelter;

    private string[] possibleNames = new string[] { "Alex", "Jordan", "Taylor", "Morgan", "Charlie", "Casey", "Drew", "Riley", "Skyler", "Parker" };

    // New productivity field
    public float productivity;

    // Adjusted decay rates
    private float hungerDecayRate;
    private float thirstDecayRate;

    void Start()
    {
        characterName = GenerateRandomName();
        world = FindObjectOfType<World>();
        shelter = FindObjectOfType<Shelter>();
        hunger = maxHunger;
        thirst = maxThirst;
        health = maxHealth;

        audioSource = gameObject.AddComponent<AudioSource>();

        productivity = Random.Range(0f, 200f); // productivity percentage 

        // Calculate adjusted decay rates based on productivity
        float productivityFactor = productivity / 100f;
        hungerDecayRate = baseHungerDecayRate * productivityFactor;
        thirstDecayRate = baseThirstDecayRate * productivityFactor;
    }

    void Update()
    {
        ManageNeeds();
    }

    void ManageNeeds()
    {
        if (isDead) return;

        hunger -= Time.deltaTime * hungerDecayRate;
        thirst -= Time.deltaTime * thirstDecayRate;

        if (hunger <= 0 || thirst <= 0)
        {
            health -= Time.deltaTime * healthDecayRate;
        }

        hunger = Mathf.Clamp(hunger, 0, maxHunger);
        thirst = Mathf.Clamp(thirst, 0, maxThirst);
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0 && !isDead)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        isDead = true;
        audioSource.PlayOneShot(deathSound);
        StartCoroutine(DisappearAfterSound());

        if (shelter != null)
        {
            shelter.RemoveResident(gameObject);
        }
    }

    IEnumerator DisappearAfterSound()
    {
        yield return new WaitForSeconds(deathSound.length);
        gameObject.SetActive(false);
    }

    public void TryToEat()
    {
        Debug.Log($"Current hunger: {hunger}, Food available: {world.HasFood(1)}");

        // Ensure the character only eats if hunger is at or below 80%
        if (hunger <= 160 && world.HasFood(1))
        {
            float foodValue = Mathf.Min(160, maxHunger - hunger);
            Eat(foodValue);
            world.RemoveFood(1);
        }
        else
        {
            Debug.Log("Character is not hungry enough or no food in inventory!");
        }
    }

    public void TryToDrink()
    {
        Debug.Log($"Current thirst: {thirst}, Water available: {world.HasWater(1)}");

        // Ensure the character only drinks if thirst is at or below 80%
        if (thirst <= 160 && world.HasWater(1))
        {
            float waterValue = Mathf.Min(160, maxThirst - thirst);
            Drink(waterValue);
            world.RemoveWater(1);
        }
        else
        {
            Debug.Log("Character is not thirsty enough or no water in inventory!");
        }
    }

    public void Eat(float foodValue)
    {
        hunger = Mathf.Min(hunger + foodValue, maxHunger);
    }

    public void Drink(float waterValue)
    {
        thirst = Mathf.Min(thirst + waterValue, maxThirst);
    }

    private string GenerateRandomName()
    {
        int index = Random.Range(0, possibleNames.Length);
        return possibleNames[index];
    }

    public float GetProductivity()
    {
        return productivity;
    }
}
