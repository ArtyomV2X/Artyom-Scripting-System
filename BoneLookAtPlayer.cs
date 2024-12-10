using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[DisallowMultipleComponent]
[AddComponentMenu("Artyom Scripting System/Bone Look At Player")]
public class BoneLookAtPlayer : UdonSharpBehaviour
{
    public GameObject objectToRotate; // The object to rotate
    public float rotationSpeed = 90.0f; // Maximum rotation speed in degrees per second
    public Vector3 rotationOffset; // Rotation offset for each axis

    private Transform objectTransform;

    void Start()
    {
        if (objectToRotate != null)
        {
            objectTransform = objectToRotate.transform;
        }
        else
        {
            Debug.LogError("Object to rotate not assigned.");
        }
    }

    void Update()
    {
        VRCPlayerApi nearestPlayer = GetNearestPlayer(); // Method to get the nearest player
        if (nearestPlayer != null)
        {
            Vector3 playerHeadPosition = nearestPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;

            // Calculate the direction to look
            Vector3 directionToLook = playerHeadPosition - objectTransform.position;
            if (directionToLook != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
                
                // Apply the rotation offset
                targetRotation *= Quaternion.Euler(rotationOffset);

                objectTransform.rotation = Quaternion.RotateTowards(objectTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    VRCPlayerApi GetNearestPlayer()
    {
        VRCPlayerApi nearestPlayer = null;
        float closestDistance = float.MaxValue;

        VRCPlayerApi[] players = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
        VRCPlayerApi.GetPlayers(players);

        Vector3 currentPosition = transform.position;

        foreach (VRCPlayerApi player in players)
        {
            if (player != null) // Allow local player for nearest search
            {
                float distance = Vector3.Distance(currentPosition, player.GetPosition());
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestPlayer = player;
                }
            }
        }

        return nearestPlayer;
    }
}
