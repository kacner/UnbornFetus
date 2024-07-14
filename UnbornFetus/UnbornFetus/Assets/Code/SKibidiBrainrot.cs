using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SKibidiBrainrot : MonoBehaviour
{
    [Header("Fetus Settings")]
    public float speed = 7f; // Initial speed of the object
    public float helpTimer; // Timer to track help

    [Header("Triggers")]
    public bool hasGameStarted; // Flag to check if the game has started
    public bool brakeAway; // Flag to check if the object has broken away

    [Header("References Objects")]
    public GameObject HelpingHeand; // Reference to the helping hand object
    public GameObject helpingText; // Reference to the helping text object
    public GameObject UmbilicalParticlesystem; // Reference to the umbilical particle system
    public PointPointMovie pointpointmove; // Reference to the PointPointMovie script

    private float changeInterval = 0.7f; // Interval for changing direction

    private Rigidbody rb; // Rigidbody component of the object
    private HingeJoint joint; // HingeJoint component of the object
    private float velocity; // Current velocity of the object

    private bool canRedo = true; // Flag to check if help can be redone

    void Start()
    {
        // Getting componets form References Objects
        joint = GetComponent<HingeJoint>(); 
        rb = GetComponent<Rigidbody>(); 

        Time.timeScale = 0.5f; // Slow down the game time

        // Set up repeated method calls
        InvokeRepeating("ChangeDirection", 0f, changeInterval);
        InvokeRepeating("HelpChecker", 1, 1);

        brakeAway = false;
        hasGameStarted = false;

        // Deactivate initial objects
        UmbilicalParticlesystem.SetActive(false);
        HelpingHeand.SetActive(false);
        helpingText.SetActive(false);
    }

    public void Update()
    {
        velocity = rb.velocity.magnitude; // Update current velocity
        Debug.Log(velocity);

        if (hasGameStarted)
            helpTimer += Time.deltaTime; // Increment help timer if the game has started

        // Check if the object should break away
        if (velocity > 7f && !brakeAway && hasGameStarted)
        {
            UmbilicalParticlesystem.SetActive(true); // Activate umbilical particle system

            Debug.Log("Disconnect called");
            brakeAway = true;
            transform.SetParent(null); // Detach from parent
            Destroy(joint); // Destroy the joint

            rb.drag = 0f;
            rb.freezeRotation = false;

            pointpointmove.StartCoroutine(pointpointmove.StartMoveDown()); // Start move down coroutine
        }
        else if (!brakeAway && hasGameStarted && speed < 10f && velocity > 4f)
        {
            speed += 0.004f; // Gradually increase speed
        }
        else if (!brakeAway && hasGameStarted && speed < 10f)
            speed += 0.0005f; // Gradually increase speed

        // Apply movement and forces if the game has started and object hasn't broken away
        if (hasGameStarted && !brakeAway)
        {
            Time.timeScale = 0.7f; // Adjust game time scale
            float moveHorizontal = Input.GetAxis("Horizontal"); // Get horizontal input
            float moveVertical = Input.GetAxis("Vertical"); // Get vertical input

            if (!brakeAway)
            {
                Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical); // Create movement vector

                rb.AddForce(movement * speed); // Apply movement force
                rb.AddForce(Vector3.down * 15f); // Apply downward force
            }
            else
            {
                moveHorizontal = 0;
                moveVertical = 0;
            }
        }
    }

    void ChangeDirection()
    {
        // Change direction at intervals if the game has started and object hasn't broken away
        if (hasGameStarted && !brakeAway)
        {
            float randomDirectionX = UnityEngine.Random.Range(-1f, 1f); // Generate random direction
            Vector3 randomDirection = new Vector3(randomDirectionX, 0f, 0f).normalized; // Normalize direction
            rb.velocity = randomDirection * speed / 3; // Set new velocity
        }
    }

    void HelpChecker()
    {
        // Check if help is needed based on conditions
        if (hasGameStarted && !brakeAway && velocity < 5f && helpTimer > 15f && canRedo)
        {
            HelpingHeand.SetActive(true); // Activate helping hand
            helpingText.SetActive(true); // Activate helping text
            if (velocity > 5)
                canRedo = false; // Disable redo if velocity is above 5
        }
        else
        {
            HelpingHeand.SetActive(false); // Deactivate helping hand
            helpingText.SetActive(false); // Deactivate helping text
        }
    }
}