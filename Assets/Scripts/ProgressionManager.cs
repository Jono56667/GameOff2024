using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ProgressionManager : MonoBehaviour
{
    public int totalClues;
    public int FoundClues;
    public bool CanProgress;
    private void OnEnable()
    {
        GameEvents.OnAddToProgression += AddToProgress;
        GameEvents.OnLoadNextStage += LoadNextStage;
    }
    private void OnDisable()
    {
        GameEvents.OnAddToProgression -= AddToProgress;
        GameEvents.OnLoadNextStage -= LoadNextStage;
    }

    private void AddToProgress()
    {
        if(FoundClues < totalClues)
        {
            FoundClues += 1;
        }
    }

    public void TriggerCanProgress()
    {
        CanProgress = true;
    }

    public void CheckProgress()
    {
        if(CanProgress)
        {
            GameEvents.OnFadeCamera?.Invoke(1);
        }
        else
        {
            Debug.Log("I should keep looking around");
        }
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
