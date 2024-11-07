using UnityEngine;
using TMPro;  // Import the TextMeshPro namespace
using System.Collections.Generic;
using UnityEngine.Events;

public class CombinationLock : MonoBehaviour
{
    public string Code;  // The correct combination code (e.g., "1234")
    public List<CodeInputManager> Inputs;  // List of CodeInputManager components representing the inputs

    public UnityEvent UnlockEvent;

    // Method to check if the entered code matches the correct code
    public void CheckCode()
    {
        // Combine all the input values into a single string
        string enteredCode = "";

        foreach (CodeInputManager inputManager in Inputs)
        {
            // Add the input value from each CodeInputManager to the enteredCode string
            enteredCode += inputManager.Input.ToString();
        }

        // Check if the combined enteredCode matches the correct Code
        if (enteredCode == Code)
        {
            // Code is correct
            Debug.Log("Code is correct!");
            UnlockEvent.Invoke();
            // Perform actions when the code is correct (e.g., unlock the door)
        }
        else
        {
            // Code is incorrect
            Debug.Log("Code "+enteredCode+ " Does not match Code "+ Code);
            // Perform actions when the code is incorrect (e.g., give feedback to the player)
        }
    }
}
