
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[AddComponentMenu("Artyom Scripting System/Timed Teleport")]
[DisallowMultipleComponent]
public class TimedTeleport : UdonSharpBehaviour
{
    [Header("Teleportation Settings")]
    public Transform teleportTarget; // The transform to teleport the player to
    public float delay = 1f; // The delay in seconds before teleportation

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (Networking.LocalPlayer == player)
        {
            SendCustomEventDelayedSeconds(nameof(TeleportPlayer), delay);
        }
        
    }

    public void TeleportPlayer()
    {
        Networking.LocalPlayer.TeleportTo(teleportTarget.position, 
                                          teleportTarget.rotation, 
                                          VRC_SceneDescriptor.SpawnOrientation.Default, 
                                          false
                                        );
    }
}

