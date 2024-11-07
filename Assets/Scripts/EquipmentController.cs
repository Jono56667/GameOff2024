using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class EquipmentController : MonoBehaviour
{

    public UnityEvent UseEvent;
    public Transform target; // The transform to follow
    public Transform RightIK;
    public float positionDamping = 10f; // Damping for position (higher means more precise)
    public float rotationDamping = 10f; // Damping for rotation (higher means more precise)
    public Vector3 ToolADSPos;

    public float initialForceMagnitude = 10f; // Initial force magnitude
    public float initialRotationTorqueMagnitude = 5f; // Initial torque magnitude
    public bool Equipt;
    private Rigidbody rb;
    private float currentForceMagnitude;
    private float currentRotationTorqueMagnitude;

    public AudioClip Clip;
    public AudioSource SoundSource;
    public Animator Anim;
    public bool LightBroken;
    public GameObject IndParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing.");
        }
        currentForceMagnitude = initialForceMagnitude;
        currentRotationTorqueMagnitude = initialRotationTorqueMagnitude;
    }

    void FixedUpdate()
    {
        if (rb == null || target == null) return;

        // Smooth position movement
        Vector3 positionDifference = target.position - rb.position;
        Vector3 desiredVelocity = positionDifference * positionDamping;
        rb.velocity = Vector3.Lerp(rb.velocity, desiredVelocity, Time.fixedDeltaTime * positionDamping);

        // Smooth rotation
        Quaternion targetRotation = target.rotation;
        Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationDamping);
        rb.MoveRotation(newRotation);

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CameraController CamCont = Camera.main.GetComponent<CameraController>();
            if (!CamCont.UiEnabled)
            {
                if (Equipt)
                {
                    UseEvent.Invoke();
                }
            }
        }
    }

    public void ToggleInd()
    {
        IndParticle.SetActive(false);
    }

    public void ToggleTorch(Light light)
    {
        PlaySound();
        if(!LightBroken)
        {
            if (light.intensity == 0)
            {
                light.intensity = 1;
                return;
            }
            else
            {
                light.intensity = 0;
                return;
            }
        }
    }

    public void PlaySound()
    {
        SoundSource.clip=Clip;
        SoundSource.Play();
    }
    
}
