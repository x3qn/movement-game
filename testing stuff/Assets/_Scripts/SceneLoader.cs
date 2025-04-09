using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    const string MAIN_MENU = "Main Menu";
    const string LEVEL_NR = "Level";
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU);
    }
    public void GoToLevelNr(int levelNr)
    {
        SceneManager.LoadScene(LEVEL_NR + levelNr.ToString());
    }
}
