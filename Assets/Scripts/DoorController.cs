using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool Locked;
    public bool Open;

    public void OpenDoor()
    {
        if(!Open)
        {
            if (Locked)
            {
                return;
            }
            if (!Locked)
            {
                Debug.Log("Open Door");
                Open = true;
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Close Door");
            Open = false;
        }
    }

    public void UnlockDoor()
    {
        if(Locked)
        {
            Locked = false;
            Debug.Log("Unlock");
            return;
        }
        if(!Locked) 
        {
            Locked = true;
            Debug.Log("lock");
            return ;
        }
    }
}
