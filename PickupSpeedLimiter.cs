using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/Pickup Speed Limiter")]

public class PickupSpeedLimiter : UdonSharpBehaviour
{
    public float maxSpeed = 5.0f; // Maximum allowed speed for the pickup
    public bool debugSpeed;
    private VRC_Pickup pickup; // Reference to the VRC_Pickup component

    void Start()
    {
        pickup = GetComponent<VRC_Pickup>();

        if (pickup == null)
        {
            Debug.LogError("No VRC_Pickup component found on this GameObject.");
        }
    }

    void Update()
    {
        if (pickup != null && !(pickup.IsHeld))
        {
            Rigidbody rb = pickup.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 velocity = rb.velocity;
                
                if (debugSpeed) {
                    if (velocity.magnitude > maxSpeed) { Debug.LogWarning("Speed of cube: " + rb.velocity.magnitude.ToString());} 
                    Debug.Log("Speed of cube: " + rb.velocity.magnitude.ToString()); }
                if (velocity.magnitude > maxSpeed)
                {
                    rb.velocity = velocity.normalized * maxSpeed;
                }
            }
        }
    }
}
