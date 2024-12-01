using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
    public List<TextMeshProUGUI> textList;  // List of TextMeshPro text objects to fade in
    public GameObject buttonToEnable;           // The button to enable after all texts fade in
    public float fadeDuration = 1f;         // Duration for each text fade-in
    public float waitDuration = 2f;         // Duration to wait before fading the next text

    void Start()
    {
        // Ensure the button is initially disabled
        buttonToEnable.SetActive(false);
        // Start the coroutine to fade in texts
        StartCoroutine(FadeInTexts());
    }

    IEnumerator FadeInTexts()
    {
        yield return new WaitForSeconds(waitDuration);
        // Loop through each TextMeshPro text in the list
        foreach (TextMeshProUGUI text in textList)
        {
            // Set the text's alpha to 0 (fully transparent) at the start
            Color startColor = text.color;
            startColor.a = 0f;
            text.color = startColor;

            // Fade the text in over the specified duration
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
                text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }

            // Wait for the specified duration before moving to the next text
            yield return new WaitForSeconds(waitDuration);
        }

        // Enable the button after all texts have faded in
        buttonToEnable.SetActive(true);
    }
}
