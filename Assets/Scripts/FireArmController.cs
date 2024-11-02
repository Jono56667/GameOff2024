using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireArmController : MonoBehaviour
{
    public Transform target; // The transform to follow
    public Transform Firepoint; // The point the bullet is fired from
    public Transform LeftIK, RightIK;
    public Vector3 WeaponADSPos;
    public float positionDamping = 10f; // Damping for position (higher means more precise)
    public float rotationDamping = 10f; // Damping for rotation (higher means more precise)

    public float initialForceMagnitude = 10f; // Initial force magnitude
    public float initialRotationTorqueMagnitude = 5f; // Initial torque magnitude
    public float BulletForce = 25f;
    public int numberOfRaycasts = 5; // Number of raycasts to perform
    public float maxRotationVariance = 5f; // Max degree of random rotation variance
    public float timeBetweenShots = 0.5f;
    public bool AutoChamber;
    private bool readyToShoot = true;
    public bool Equipt;
    private Rigidbody rb;
    private float currentForceMagnitude;
    private float currentRotationTorqueMagnitude;
    private float lastShotTime;

    public GameObject ImpactEffect;
    public GameObject Source;
    public AudioSource GunSoundSource;
    public AudioClip FireSound;
    public AudioClip CockSound;
    public Animator Anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing.");
        }
        currentForceMagnitude = initialForceMagnitude;
        currentRotationTorqueMagnitude = initialRotationTorqueMagnitude;
        lastShotTime = Time.time;
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

        // Reset force and torque if the weapon hasn't been fired for more than 1 second
        if (Time.time - lastShotTime > 1f)
        {
            currentForceMagnitude = initialForceMagnitude;
            currentRotationTorqueMagnitude = initialRotationTorqueMagnitude;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Equipt)
            {
                if (AutoChamber) return;

                if (readyToShoot)
                {
                    StartCoroutine(ApplyRandomForceAndTorque());
                    return;
                }
                else
                {
                    StartCoroutine(CockWeapon());
                    return;
                }
            }
        }

        if (Input.GetMouseButton(0)) // Left mouse button click
        {
            if (Equipt)
            {
                if (!AutoChamber) return;

                if (readyToShoot)
                {
                    StartCoroutine(ApplyRandomForceAndTorque());
                    return;
                }
            }
        }
    }

    private IEnumerator ApplyRandomForceAndTorque()
    {
        readyToShoot = false;

        // Generate random torque magnitudes
        float randomXTorque = Random.Range(-currentRotationTorqueMagnitude * 10, 0);
        float randomYTorque = Random.Range(-currentRotationTorqueMagnitude / 5, currentRotationTorqueMagnitude / 5);

        // Convert the local X-axis to world space
        Vector3 localXAxis = transform.right;
        Vector3 localYAxis = transform.up;

        // Apply the torque to the Rigidbody
        rb.AddTorque(localXAxis * randomXTorque, ForceMode.Impulse);
        rb.AddTorque(localYAxis * randomYTorque, ForceMode.Impulse);

        // Generate and apply the force
        Vector3 localBackward = -transform.forward; // Local Z-axis (backwards direction)
        Vector3 force = localBackward * currentForceMagnitude;
        rb.AddForce(force, ForceMode.Impulse);

        // Fire bullets and play gun sound
        FireBullets();
        StartCoroutine(PlayGunSound());

        if(currentForceMagnitude > initialForceMagnitude * 0.025 &&  currentRotationTorqueMagnitude > initialRotationTorqueMagnitude * 0.025)
        {
            // Adjust force and torque for the next shot
            currentForceMagnitude *= 0.025f;
            currentRotationTorqueMagnitude *= 0.025f;

            Debug.Log(currentForceMagnitude + "," + currentRotationTorqueMagnitude);
        }

        lastShotTime = Time.time;

        if (AutoChamber)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            readyToShoot = true;
        }
    }

    private void FireBullets()
    {
        if (Firepoint == null)
        {
            Debug.LogError("Bullet origin transform is not assigned.");
            return;
        }

        for (int i = 0; i < numberOfRaycasts; i++)
        {
            Vector3 randomDirection = Firepoint.forward;
            Quaternion randomRotation = Quaternion.Euler(
                Random.Range(-maxRotationVariance, maxRotationVariance),
                Random.Range(-maxRotationVariance, maxRotationVariance),
                0
            );
            randomDirection = randomRotation * randomDirection;

            if (Physics.Raycast(Firepoint.position, randomDirection, out RaycastHit hit, 100f))
            {
                Debug.DrawRay(Firepoint.position, randomDirection * hit.distance, Color.red, 1.0f);
                Rigidbody hitRigidbody = hit.collider.GetComponent<Rigidbody>();

                if (ImpactEffect != null)
                {
                    GameObject impactPoint = Instantiate(ImpactEffect, hit.point, Quaternion.identity);
                    Destroy(impactPoint, 2f);
                }

                if (hitRigidbody != null)
                {
                    Vector3 forceDirection = (hit.point - Firepoint.position).normalized;
                    hitRigidbody.AddForceAtPosition(forceDirection * BulletForce / numberOfRaycasts, hit.point, ForceMode.Impulse);
                }
            }
        }
    }

    private IEnumerator CockWeapon()
    {
        GunSoundSource.clip = CockSound;
        GunSoundSource.Play();
        Anim.SetTrigger("Rack");
        AnimationClip clip = GetAnimationClip("Rack-Shotgun");
        yield return new WaitForSeconds(clip?.length ?? 1f); // Default to 1 second if clip is null
        readyToShoot = true;
    }

    private IEnumerator PlayGunSound()
    {
        GameObject gunShot = Instantiate(Source, transform.position, Quaternion.identity);
        AudioSource audioSource = gunShot.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.clip = FireSound;
            audioSource.pitch = Random.Range(1.2f, 1.3f);
            audioSource.Play();
        }
        yield return new WaitForSeconds(FireSound.length);
        Destroy(gunShot);
    }

    private AnimationClip GetAnimationClip(string animationName)
    {
        foreach (AnimationClip clip in Anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip;
            }
        }
        return null;
    }
}
