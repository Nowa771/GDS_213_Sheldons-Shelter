using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddResident : MonoBehaviour
{
    public GameObject residentPrefab;
    private Shelter shelter;
    public CharacterManager characterManager;

    void Start()
    {
        shelter = FindObjectOfType<Shelter>();

        if (shelter == null)
        {
            Debug.LogError("Shelter not found in the scene.");
            return;
        }

        if (characterManager == null)
        {
            Debug.LogError("CharacterManager not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddResidentToShelter();
        }
    }

    void AddResidentToShelter()
    {
        if (shelter != null)
        {
            GameObject newResident = Instantiate(residentPrefab);
            bool added = shelter.AddResident(newResident);

            if (added)
            {
                Character newCharacter = newResident.GetComponent<Character>();
                if (newCharacter != null && characterManager != null)
                {
                    characterManager.AddCharacter(newCharacter);
                }
                else
                {
                    Debug.LogError("Newly instantiated resident does not have a Character component or CharacterManager is not assigned.");
                    Destroy(newResident);
                }
            }
            else
            {
                Destroy(newResident);
            }
        }
    }
}
