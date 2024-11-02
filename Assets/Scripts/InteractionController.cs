using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GameEvents;

public class InteractionController : MonoBehaviour
{
    public UnityEvent InteractEvent;
    //public FireArmController FireArmController;

    public GameObject player;
    private void OnEnable()
    {
        GameEvents.Oninteraction += InteractionEvent;
        GameEvents.OndropWeapon += DropWeapon;
        GameEvents.OndropTool += DropTool;
    }

    private void OnDisable()
    {
        GameEvents.Oninteraction -= InteractionEvent;
        GameEvents.OndropWeapon -= DropWeapon;
        GameEvents.OndropTool -= DropTool;
    }

    private void InteractionEvent(GameObject InteractedObj, GameObject Player)
    {
        if(InteractedObj == this.gameObject)
        {
            player = Player;
            InteractEvent.Invoke();
        }
    }

    public void PickUpWeapon(FireArmController Controller)
    {
        Controller = GetComponent<FireArmController>();

        player.GetComponent<CameraController>().EquiptWeapon = this.gameObject;

        Controller.target = player.GetComponent<CameraController>().WeaponHoldPoint.transform;

        Controller.Equipt = true;

    }

    public void PickUpTool(EquipmentController Controller)
    {
        Controller = GetComponent<EquipmentController>();

        player.GetComponent<CameraController>().EquiptWeapon = this.gameObject;

        Controller.target = player.GetComponent<CameraController>().WeaponHoldPoint.transform;

        Controller.Equipt = true;

    }

    private void DropWeapon(GameObject Weapon, GameObject Player, FireArmController Controller)
    {
        Player.GetComponent<CameraController>().EquiptWeapon = null;

        Controller.target = null;

        Controller.Equipt = false;
    }
    private void DropTool(GameObject Tool, GameObject Player, EquipmentController Controller)
    {
        Player.GetComponent<CameraController>().EquiptWeapon = null;

        Controller.target = null;

        Controller.Equipt = false;
    }
}
