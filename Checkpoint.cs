using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/Checkpoint")]
public class Checkpoint : UdonSharpBehaviour
{
    public GameObject targetObject; // The game object to move

    [UdonSynced]
    private bool hasBeenUsed = false;


    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (hasBeenUsed || !player.isLocal) return;

        targetObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
        hasBeenUsed = true;
        gameObject.SetActive(false); // Disables the script after use
    }
}
