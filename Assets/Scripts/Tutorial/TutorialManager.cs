using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject MovementTutorial;
    public GameObject BuildInsturctions;
    public GameObject CharacterSelect;
    public GameObject CharacterMove;

    private bool wp = false, ap = false, sp = false, dp = false;
    private bool Spacep = false;
    private bool mouse1d = false;
    private bool mouse0d = false; 
    private bool isTutorialMode = true;

    // Start is called before the first frame update
    void Start()
    {
        // Start tutorial mode
        isTutorialMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            wp = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ap = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            sp = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            dp = true;
        }

        if (wp && ap && sp && dp)
        {
            MovementTutorial.SetActive(false);

            if (Input.GetKeyDown(KeyCode.B))
            {
                Spacep = true;
            }

            BuildInsturctions.SetActive(true);

            if (Spacep)
            {
                BuildInsturctions.SetActive(false);
                CharacterMove.SetActive(true);

                if (Input.GetMouseButtonDown(1))
                {
                    mouse1d = true;
                }

                if (mouse1d)
                {
                    CharacterMove.SetActive(false);
                    CharacterSelect.SetActive(true);

                    if (Input.GetMouseButtonDown(0))
                    {
                        mouse0d = true;
                    }

                    if (mouse0d)
                    {
                        CharacterSelect.SetActive(false);
                        isTutorialMode = false; // Set tutorial mode to false
                    }
                }
            }
        }
    }
}