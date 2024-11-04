using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameEvents;

public class CanvasController : MonoBehaviour
{
    public GameObject JournalUI;
    public TextMeshProUGUI AddedToNotesText;
    private AudioSource Source;

    public List<GameObject> Notes;
    private void OnEnable()
    {
        GameEvents.OnEnableUi += ToggleUi;
        GameEvents.OnFoundNote += FoundNote;
    }
    private void OnDisable()
    {
        GameEvents.OnEnableUi -= ToggleUi;
        GameEvents.OnFoundNote += FoundNote;
    }

    private void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    private void ToggleUi(bool toggle, string UI)
    {
        if(UI == "Journal")
        {
            RectTransform rectTransform = JournalUI.GetComponent<RectTransform>();
            if (toggle)
            {
                rectTransform.anchoredPosition = new Vector2(-1000, 0);
                GameEvents.OnEnableInput?.Invoke(true);
                //JournalUI.SetActive(false);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(0, 0);
                GameEvents.OnEnableInput?.Invoke(false);
                //JournalUI.SetActive(true);
            }
        }

        if(Notes.Count!=0)
        {
            // Check for matching UI string in the Notes list
            foreach (GameObject note in Notes)
            {
                if (note.name == UI) // Assuming the UI string matches the GameObject's name
                {
                    Debug.Log(note.name);
                    if (!note.activeInHierarchy)
                    {
                        note.SetActive(true); // Toggle the GameObject's active state
                        GameEvents.OnEnableInput?.Invoke(false);
                        break; // Exit the loop once a match is found
                    }
                    else
                    {
                        note.SetActive(false); // Toggle the GameObject's active state
                        GameEvents.OnEnableInput?.Invoke(true);
                        break; // Exit the loop once a match is found
                    }
                }
            }
        }
    }

    public void ToggleFoundNoteUI(string UI)
    {
        GameEvents.OnEnableUi?.Invoke(false, UI);
    }


    private void FoundNote(int ID, string Name)
    {
        AddedToNotesText.text = (Name + " has been added to notes");
        AddedToNotesText.gameObject.GetComponent<Animator>().SetTrigger("Triggered");
    }

    public void PlayFile(AudioClip clip)
    {
        if(Source.isPlaying)
        {
            Source.clip = null;
            Source.Stop();
        }
        else
        {
            Source.clip = clip;
            Source.Play();
        }
        
    }
}
