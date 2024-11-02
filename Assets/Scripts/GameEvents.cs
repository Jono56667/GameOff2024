using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public delegate void Interaction(GameObject IteractedObj,GameObject Player);
    public static Interaction Oninteraction;

    public delegate void DropWeapon(GameObject Weapon,GameObject Player,FireArmController Controller);
    public static DropWeapon OndropWeapon;

    public delegate void DropTool(GameObject Tool, GameObject Player, EquipmentController Controller);
    public static DropTool OndropTool;
}
