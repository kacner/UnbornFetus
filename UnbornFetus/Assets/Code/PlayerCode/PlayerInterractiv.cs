using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterractiv : MonoBehaviour
{
    public float maxDistance = 10f; // Triggering raing 
    public bool Triggered; // Outside Triggering 

    [Header("DEBUG")]
    public bool ShowRayCastPoint = false; // Debug 

    Camera mainCamera = Camera.main; // Connecting the main camera to an object

    void Start()
    {
        Triggered = false; // Failsafe to prevent something from triggering from the start by mistake
        if (mainCamera == null) // Failsafe if it cant fine cam with the tag MainCamra
        {
            Debug.LogError("Main camera not found. Please ensure your camera is tagged as 'MainCamera'.");
            return;
        }
    }

    void Update()
    {
        RaycastPoint();
    }

    public void RaycastPoint()
    {
        Transform cameraTransform = mainCamera.transform;
        Vector3 direction = cameraTransform.forward;

        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, direction, out hit, maxDistance))
        {
            Triggered = true;
            Debug.Log("Ray hit: " + hit.collider.name);

            if (ShowRayCastPoint)
            {
                Debug.DrawRay(cameraTransform.position, direction * maxDistance, Color.red);
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

    /** Optional: Draw gizmos to visualize ray in the Scene and Game views
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
    **/
}
