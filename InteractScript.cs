using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[Obsolete("Replaced with Linked Object System")]
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/_DEPRECATED_/Interact Script")]
public class InteractScript : UdonSharpBehaviour
{
    public Animator[] animators; // Array to hold Animators
    public string[] parameterNames; // Array to hold corresponding parameter names
    public InteractionType interactionType = InteractionType.None; // Dropdown for interaction type
    public float temporaryDuration = 2.0f; // Duration for temporary interaction
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

    public override void Interact()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject); // Ensure the local player is the owner
        if (interactionType == InteractionType.Temporary)
        {
            isOccupied = isOccupied + 1;
            UpdateAnims(true);
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(NetworkUpdateAnims));
            SendCustomEventDelayedSeconds(nameof(RevertTemporaryInteraction), temporaryDuration);
        }
        else if (interactionType == InteractionType.Permanent)
        {
            isOccupied = isOccupied + 1;
            UpdateAnims(true);
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(NetworkUpdateAnims));
        }
        else if (interactionType == InteractionType.Toggle)
        {
            switch(isOccupied)
            {
                case 0:
                    isOccupied = isOccupied + 1;
                    UpdateAnims(true);
                    break;
                case 1:
                    isOccupied = 0;
                    UpdateAnims(false);
                    break;
            }
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(NetworkUpdateAnims));
        }
    }

    public void RevertTemporaryInteraction()
    {
        isOccupied = isOccupied - 1;
        if (isOccupied < 1)
        {
            UpdateAnims(false);
        }
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(NetworkUpdateAnims));
    }

    public void NetworkUpdateAnims()
    {
        bool state = isOccupied > 0;
        UpdateAnims(state);
    }
}
