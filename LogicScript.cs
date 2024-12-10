using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[Obsolete("Replaced with Linked Object System")]
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/_DEPRECATED_/Logic Script")]
public class LogicScript : UdonSharpBehaviour
{
    public Animator[] animators; // Array to hold Animators
    public string[] parameterNames; // Array to hold corresponding parameter names
    public bool playerTriggerEnabled = true;
    public bool pickupTriggerEnabled = true;
    
    [UdonSynced]
    public int isOccupied;
    
    public GameObject audioObjectTrue; // GameObject to enable when the state changes to true
    public GameObject audioObjectFalse; // GameObject to enable when the state changes to false

    void Start()
    {
        isOccupied = 0;
    }

    private void UpdateAnims(bool state)
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetBool(parameterNames[i], state);
        }

        // Enable or disable GameObjects based on the state
        if (state)
        {
            if (audioObjectFalse != null) audioObjectFalse.SetActive(false);
            if (audioObjectTrue != null) audioObjectTrue.SetActive(true);
        }
        else
        {
            if (audioObjectTrue != null) audioObjectTrue.SetActive(false);
            if (audioObjectFalse != null) audioObjectFalse.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pickupTriggerEnabled)
        {
            VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
            if (pickup != null)
            {
                isOccupied = isOccupied + 1;
                UpdateAnims(true);
            }
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (playerTriggerEnabled)
        {
            isOccupied = isOccupied + 1;
            if (isOccupied > 0)
            {
                UpdateAnims(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (pickupTriggerEnabled)
        {
            VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
            if (pickup != null)
            {
                isOccupied = isOccupied - 1;
                if (isOccupied < 1)
                {
                    UpdateAnims(false);
                }
            }
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (playerTriggerEnabled)
        {
            isOccupied = isOccupied - 1;
            if (isOccupied < 1)
            {
                UpdateAnims(false);
            }
        }
    }
}
