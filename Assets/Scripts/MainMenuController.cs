using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject AboutDevMenu;

    public GameObject CurrentMenu;

    private void Start()
    {
        if (MainMenu != null)
        {
            CurrentMenu = MainMenu;
            CurrentMenu.SetActive(true);
        }
    }
    public void StartGame()
    {
        LoadNextStage();
    }

    public void Options()
    {
        CurrentMenu.SetActive(false);
        CurrentMenu = OptionsMenu;
        CurrentMenu.SetActive(true);
    }

    public void AboutDev()
    {
        CurrentMenu.SetActive(false);
        CurrentMenu = AboutDevMenu;
        CurrentMenu.SetActive(true);
    }

    public void Back()
    {
        CurrentMenu.SetActive(false);
        CurrentMenu = MainMenu;
        CurrentMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextStage()
    {
        // Get the current active scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the next scene index
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene by index
            SceneManager.LoadScene(nextSceneIndex);
        }

    }
}
