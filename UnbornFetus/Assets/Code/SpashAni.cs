using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SpashAni : MonoBehaviour
{
    // Public variable to control the speed of scaling down and when to destroy object.

    [Header("Spash Verison")]
    public bool spash3D;
    public bool spash2D;

    [Header("Editbil Settings")]
    public float speed;
    public float colapsValue;
    public float waitingValue;

    // Private variables for tracking time, scaling, and the object's state
    private float timeTime;
    private Vector3 scaling;
    private bool isGone;
    private float waitTimer; // Add a timer variable

    private void Start()
    {
        if (spash3D)
            spash2D = false;

        if (spash2D)
            spash3D = false;

        waitTimer = waitingValue; // Initialize the wait timer
    }

    void Update()
    {
        timeTime = Time.deltaTime;

        if (spash3D)
        {
            Fade(speed, timeTime);

            if (scaling.x < colapsValue) // Check if the new x scale is below a certain threshold to consider the object "gone"
            {
                isGone = true;
            }
        }

        if (spash2D)
        {
            // Add timer logic within the designated area
            if (!isGone)
            {
                if (waitTimer > 0)
                {
                    waitTimer -= timeTime; // Decrement the timer
                }
                else
                {
                        Fade(speed, timeTime);

                        if (scaling.x < colapsValue) // Check if the new x scale is below a certain threshold to consider the object "gone"
                        {
                            isGone = true;
                        }
                }
            }
        }
    }

    void Fade(float speed, float time)
    {
        // Calculate the new scale values by interpolating between current scale and zero
        scaling = new Vector3(Mathf.Lerp(transform.localScale.x, 0, time * speed),
                              Mathf.Lerp(transform.localScale.y, 0, time * speed),
                              Mathf.Lerp(transform.localScale.z, 0, time * speed));

        if (scaling.x < colapsValue) // Check if the new x scale is below a certain threshold to consider the object "gone"
        {
            isGone = true;
        }

        if (!isGone) // If the object is not "gone", update its scale
            transform.localScale = scaling;
        else if (isGone) // If the object is "gone", destroy the game object
            Destroy(gameObject);
    }
}
