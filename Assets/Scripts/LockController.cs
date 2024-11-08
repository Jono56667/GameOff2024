using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    public GameObject LockScreen;

    public void EnableLockScreen()
    {
        if (LockScreen.activeInHierarchy)
        {
            LockScreen.SetActive(false);
            GameEvents.OnEnableInput?.Invoke(true);
        }
        else
        {
            LockScreen.SetActive(true);
            GameEvents.OnEnableInput?.Invoke(false);
        }
    }
}
