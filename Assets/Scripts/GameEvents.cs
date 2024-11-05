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

    public delegate void EnableUi(bool UiEnabled, string UI);
    public static EnableUi OnEnableUi;

    public delegate void EnableInput(bool InputEnabled);
    public static EnableInput OnEnableInput;

    public delegate void FoundNote(int NoteID,string Name);
    public static FoundNote OnFoundNote;

    public delegate void HideNoteInfo();
    public static HideNoteInfo OnHideNoteInfo;

    public delegate void AddToProgression();
    public static AddToProgression OnAddToProgression;

    public delegate void FadeCamera(int Speed);
    public static FadeCamera OnFadeCamera;

    public delegate void LoadNextStage();
    public static LoadNextStage OnLoadNextStage;
}
