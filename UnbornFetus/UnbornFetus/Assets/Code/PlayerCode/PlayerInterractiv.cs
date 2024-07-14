using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterractiv : MonoBehaviour
{
    public float maxDistance = 10f; // Triggering range
    public bool Triggered; // Outside triggering

    [Header("DEBUG")]
    public bool ShowRayCastPoint = false; // Debug

    public string TargetName;

    private RaycastHit hit;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Connecting the main camera to an object
        Triggered = false; // Failsafe to prevent something from triggering from the start by mistake
        if (mainCamera == null) // Failsafe if it can't find cam with the tag MainCamera
        {
            Debug.LogError("Main camera not found. Please ensure your camera is tagged as 'MainCamera'.");
            return;
        }
    }

    void Update()
    {
        if (mainCamera != null)
        {
            RaycastPoint();
        }

        if (Triggered)
        {
            Debug.Log("Ray hit: " + hit.collider.name);
            TargetName = hit.collider.gameObject.name;
        }
    }

    public void RaycastPoint()
    {
        Transform cameraTransform = mainCamera.transform;
        Vector3 direction = cameraTransform.forward;

        if (Physics.Raycast(cameraTransform.position, direction, out hit, maxDistance))
        {
            Triggered = true;

            if (ShowRayCastPoint)
            {
                Debug.DrawRay(cameraTransform.position, direction * maxDistance, Color.red);
                Debug.Log("Objects name : " + TargetName);
            }
        }
        else
        {
            Triggered = false;

            if (ShowRayCastPoint)
            {
                Debug.DrawRay(cameraTransform.position, direction * maxDistance, Color.green);
            }
        }
    }

    // Optional: Draw gizmos to visualize ray in the Scene and Game views
    void OnDrawGizmos()
    {
        if (ShowRayCastPoint)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }

            Transform cameraTransform = mainCamera.transform;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * maxDistance);
        }
    }
}
