
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Image = UnityEngine.UI.Image;

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

    public bool UiEnabled;
    public Image FadeImage;

    public TextMeshProUGUI interactionText; // Assign this in the inspector to your UI Text element
    public float raycastDistance = 10f; // Distance for the raycast

    private void OnEnable()
    {
        GameEvents.OnEnableInput += ToggleInput;
        GameEvents.OnFadeCamera += FadeToBlack;
    }
    private void OnDisable()
    {
        GameEvents.OnEnableInput -= ToggleInput;
        GameEvents.OnFadeCamera -= FadeToBlack;
    }
    private void Start()
    {
        if (playerBody == null)
        {
            UiEnabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            GameEvents.OnFadeCamera?.Invoke(-2);
        }
        
    }

    private void Update()
    {
        if (!UiEnabled)
        {

            // Get mouse movement
            MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Rotate camera based on mouse movement
            xRotation -= MouseY;
            xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f); // Limit vertical rotation
            transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
            if(playerBody!= null)
            {
                playerBody.Rotate(Vector3.up * MouseX); // Rotate player body
            }

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
                    if (EquiptWeapon.GetComponent<EquipmentController>() != null)
                    {
                        Vector3 ToolADSPos = EquiptWeapon.GetComponent<EquipmentController>().ToolADSPos;
                        WeaponHoldPoint.transform.localPosition = new Vector3(ToolADSPos.x, ToolADSPos.y, ToolADSPos.z);
                    }

                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                Camera MainCam = this.gameObject.GetComponent<Camera>();
                MainCam.fieldOfView = defaultFOV;
                if (EquiptWeapon != null)
                {
                    WeaponHoldPoint.transform.localPosition = WeaponIdlePos;
                }
            }

        }


        if (Input.GetKeyDown(KeyCode.J)) 
        {
            GameEvents.OnEnableUi?.Invoke(UiEnabled,"Journal");
        }

        // Perform the raycast from the center of the camera's view
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // Check if the raycast hits something within the specified distance
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Check if the hit object has an InteractionController script
            InteractionController interactionController = hit.collider.GetComponent<InteractionController>();

            if (interactionController != null)
            {
                // If the InteractionController exists, update the UI text with the input string
                interactionText.text = interactionController.input;
            }
            else
            {
                // If no InteractionController script is found, clear the UI text
                interactionText.text = "";
            }
        }
        else
        {
            // If the raycast doesn't hit anything, clear the UI text
            interactionText.text = "";
        }

    }

    private void ToggleInput(bool toggle)
    {
        if(toggle)
        {
            UiEnabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            UiEnabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void FadeToBlack(int Speed)
    {
        StartCoroutine(FadeCoroutine(Speed));
        if(playerBody != null)
        {
            GameEvents.OnEnableInput?.Invoke(true);
        }
        else
        {
            GameEvents.OnEnableInput?.Invoke(true);
        }
    }
    private IEnumerator FadeCoroutine(int Speed)
    {
        Color originalColor = FadeImage.color; // Store the original color of the Image
        bool isFadingOut = Speed > 0; // If Speed is positive, we are fading out, otherwise fading in
        float targetAlpha = isFadingOut ? 1f : 0f; // Target alpha based on fade direction
        float duration = Mathf.Abs(Speed); // Use absolute value for duration, as speed can be negative
        float timeElapsed = 0f;

        // Loop to gradually change the alpha value
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, targetAlpha, timeElapsed / duration); // Interpolating the alpha value
            FadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha); // Set new color with updated alpha

            yield return null;
        }

        // Ensure the color is exactly the target alpha at the end
        FadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);

        // Trigger the event only if we're fading out
        if (isFadingOut)
        {
            GameEvents.OnLoadNextStage?.Invoke();
        }
    }
}
