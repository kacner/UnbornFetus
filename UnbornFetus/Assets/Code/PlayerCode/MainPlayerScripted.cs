using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

/** 
 * ═════════ஜ۩۞۩ஜ═════════
 *       Code Shortcut
 *  
 *  Player Input Method(CFM)
 *  Calculate Frictrion Method (CFM)
 *  Calculate Movment Meothod (CMM)
 *  Get Jump Velocity Method (GPVM)
 *  Face Cam Method (FCM)
 *  DoStuff Method (DSM)
 *  Update Coin Count Method (UCCM)
 *  ReRunner Method (RRM)
 *  
 *  Jumpibil Objects Check Bool (JOCB)
 *  
 *  OnTrigger Colider (OC)
 *  
 *  Speed Coin Timer IEnumerator (SCTIE)
 *  CointTimer IEnumerator (CIE)
 *  Inspec1 IEnumerator (IIE)
 * 
 * ═════════ஜ۩۞۩ஜ═════════
 **/


public class MainPlayerScripted : MonoBehaviour
{
    // Inproting < Private Conponents >
    private Rigidbody rb;
    QuakeMovement quakeMovement;

    [Header("Movement Settings")]
    public float groundAcceleration = 200f;
    public float maxGroundSpeed = 6.4f;
    [Space]
    public float friction = 8f;
    [Space]
    public float maxAirSpeed = 0.6f;
    public float airAcceleration = 200f;
    public float jumpForce = 5f;
    public bool isGrounded = false;

    [Header("Camera Settings")]

    public GameObject Cameran;
    public Transform mainCamera;

    [Header("RayCast Settings")]
    public Vector3 rayDirection = Vector3.down; // Direction of the ray
    public float rayDistance = 0.1f; // Additional distance for the ray

    [Header("PickUps Settings")]

    public float rotationSpeed = 5f;
    public int coinCount = 0;

    [Space]

    public float KarambitrotationSpeed = 360f;

    [Space]

    public GameObject SlotmachineParent;
    public GameObject Karambit;
    public TextMeshPro CoinCountTmp;

    [Header("PedoMeter Settings")]
    public TextMeshProUGUI textMeshPro;

    private Vector3 lastPosition;
    private float totalDistance;

    private bool CanPickupSpeedCoin = true;
    private bool CanPickupCoin = true;
    private bool KarambitPickedup = false;
    private bool Inspecting = false;

    private CoinFunctionality coinFucntionality;

    // Start is called before the first frame update
    void Start()
    {
        quakeMovement = GetComponent<QuakeMovement>();
        rb = GetComponent<Rigidbody>();

        InvokeRepeating("UpdateCoinCount", 1, 1);
        InvokeRepeating("DOSTUFF", 0.5f, 0.5f);

        if (KarambitPickedup == false)
                Karambit.SetActive(false);

        lastPosition = new Vector3(transform.position.x, 0f, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        ReRunner();
        PlayerInput();

        Vector3 Velocity = GetComponent<Rigidbody>().velocity; // Collects pl*ayers curent velocity 
        Velocity += CalculateMovement(PlayerInput(), Velocity);
        GetComponent<Rigidbody>().velocity = Velocity;

        if (GetComponent<Rigidbody>().velocity.magnitude > 1f)
        {
            FaceCam();
        }


    }

    // Player Input Method(CFM)
    // Calculates Player Input into a vector 2
    public Vector2 PlayerInput()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        Vector2 playerMovmentInput = new Vector2(inputHorizontal, inputVertical);
        return playerMovmentInput;
    }

    // Calculate Frictrion Method (CFM)
    private Vector3 CalculateFriction(Vector3 currentVelocity)
    {
        // Check if the object is grounded
        isGrounded = JumpibleObjects();
        float speed = currentVelocity.magnitude;

        // Return the current velocity if not grounded, jumping, or stationary
        if (!isGrounded || Input.GetButton("Jump") || speed == 0f)
            return currentVelocity;

        // Calculate the speed drop due to friction and apply it to the current velocity
        float drop = speed * friction * Time.deltaTime;
        return currentVelocity * (Mathf.Max(speed - drop, 0f) / speed);
    }

    // Calculate Movment Meothod (CMM)
    private Vector3 CalculateMovement(Vector2 input, Vector3 currentVelocity)
    {    
        isGrounded = JumpibleObjects(); // Check if the player is grounded and update the isGrounded variable

        // Determine the acceleration and maximum speed based on whether the player is grounded
        float acceleration = isGrounded ? groundAcceleration : airAcceleration;
        float maxSpeed = isGrounded ? maxGroundSpeed : maxAirSpeed;

        // Get the yaw rotation from the camera and apply it to the input to get the desired velocity
        Vector3 cameraYawRotation = new Vector3(0f, Cameran.transform.rotation.eulerAngles.y, 0f);
        Vector3 desiredVelocity = Quaternion.Euler(cameraYawRotation) * new Vector3(input.x, 0f, input.y) * acceleration * Time.deltaTime;

        /** 
            Extract the horizontal component of the current velocity (ignore the vertical component) then
            Calculate the speed factor based on the current speed and the maximum speed then
            Calculate the alignment factor which indicates how much the desired direction aligns with the current direction then
            Blend the desired velocity and the speed-adjusted velocity based on the alignment factor
        **/
        Vector3 horizontalCurrentVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        float speedFactor = Mathf.Max(0f, 1 - (horizontalCurrentVelocity.magnitude / maxSpeed));
        float alignmentFactor = Vector3.Dot(horizontalCurrentVelocity, desiredVelocity);
        Vector3 blendedVelocity = Vector3.Lerp(desiredVelocity, desiredVelocity * speedFactor, alignmentFactor);

         blendedVelocity.y = CalculateJumpVelocity(currentVelocity.y).y;  // Add the vertical component (e.g., for jumping) to the final velocity

        return blendedVelocity; // Return the final calculated velocity
    }

