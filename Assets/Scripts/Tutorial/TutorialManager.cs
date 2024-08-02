using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject MovementTutorial;
    public GameObject BuildInstructions;
    public GameObject AdditionalBuildInstructions;
    public GameObject CharacterSelect;
    public GameObject CharacterMove;

    public Button MovementContinueButton;
    public Button BuildContinueButton;
    public Button AdditionalBuildContinueButton;
    public Button CharacterMoveContinueButton;
    public Button CharacterSelectContinueButton;

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

    void Start()
    {
        // Start tutorial mode with the first step
        SetTutorialStep(TutorialStep.Movement);

        // Set up button listeners
        MovementContinueButton.onClick.AddListener(() => SetTutorialStep(TutorialStep.Build));
        BuildContinueButton.onClick.AddListener(() => SetTutorialStep(TutorialStep.AdditionalBuild));
        AdditionalBuildContinueButton.onClick.AddListener(() => SetTutorialStep(TutorialStep.CharacterMove));
        CharacterMoveContinueButton.onClick.AddListener(() => SetTutorialStep(TutorialStep.CharacterSelect));
        CharacterSelectContinueButton.onClick.AddListener(() => SetTutorialStep(TutorialStep.None));
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
        // Handle movement logic if needed
    }

    private void HandleBuildStep()
    {
        // Handle build logic if needed
    }

    private void HandleAdditionalBuildStep()
    {
        // Handle additional build logic if needed
    }

    private void HandleCharacterMoveStep()
    {
        // Handle character move logic if needed
    }

    private void HandleCharacterSelectStep()
    {
        // Handle character select logic if needed
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

        // Activate/deactivate UI buttons based on the step
        MovementContinueButton.gameObject.SetActive(step == TutorialStep.Movement);
        BuildContinueButton.gameObject.SetActive(step == TutorialStep.Build);
        AdditionalBuildContinueButton.gameObject.SetActive(step == TutorialStep.AdditionalBuild);
        CharacterMoveContinueButton.gameObject.SetActive(step == TutorialStep.CharacterMove);
        CharacterSelectContinueButton.gameObject.SetActive(step == TutorialStep.CharacterSelect);
    }
}
