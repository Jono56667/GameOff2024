using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionFirer : MonoBehaviour
{
    public KeyCode InteractionInput;
    public Camera PlayerCamera;

    private void Update()
    {
        if (Input.GetKeyDown(InteractionInput))
        {
            if(PlayerCamera.GetComponent<CameraController>().EquiptWeapon == null)
            {
                RaycastForward();
            }
            else
            {
                DropItem();
            }
        }

        if(Input.GetKeyDown (KeyCode.E)) 
        {
            RaycastForward();
        }
    }

    private void RaycastForward()
    {
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out RaycastHit hit, 1000f))
            {
                if (hit.collider.gameObject.GetComponent<InteractionController>() != null)
                {
                    GameEvents.Oninteraction?.Invoke(hit.collider.gameObject, this.gameObject);
                }
            }
    }

    private void DropItem()
    {
        if (this.gameObject.GetComponent<CameraController>().EquiptWeapon.GetComponent<FireArmController>() != null)
        {
            GameEvents.OndropWeapon?.Invoke(PlayerCamera.GetComponent<CameraController>().EquiptWeapon, this.gameObject, this.gameObject.GetComponent<CameraController>().EquiptWeapon.GetComponent<FireArmController>());
        }
        else
        {
            GameEvents.OndropTool?.Invoke(PlayerCamera.GetComponent<CameraController>().EquiptWeapon, this.gameObject, this.gameObject.GetComponent<CameraController>().EquiptWeapon.GetComponent<EquipmentController>());
        }
    }
}
