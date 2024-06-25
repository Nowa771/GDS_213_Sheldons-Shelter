using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public List<Character> characters;

    // Threshold values for automatic eating and drinking
    public float hungerThreshold = 20.0f;
    public float thirstThreshold = 20.0f;

    // Cooldown times for eating and drinking (in seconds)
    public float eatCooldown = 5.0f;
    public float drinkCooldown = 5.0f;

    private Dictionary<Character, float> lastEatTimes = new Dictionary<Character, float>();
    private Dictionary<Character, float> lastDrinkTimes = new Dictionary<Character, float>();

    void Start()
    {
        foreach (Character character in characters)
        {
            lastEatTimes[character] = Time.time;
            lastDrinkTimes[character] = Time.time;
        }
    }

    void Update()
    {
        foreach (Character character in characters)
        {
            CheckCharacterNeeds(character);
        }
    }

    public void AddCharacter(Character character)
    {
        if (!characters.Contains(character))
        {
            characters.Add(character);
            lastEatTimes[character] = Time.time;
            lastDrinkTimes[character] = Time.time;
        }
    }

    void CheckCharacterNeeds(Character character)
    {
        if (character.hunger < hungerThreshold && Time.time > lastEatTimes[character] + eatCooldown)
        {
            character.TryToEat();
            lastEatTimes[character] = Time.time;
        }

        if (character.thirst < thirstThreshold && Time.time > lastDrinkTimes[character] + drinkCooldown)
        {
            character.TryToDrink();
            lastDrinkTimes[character] = Time.time;
        }
    }
}
