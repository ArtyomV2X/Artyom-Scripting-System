using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[Obsolete("Replaced with Linked Object System")]
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/_DEPRECATED_/Pickup Detector")]
public class PickupDetector : UdonSharpBehaviour
{
    // You can specify the pickup object here if you want to check for a specific object
    public Animator targetAnimator; // The Animator component to control
    public string parameterName; // The name of the boolean parameter to change

    private void OnTriggerEnter(Collider other)
    {
        VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
        if (pickup != null)
        {
            targetAnimator.SetBool(parameterName, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
        if (pickup != null)
        {
            targetAnimator.SetBool(parameterName, false);
        }
    }
}
