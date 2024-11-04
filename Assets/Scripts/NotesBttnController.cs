using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameEvents;

public class NotesBttnController : MonoBehaviour
{
    public int ID;
    public bool Discovered;

    public TextMeshProUGUI BttnText;
    public GameObject Info;
    public GameObject UndescoveredInfo;
    private void OnEnable()
    {
        GameEvents.OnFoundNote += FoundNote;
        GameEvents.OnHideNoteInfo += HideNote;
    }

    private void OnDisable()
    {
        GameEvents.OnFoundNote -= FoundNote;
        GameEvents.OnHideNoteInfo -= HideNote;
    }

    private void FoundNote(int noteID, string Name)
    {
        Debug.Log("Picked Up " + Name);
        if (noteID == ID)
        {
            Discovered = true;
            BttnText.text = Name;
        }
    }

    public void SelectedNote()
    {
        if(Discovered)
        {
            GameEvents.OnHideNoteInfo?.Invoke();
            Info.SetActive(true);
        }
        else
        {
            GameEvents.OnHideNoteInfo?.Invoke();
            UndescoveredInfo.SetActive(true);
        }
    }

    private void HideNote()
    {
        if (Info.activeInHierarchy)
        {
            Info.SetActive(false);
        }
        if (UndescoveredInfo.activeInHierarchy)
        {
            UndescoveredInfo.SetActive(false);
        }
        
    }

}
