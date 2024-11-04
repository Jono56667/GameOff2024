using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public int ID;
    public string Name;
    public void PickUpNote()
    {
        GameEvents.OnFoundNote?.Invoke(ID, Name);
    }
}
