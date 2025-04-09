using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [Header("Pause Menu")]
    [Tooltip("Das UI-Panel, das das Pause-Menu darstellt.")]
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    private void Update()
    {
        //Wurde Escape-Taste gedrueckt
        if (Input.GetKeyDown(KeyCode.Escape))
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
        pauseMenuUI.SetActive(true); //Pause-Menue anzeigen
        Time.timeScale = 0f;        //Spielzeit pausieren
        isPaused = true;
        Debug.Log("Spiel pausiert.");
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); //Pause-Menue ausblenden
        Time.timeScale = 1f;          //Spielzeit fortsetzen
        isPaused = false;
        Debug.Log("Spiel fortgesetzt.");
    }
}