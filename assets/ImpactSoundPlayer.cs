using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[DisallowMultipleComponent]
[AddComponentMenu("Artyom Scripting System/Impact Sound Player")]
public class ImpactSoundPlayer : UdonSharpBehaviour
{
    public GameObject smallImpactSoundObject; // GameObject for small impact sound
    public GameObject mediumImpactSoundObject; // GameObject for medium impact sound
    public GameObject largeImpactSoundObject; // GameObject for large impact sound

    public float mediumImpactThreshold = 5.0f; // Threshold for medium impact
    public float largeImpactThreshold = 10.0f; // Threshold for large impact

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce >= largeImpactThreshold)
        {
            PlaySound(largeImpactSoundObject);
        }
        else if (impactForce >= mediumImpactThreshold)
        {
            PlaySound(mediumImpactSoundObject);
        }
        else
        {
            PlaySound(smallImpactSoundObject);
        }
    }

    private void PlaySound(GameObject soundObject)
    {
        if (soundObject != null)
        {
            soundObject.SetActive(false); // Reset the sound object
            soundObject.SetActive(true);  // Activate the sound object to play the sound
        }
    }
}
