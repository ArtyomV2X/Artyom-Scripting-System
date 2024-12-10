
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[Obsolete("Replaced with Linked Object System")]
[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/Pickup Following Mover Fix")]
public class PlayerTrigger : UdonSharpBehaviour
{
    public Animator targetAnimator; // The Animator component to control
    public string parameterName; // The name of the boolean parameter to change

    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        if (targetAnimator != null && !string.IsNullOrEmpty(parameterName))
        {
            targetAnimator.SetBool(parameterName, true); // Set the boolean parameter to true
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (targetAnimator != null && !string.IsNullOrEmpty(parameterName))
        {
            targetAnimator.SetBool(parameterName, false); // Set the boolean parameter to false
        }
    }
}
