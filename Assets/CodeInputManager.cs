using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeInputManager : MonoBehaviour
{
    public int Input;

    public void AddToVal(TextMeshProUGUI Text)
    {
        // If Input is 9, set it to 0, otherwise increment by 1
        if (Input == 9)
        {
            Input = 0;
        }
        else
        {
            Input += 1;
        }

        // Update the UI Text
        Text.text = Input.ToString();
    }

    // Called when the "Remove" button is pressed
    public void RemoveFromVal(TextMeshProUGUI Text)
    {
        // If Input is 0, set it to 9, otherwise decrement by 1
        if (Input == 0)
        {
            Input = 9;
        }
        else
        {
            Input -= 1;
        }

        // Update the UI Text
        Text.text = Input.ToString();
    }
}
