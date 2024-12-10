using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Components;
using VRC.Udon;
using System;
[Obsolete("Component under review for further dubugging... Pardon our dust!")]

[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/Pickup Fizzler")]
public class PickupFizzler : UdonSharpBehaviour
{
    public AudioSource fizzlerSound; // Sound to play when a pickup is fizzled
    public AudioSource playerEnterSound; // Sound to play when a player enters the collider
    public int fizzlerLayer = 8; // Layer number for the fizzler
    public int pickupLayer = 9; // Layer number for the pickup
    public float fizzleDuration = 2.0f; // Duration for the fizzle effect
    public Material fizzleMaterial; // Material to apply during the fizzle effect
    private Material[][] originalMaterials;
    private VRC_Pickup pickup;
    private VRCObjectSync objectSync;
    private Renderer[] renderers;
    private Collider pickupCollider;
    private Rigidbody pickupRigidbody;
    private bool isFizzling = false;
    private float fizzleTimer = 0.0f;
    private Vector3 storedVelocity;
    private Vector3 storedAngularVelocity;

    private void Start()
    {
        // Ensure the AudioSource is initially inactive
        if (fizzlerSound != null)
        {
            fizzlerSound.Stop();
        }

        if (playerEnterSound != null)
        {
            playerEnterSound.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is on the pickup layer and not already fizzling
        if (other.gameObject.layer == pickupLayer && !isFizzling)
        {
            // Check if the collider has a VRC_Pickup component
            pickup = other.GetComponent<VRC_Pickup>();
            if (pickup != null)
            {
                // Get the VRCObjectSync component
                objectSync = pickup.GetComponent<VRCObjectSync>();
                if (objectSync == null)
                {
                    Debug.LogError("No VRCObjectSync component found on the pickup object.");
                    return;
                }
                UdonBehaviour behaviour = (UdonBehaviour)GetComponent(typeof(UdonBehaviour));
                behaviour.DisableInteractive = true;

                // Get the pickup's collider and rigidbody
                pickupCollider = pickup.GetComponent<Collider>();
                pickupRigidbody = pickup.GetComponent<Rigidbody>();

                // Store the current velocity and angular velocity
                if (pickupRigidbody != null)
                {
                    storedVelocity = pickupRigidbody.velocity;
                    storedAngularVelocity = pickupRigidbody.angularVelocity;
                }

                // Get the renderers and materials of the pickup
                renderers = pickup.GetComponentsInChildren<Renderer>();
                originalMaterials = new Material[renderers.Length][];
                for (int i = 0; i < renderers.Length; i++)
                {
                    originalMaterials[i] = renderers[i].materials;
                    Material[] newMaterials = new Material[renderers[i].materials.Length];
                    for (int j = 0; j < newMaterials.Length; j++)
                    {
                        newMaterials[j] = fizzleMaterial;
                    }
                    renderers[i].materials = newMaterials;
                }

                // Play the fizzler sound
                if (fizzlerSound != null)
                {
                    fizzlerSound.transform.SetParent(pickup.transform);
                    fizzlerSound.transform.localPosition = Vector3.zero;
                    fizzlerSound.Play();
                }

                // Disable the pickup's collider
                if (pickupCollider != null)
                {
                    pickupCollider.enabled = false;
                }

                // Force the player to drop the pickup
                if (Networking.LocalPlayer != null)
                {
                    pickup.Drop(Networking.LocalPlayer);
                }

                // Start the fizzling process
                isFizzling = true;
                fizzleTimer = fizzleDuration;
            }
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (playerEnterSound != null && player.isLocal)
        {
            playerEnterSound.Play();
        }
    }

    private void Update()
    {
        if (isFizzling)
        {
            fizzleTimer -= Time.deltaTime;

            // Apply the stored velocity and angular velocity
            if (pickupRigidbody != null)
            {
                pickupRigidbody.velocity = storedVelocity;
                pickupRigidbody.angularVelocity = storedAngularVelocity;
            }
            if (Networking.LocalPlayer != null)
            {
                pickup.Drop(Networking.LocalPlayer);
            }

            if (fizzleTimer <= 0.0f)
            {
                // Respawn the pickup using VRCObjectSync
                if (objectSync != null)
                {
                    objectSync.Respawn();
                }

                // Stop and detach the sound
                if (fizzlerSound != null)
                {
                    fizzlerSound.Stop();
                    fizzlerSound.transform.SetParent(null);
                }

                // Re-enable the pickup's collider
                if (pickupCollider != null)
                {
                    pickupCollider.enabled = true;
                }

                // Reset the original materials
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].materials = originalMaterials[i];
                }

                isFizzling = false;
                UdonBehaviour behaviour = (UdonBehaviour)GetComponent(typeof(UdonBehaviour));
                behaviour.DisableInteractive = false;
            }
        }
    }
}
