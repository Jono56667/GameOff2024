using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float defaultFOV, ADSFOV;
    public float mouseSensitivity = 100.0f;
    public Transform playerBody;

    private float xRotation = 0.0f;
    private Vector3 WeaponIdlePos;

    public GameObject WeaponHoldPoint;
    public GameObject EquiptWeapon;

    public float MouseX,MouseY;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
            // Get mouse movement
            MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Rotate camera based on mouse movement
            xRotation -= MouseY;
            xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f); // Limit vertical rotation
            transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
            playerBody.Rotate(Vector3.up * MouseX); // Rotate player body

        if (Input.GetMouseButtonDown(1))
        {

            Camera MainCam = this.gameObject.GetComponent<Camera>();
            defaultFOV = MainCam.fieldOfView;
            MainCam.fieldOfView = 30;

            if (EquiptWeapon != null)
            {
                WeaponIdlePos = WeaponHoldPoint.transform.localPosition;
                if (EquiptWeapon.GetComponent<FireArmController>() != null)
                {
                    Vector3 WeaponADSPos = EquiptWeapon.GetComponent<FireArmController>().WeaponADSPos;
                    WeaponHoldPoint.transform.localPosition = new Vector3(WeaponADSPos.x, WeaponADSPos.y, WeaponADSPos.z);
                }
                if(EquiptWeapon.GetComponent<EquipmentController>() != null)
                {
                    Vector3 ToolADSPos = EquiptWeapon.GetComponent<EquipmentController>().ToolADSPos;
                    WeaponHoldPoint.transform.localPosition = new Vector3(ToolADSPos.x, ToolADSPos.y, ToolADSPos.z);
                }
                
            }
        }
        if(Input.GetMouseButtonUp(1))
        {
            Camera MainCam = this.gameObject.GetComponent<Camera>();
            MainCam.fieldOfView = defaultFOV;
            if (EquiptWeapon != null)
            {
                WeaponHoldPoint.transform.localPosition = WeaponIdlePos;
            }
        }
    }

}
