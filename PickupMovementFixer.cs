using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/Pickup Following Mover Fix")]
public class PickupMovementFixer : UdonSharpBehaviour
{
    public GameObject targetObject;  // The object to follow

    private int pickupLayer = 13;  // The layer assigned to pickup objects
    private Collider triggerCollider;
    private Vector3 previousTargetPosition;
    private Vector3 targetVelocity;

    private void Start()
    {
        triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null || !triggerCollider.isTrigger)
        {
            Debug.LogError("PickupMovementFixer: Collider must be set as a trigger.");
        }
        previousTargetPosition = targetObject.transform.position;
    }

    private void Update()
    {
        Vector3 currentTargetPosition = targetObject.transform.position;
        targetVelocity = (currentTargetPosition - previousTargetPosition) / Time.deltaTime;
        previousTargetPosition = currentTargetPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == pickupLayer)
        {
            VRC_Pickup pickup = other.gameObject.GetComponent<VRC_Pickup>();
            if (pickup != null)
            {
                Vector3 relativePosition = other.transform.position - targetObject.transform.position;
                pickup.gameObject.transform.position = targetObject.transform.position + relativePosition;
                Rigidbody pickupRigidbody = other.GetComponent<Rigidbody>();
                if (pickupRigidbody != null)
                {
                    pickupRigidbody.velocity = targetVelocity;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == pickupLayer)
        {
            VRC_Pickup pickup = other.gameObject.GetComponent<VRC_Pickup>();
            if (pickup != null)
            {
                Vector3 relativePosition = other.transform.position - targetObject.transform.position;
                pickup.gameObject.transform.position = targetObject.transform.position + relativePosition;
                Rigidbody pickupRigidbody = other.GetComponent<Rigidbody>();
                if (pickupRigidbody != null)
                {
                    pickupRigidbody.velocity = targetVelocity;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == pickupLayer)
        {
            // Optionally, you can add code here to handle when the pickup exits the trigger
        }
    }
}