    // Get Jump Velocity Method (GPVM)
    // Method to calculate the new velocity when jumping
    public Vector3 CalculateJumpVelocity(float currentYVelocity)
    {
        Vector3 jumpVelocity = Vector3.zero; // Initialize the jump velocity as zero

        // Check if the jump button is pressed and the player is on a jumpable object (e.g., grounded)
        if (Input.GetButton("Jump") && JumpibleObjects())
            jumpVelocity = new Vector3(0f, jumpForce - currentYVelocity, 0f); // Calculate the jump velocity needed to achieve the desired jump force, considering the current Y velocity

        return jumpVelocity; // Return the calculated jump velocity
    }

    // Jumpibil Objects Check Bool (JOCB)
    public bool JumpibleObjects()
    {
        // Create a ray starting from the object's position, pointing in the specified direction
        Ray ray = new Ray(transform.position, rayDirection);

        // Calculate the distance for the ray to travel then Perform a raycast to detect if there's a surface within the specified distance
        float distance = GetComponent<Collider>().bounds.extents.y + rayDistance;
        bool isJumpible = Physics.Raycast(ray, distance, ~0, QueryTriggerInteraction.Ignore);

        return isJumpible;
    }

    // Face Cam Method (FCM)
    void FaceCam()
    {
        // Compute the vector from the object to the camera and ensure rotation affects only the horizontal plane
        Vector3 directionToCamera = mainCamera.position - transform.position;
        directionToCamera.y = 0;

        // Determine the rotation needed to face the camera and apply an additional -90 degree rotation around the Y-axis
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
        Quaternion rotationOffset = Quaternion.Euler(0, -90f, 0);
        targetRotation *= rotationOffset;

        // Smoothly transition to the target rotation using Quaternion.Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // DoStuff Method (DSM)
    private void PedometerCalc()
    {
        // Get the current position with y set to 0 for horizontal calculations
        Vector3 currentPosition = new Vector3(transform.position.x, 0f, transform.position.z);

        // Calculate and add the distance moved this frame to the total distance
        float distanceThisFrame = Vector3.Distance(currentPosition, lastPosition);
        totalDistance += distanceThisFrame;

        // Update last position and display total distance
        lastPosition = currentPosition;
        textMeshPro.text = totalDistance.ToString("F0");

        // Reset total distance and last position
        totalDistance = 0f;
        lastPosition = Vector3.zero;
    }

    // Update Coin Count Method (UCCM)
    void UpdateCoinCount()
    {
        // Update the text component with the current coin count formatted as currency
        CoinCountTmp.text = "$" + coinCount.ToString();
    }

    // ReRunner Method (RRM)
    // To provent overloading when there is a void update
    public void ReRunner()
    {

        float timeTime = Time.deltaTime; // Updating time into a private veribile 

        // Cheking if player is inspecting 
        if (Input.GetKey(KeyCode.F) && !Inspecting)
        {
            Inspecting = true;
            StartCoroutine(Inspec1());
        }
    }

    // OnTrigger Colider (OC)
    private void OnTriggerEnter(Collider other)
    {
        // Get the CoinFunctionality component from the collided object
       /* coinFucntionality = other.GetComponent<CoinFunctionality>();
        coinFucntionality.collition(); // Call the collision function*/

        // Check if the collided object is a SpeedCoin and if the player can pick it up
        if (other.gameObject.tag == "SpeedCoin" && CanPickupSpeedCoin)
        {
            CanPickupSpeedCoin = false; // Disable picking up another speed coin
            // Increase the player's speed by modifying the Rigidbody's velocity
            rb.velocity = new Vector3(rb.velocity.x * 1.5f, rb.velocity.y, rb.velocity.z * 1.5f);
            StartCoroutine(SpeedCoinTimer(0.5f)); // Start a coroutine to reset speed coin pickup
        }

        // Check if the collided object is a KarambitPickup, if it hasn't been picked up, and if the slot machine is inactive
        if (!KarambitPickedup && other.gameObject.tag == "KarambitPickup" && !SlotmachineParent.activeSelf)
        {
            Karambit.SetActive(true); // Activate the Karambit
            KarambitPickedup = true; // Mark the Karambit as picked up
        }

        // Check if the collided object is a Coin and if the player can pick it up
        if (other.gameObject.tag == "Coin" && CanPickupCoin)
        {
           var script = other.GetComponent<CoinFunctionality>();
            script.collition();
            CanPickupCoin = false; // Disable picking up another coin
            coinCount++; // Increase the coin count
            StartCoroutine(CoinTimer(0.2f)); // Start a coroutine to reset coin pickup
        }
    }

    // Speed Coin Timer IEnumerator (SCTIE)
    // Coroutine to reset the ability to pick up speed coins after a delay
    IEnumerator SpeedCoinTimer(float time)
    {
        yield return new WaitForSeconds(time);
        CanPickupSpeedCoin = true;
    }

    // CointTimer IEnumerator (CIE)
    // Coroutine to reset the ability to pick up coins after a delay
    IEnumerator CoinTimer(float time)
    {
        yield return new WaitForSeconds(time);
        CanPickupCoin = true;
    }

    // Inspec1 IEnumerator (IIE)
    IEnumerator Inspec1()
    {
        float totalRotation = 0f;

        while (totalRotation < 360f)
        {
            float rotationAmount = KarambitrotationSpeed * Time.deltaTime;
            Karambit.transform.Rotate(0, -rotationAmount, 0);
            totalRotation += rotationAmount;

            yield return null;
        }
        Inspecting = false;
    }

}
