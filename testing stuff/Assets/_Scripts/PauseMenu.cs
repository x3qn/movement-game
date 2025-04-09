using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]
    [Tooltip("Das UI-Panel, das das Pause-Menu darstellt.")]
    public GameObject pauseMenuUI;

    [Header("Kamera & Steuerung")]
    [Tooltip("Das Kamerasteuerungs-Skript, das deaktiviert werden soll.")]
    public MonoBehaviour cameraController; // Beispiel: CameraController, PlayerController, etc.

    private bool isPaused = false;

    private void Update()
    {
        // Wurde Escape-Taste gedrückt?
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void Endgame()
    {
        Debug.Log("GameOver");
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Pause-Menü anzeigen
        Time.timeScale = 0f;         // Spielzeit pausieren
        isPaused = true;

        // Cursor aktivieren & sichtbar machen
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Kamerasteuerung deaktivieren
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }

        Debug.Log("Spiel pausiert.");
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Pause-Menü ausblenden
        Time.timeScale = 1f;          // Spielzeit fortsetzen
        isPaused = false;

        // Cursor deaktivieren & sperren
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Kamerasteuerung wieder aktivieren
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        Debug.Log("Spiel fortgesetzt.");
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; // Wichtiger Fix: Stelle sicher, dass die Zeit nicht auf 0 bleibt!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Lädt die aktuelle Szene neu
        Debug.Log("Szene wird neugestartet.");
    }
}
