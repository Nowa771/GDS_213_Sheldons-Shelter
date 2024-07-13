using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public List<Character> characters;
    public CharacterInfoDisplay characterInfoDisplay; // Reference to CharacterInfoDisplay

    private int currentIndex = 0;

    void Start()
    {
        if (characters.Count > 0)
        {
            SelectCharacter(currentIndex);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SelectPreviousCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SelectNextCharacter();
        }
    }

    public void AddCharacter(Character character)
    {
        if (!characters.Contains(character))
        {
            characters.Add(character);
        }
    }

    void SelectCharacter(int index)
    {
        if (characters.Count > 0 && index >= 0 && index < characters.Count)
        {
            if (characters[currentIndex] != null)
            {
                characters[currentIndex].GetComponent<CharacterMovement>().Deselect();
            }

            currentIndex = index;
            characters[currentIndex].GetComponent<CharacterMovement>().Select();
            characterInfoDisplay.SelectCharacter(characters[currentIndex]); // Notify CharacterInfoDisplay
        }
    }

    void SelectPreviousCharacter()
    {
        int previousIndex = (currentIndex - 1 + characters.Count) % characters.Count;
        SelectCharacter(previousIndex);
    }

    void SelectNextCharacter()
    {
        int nextIndex = (currentIndex + 1) % characters.Count;
        SelectCharacter(nextIndex);
    }
}
