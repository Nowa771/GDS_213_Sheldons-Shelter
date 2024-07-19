using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject MovementTutorial;
    public GameObject BuildInstructions;
    public GameObject AdditionalBuildInstructions;
    public GameObject CharacterSelect;
    public GameObject CharacterMove;

    private enum TutorialStep
    {
        Movement,
        Build,
        AdditionalBuild,
        CharacterMove,
        CharacterSelect,
        None
    }

    private TutorialStep currentStep = TutorialStep.Movement;
    private bool wp, ap, sp, dp;
    private bool spacePressed;
    private bool mouse1Pressed;
    private bool mouse0Pressed;

    void Start()
    {
        // Start tutorial mode with the first step
        currentStep = TutorialStep.Movement;
        SetTutorialStep(TutorialStep.Movement);
    }

    void Update()
    {
        switch (currentStep)
        {
            case TutorialStep.Movement:
                HandleMovementStep();
                break;
            case TutorialStep.Build:
                HandleBuildStep();
                break;
            case TutorialStep.AdditionalBuild:
                HandleAdditionalBuildStep();
                break;
            case TutorialStep.CharacterMove:
                HandleCharacterMoveStep();
                break;
            case TutorialStep.CharacterSelect:
                HandleCharacterSelectStep();
                break;
        }
    }

    private void HandleMovementStep()
    {
        if (Input.GetKeyDown(KeyCode.W)) wp = true;
        if (Input.GetKeyDown(KeyCode.A)) ap = true;
        if (Input.GetKeyDown(KeyCode.S)) sp = true;
        if (Input.GetKeyDown(KeyCode.D)) dp = true;

        Debug.Log($"Movement step: W: {wp}, A: {ap}, S: {sp}, D: {dp}");

        if (wp && ap && sp && dp)
        {
            SetTutorialStep(TutorialStep.Build);
        }
    }

    private void HandleBuildStep()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            spacePressed = true;
        }

        Debug.Log($"Build step: B pressed: {spacePressed}");

        if (spacePressed)
        {
            SetTutorialStep(TutorialStep.AdditionalBuild);  // Proceed to the next step
        }
    }

    private void HandleAdditionalBuildStep()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Additional build step: Mouse button 0 pressed");
            SetTutorialStep(TutorialStep.CharacterMove);  // Proceed to the next step
        }
    }

    private void HandleCharacterMoveStep()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mouse1Pressed = true;
        }

        Debug.Log($"Character move step: Mouse button 1 pressed: {mouse1Pressed}");

        if (mouse1Pressed)
        {
            SetTutorialStep(TutorialStep.CharacterSelect); // Proceed to the next step
        }
    }

    private void HandleCharacterSelectStep()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouse0Pressed = true;
        }

        Debug.Log($"Character select step: Mouse button 0 pressed: {mouse0Pressed}");

        if (mouse0Pressed)
        {
            SetTutorialStep(TutorialStep.None);  // End of tutorial
        }
    }

    private void SetTutorialStep(TutorialStep step)
    {
        currentStep = step;
        Debug.Log($"Tutorial step changed to: {step}");

        MovementTutorial.SetActive(step == TutorialStep.Movement);
        BuildInstructions.SetActive(step == TutorialStep.Build);
        AdditionalBuildInstructions.SetActive(step == TutorialStep.AdditionalBuild);
        CharacterMove.SetActive(step == TutorialStep.CharacterMove);
        CharacterSelect.SetActive(step == TutorialStep.CharacterSelect);
    }
}
