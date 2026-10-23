
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;
    public PlayerHealth playerHealth;

    private bool isPaused = false;

    private void Start()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    // Called by PlayerInput (New Input System)
    private void OnPause(InputValue value)
    {
        if (!value.isPressed)
            return;

        // Do not open Pause Menu after death
        if (playerHealth != null &&
            playerHealth.CurrentLives <= 0)
            return;

        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (pausePanel == null)
            return;

        isPaused = true;
        pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }
}
