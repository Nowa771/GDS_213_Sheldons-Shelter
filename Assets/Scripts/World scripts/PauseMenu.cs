using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public float fadeDuration = 0.5f; // Duration for the fade effect

    private CanvasGroup canvasGroup;
    private bool isPaused = false;

    void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        canvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = pauseMenuUI.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        StartCoroutine(FadeOut());
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 0f;
        pauseMenuUI.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled time to respect the time scale
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 1f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled time to respect the time scale
            yield return null;
        }

        canvasGroup.alpha = 0f;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
