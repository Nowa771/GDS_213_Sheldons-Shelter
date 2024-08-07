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
    public AudioClip eatSound;
    public AudioClip drinkSound;
    public AudioClip healSound;

    private AudioSource audioSource;
    private bool isDead = false;

    private World world;
    private Shelter shelter;
    private CharacterManager characterManager; // Reference to CharacterManager

    private string[] possibleNames = new string[]
    {
        "Aiden", "Alec", "Alex", "Aubrey", "Blair", "Blake", "Brielle", "Brady", "Callie", "Casey", "Caden", "Charlie",
        "Cleo", "Daisy", "Derek", "Dylan", "Ella", "Eli", "Finn", "Flora", "Fiona", "Gage", "Gwen", "Harlow", "Holly",
        "Huxley", "Iris", "Ivy", "Jack", "Jasper", "James", "Jude", "Kai", "Kali", "Kira", "Lena", "Lila", "Liam", "Luca",
        "Maggie", "Mason", "Maya", "Nell", "Niamh", "Nina", "Olivia", "Oscar", "Parker", "Peyton", "Piper", "Quin",
        "Quincy", "Riley", "Reese", "Rex", "Sage", "Sloane", "Tate", "Theo", "Tess", "Uma", "Uriah", "Vera", "Violet",
        "Wade", "Wren", "Xander", "Yara", "Zane", "Zara", "Ava", "Bella", "Ben", "Brenna", "Brody", "Carmen", "Carson",
        "Chloe", "Clara", "Colin", "Cooper", "Diana", "Dylan", "Eleanor", "Emily", "Ethan", "Eva", "Felicity", "Gabriel",
        "Gage", "Gavin", "Grace", "Hannah", "Harper", "Harrison", "Henry", "Hudson", "Isaac", "Isla", "Jacob", "Jasmine",
        "Jesse", "Julia", "Kaden", "Katie", "Kayla", "Kennedy", "Kylie", "Landon", "Layla", "Leo", "Lila", "Lily", "Luna",
        "Madeline", "Max", "Mia", "Michael", "Molly", "Nolan", "Nora", "Oliver", "Roman", "Rosie", "Ryan", "Samantha",
        "Samuel", "Sophie", "Stella", "Theo", "Thomas", "Toby", "Tristan", "Victoria", "Violet", "Will", "Wyatt", "Zoe",
        "Zach", "Abigail", "Adam", "Adeline", "Alana", "Albert", "Alexis", "Amber", "Amelia", "Annie", "Arthur", "Ava",
        "Brennan", "Bria", "Bryce", "Caleb", "Cameron", "Carla", "Cedric", "Chase", "Claire", "Clayton", "Conner",
        "Daniel", "David", "Delilah", "Destiny", "Dominic", "Eliana", "Elliot", "Elodie", "Emery", "Eric", "Erica",
        "Evelyn", "Fiona", "Flora", "Freddie", "Gemma", "Gianna", "Grant", "Grayson", "Hazel", "Irene", "Joanna", "Jordyn",
        "Julian", "Kara", "Kasey", "Kendall", "Kimberly", "Lachlan", "Lana", "Lincoln", "Lola", "Maya", "Mia", "Milo",
        "Morgan", "Nadia", "Naomi", "Nico", "Ophelia", "Owen", "Parker", "Phoenix", "Priscilla", "River", "Rose", "Ruby",
        "Salvatore", "Samantha", "Santiago", "Sawyer", "Scarlett", "Seth", "Silas", "Sofia", "Solomon", "Talia", "Tiana",
        "Ulysses", "Vivian", "Wyatt", "Zara", "Zoe"
    };

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
        characterManager = FindObjectOfType<CharacterManager>(); // Get reference to CharacterManager
        hunger = maxHunger;
        thirst = maxThirst;
        health = maxHealth;

        audioSource = gameObject.AddComponent<AudioSource>();

        productivity = Random.Range(0f, 200f); // productivity percentage 

        // Calculate adjusted decay rates based on productivity
        float productivityFactor = productivity / 200f;
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
        StartCoroutine(DestroyAfterSound());

        // Call OnCharacterDestroyed to update shelter capacity
        OnCharacterDestroyed(gameObject);
    }

    IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(deathSound.length);
        Destroy(gameObject);
    }

    public void TryToEat()
    {
        Debug.Log($"Current hunger: {hunger}, Food available: {world.HasFood(1)}");

        // Ensure the character only eats if hunger is at or below 80%
        if (hunger <= 160 && world.HasFood(1))
        {
            float foodValue = Mathf.Min(400, maxHunger - hunger);
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
            float waterValue = Mathf.Min(400, maxThirst - thirst);
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
        audioSource.PlayOneShot(eatSound); // Play eating sound
    }

    public void Drink(float waterValue)
    {
        thirst = Mathf.Min(thirst + waterValue, maxThirst);
        audioSource.PlayOneShot(drinkSound); // Play drinking sound
    }

    public void Heal(float healAmount)
    {
        Debug.Log(characterName + " attempting to heal. Current health: " + health);

        // Check if health is already at max
        if (health >= maxHealth)
        {
            Debug.Log(characterName + " is already at maximum health. No med pack used.");
            return; // Exit the method if already at max health
        }

        // Check if there are med packs available
        if (!Inventory.Instance.HasMedpacks(1))
        {
            Debug.Log("No med packs available for healing.");
            return; // Exit if no med packs are available
        }

        // Proceed with healing and consuming med pack
        float healthBeforeHealing = health;
        health = Mathf.Min(health + healAmount, maxHealth);

        if (health > healthBeforeHealing) // Only consume if healing actually occurs
        {
            Inventory.Instance.RemoveMedpacks(1); // Remove one med pack
            Debug.Log(characterName + " has been healed. Current health: " + health + ". Med pack used.");
            audioSource.PlayOneShot(healSound); // Play healing sound
        }
        else
        {
            Debug.Log("Healing was attempted but health is already at maximum.");
        }
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

    void OnCharacterDestroyed(GameObject character)
    {
        if (shelter != null) // Assuming you have a reference to the Shelter script
        {
            shelter.RemoveResident(character);
            shelter.RefreshCurrentCapacity(); // Update capacity after removal
        }

        if (characterManager != null) // Notify CharacterManager about the removal
        {
            Character characterComponent = character.GetComponent<Character>();
            if (characterComponent != null)
            {
                characterManager.RemoveCharacter(characterComponent);
            }
        }
    }
}
