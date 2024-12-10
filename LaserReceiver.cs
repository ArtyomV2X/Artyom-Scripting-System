using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/Laser Receiver Controller")]
public class LaserReceiver : UdonSharpBehaviour
{
    public GameObject targetObject; // The object to turn on/off
    public int laserLayer = 22; // Layer number to detect the laser
    public Animator[] animators; // Array to hold Animators
    public string[] parameterNames; // Array to hold corresponding parameter names
    public GameObject audioObjectTrue; // GameObject to enable when the state changes to true
    public GameObject audioObjectFalse; // GameObject to enable when the state changes to false
    public GameObject audioObjectLoop; // GameObject to loop while the state is true
    public Collider triggerCollider; // Reference to the trigger collider
    public Renderer targetRenderer; // Renderer to change material
    public Material materialTrue; // Material to apply when the state is true
    public Material materialFalse; // Material to apply when the state is false
    public int materialIndex = 0; // Index of the material to change

    private int colliderCount = 0; // Counter for colliders with the laser layer
    private Collider[] collidersInsideTrigger;

    private void Start()
    {
        // Ensure the target object is initially off
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }

        // Ensure the looping audio is initially off
        if (audioObjectLoop != null)
        {
            audioObjectLoop.SetActive(false);
        }

        // Initial check for colliders inside the trigger
        UpdateColliderCount();
    }

    private void Update()
    {
        // Continuously check for colliders inside the trigger
        UpdateColliderCount();
    }

    private void UpdateColliderCount()
    {
        // Define the size and position of the box for the OverlapBox check
        Vector3 boxCenter = triggerCollider.bounds.center;
        Vector3 boxHalfExtents = triggerCollider.bounds.extents;

        // Perform the OverlapBox check
        collidersInsideTrigger = Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.identity, 1 << laserLayer);

        // Update the collider count based on the result
        int newColliderCount = 0;

        foreach (var col in collidersInsideTrigger)
        {
            if (col.GetComponent<VRC_Pickup>() == null)
            {
                newColliderCount++;
            }
        }

        if (newColliderCount != colliderCount)
        {
            colliderCount = newColliderCount;
            Debug.Log("Updated collider count: " + colliderCount);

            // Turn on or off the target object based on the collider count
            if (colliderCount > 0)
            {
                if (targetObject != null)
                {
                    targetObject.SetActive(true);
                }
                UpdateAnims(true);
            }
            else if (colliderCount == 0)
            {
                if (targetObject != null)
                {
                    targetObject.SetActive(false);
                }
                UpdateAnims(false);
            }
        }
    }

    private void UpdateAnims(bool state)
    {
        for (int i = 0; i < animators.Length; i++)
        {
            if (animators[i] != null)
            {
                animators[i].SetBool(parameterNames[i], state);
            }
        }

        // Enable or disable GameObjects based on the state
        if (state)
        {
            if (audioObjectFalse != null) audioObjectFalse.SetActive(false);
            if (audioObjectTrue != null) audioObjectTrue.SetActive(true);
            if (audioObjectLoop != null) audioObjectLoop.SetActive(true); // Enable looping audio
            if (targetRenderer != null && materialTrue != null) SetMaterial(materialTrue); // Change material to true
        }
        else
        {
            if (audioObjectTrue != null) audioObjectTrue.SetActive(false);
            if (audioObjectFalse != null) audioObjectFalse.SetActive(true);
            if (audioObjectLoop != null) audioObjectLoop.SetActive(false); // Disable looping audio
            if (targetRenderer != null && materialFalse != null) SetMaterial(materialFalse); // Change material to false
        }
    }

    private void SetMaterial(Material material)
    {
        Material[] materials = targetRenderer.materials;
        if (materialIndex >= 0 && materialIndex < materials.Length)
        {
            materials[materialIndex] = material;
            targetRenderer.materials = materials;
        }
    }
}
