using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool Locked;
    public bool Open;
    public AudioSource Source;
    public AudioClip LockSound, OpenSound, CloseSound;
    public Vector3 OpenPos, ClosedPos;
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
                if(Source != null)
                {
                    Debug.Log("Open Door");
                    Open = true;
                    Source.clip = OpenSound;
                    Source.Play();
                    transform.localRotation = Quaternion.Euler(OpenPos);

                }
                else
                {
                    Debug.Log("Open Door");
                    Open = true;
                    gameObject.SetActive(false);
                }
                

            }
        }
        else
        {
            Debug.Log("Close Door");
            Open = false;
            transform.localRotation = Quaternion.Euler(ClosedPos);
            Source.clip = CloseSound;
            Source.Play();
        }
    }

    public void UnlockDoor()
    {
        if(Locked)
        {
            Locked = false;
            Debug.Log("Unlock");
            Source.clip = LockSound;
            Source.Play();
            return;
        }
        if(!Locked) 
        {
            Locked = true;
            Debug.Log("lock");
            Source.clip = LockSound;
            Source.Play();
            return ;
        }
    }
}
