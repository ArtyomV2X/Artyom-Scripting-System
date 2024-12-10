using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

[DisallowMultipleComponent]

[AddComponentMenu("Artyom Scripting System/Laser Beam Controller")]
public class LaserBeam : UdonSharpBehaviour
{
    public LineRenderer lineRenderer; // Line Renderer for the laser beam
    public Transform laserOrigin; // Origin point of the laser
    public float maxDistance = 100.0f; // Maximum distance the laser can reach
    public LayerMask layerMask; // Layer mask to specify which colliders to detect
    public GameObject endPointObject; // GameObject to place at the end of the laser line

    void Update()
    {
        if (lineRenderer == null || laserOrigin == null)
        {
            return;
        }

        RaycastHit hit;
        Vector3 laserEndPoint = laserOrigin.position + (laserOrigin.forward * maxDistance);

        if (Physics.Raycast(laserOrigin.position, laserOrigin.forward, out hit, maxDistance, layerMask))
        {
            laserEndPoint = hit.point;
        }

        // Convert world positions to local positions relative to the laser object's transform
        Vector3 localLaserOrigin = transform.InverseTransformPoint(laserOrigin.position);
        Vector3 localLaserEndPoint = transform.InverseTransformPoint(laserEndPoint);

        lineRenderer.SetPosition(0, localLaserOrigin);
        lineRenderer.SetPosition(1, localLaserEndPoint);

        // Place the endPointObject at the end of the laser line
        if (endPointObject != null)
        {
            endPointObject.transform.position = laserEndPoint;
        }
    }
}
