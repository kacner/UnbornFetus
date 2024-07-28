using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerCam : MonoBehaviour
{
    [Header("Camera Sensitivity")]
    public float snsX; // Sensitivity for the X-axis (horizontal)
    public float snsY; // Sensitivity for the Y-axis (vertical)

    [Header("Speed Flair Settings")]
    public Color startingColor; // Initial color for the speed lines
    public float alpha = 0f; // Alpha value for the speed lines

    [Header("Camera FOV Settings")]
    public float baseFOV = 60f; // Base Field of View for the camera
    public float maxFOVChange = 30f; // Maximum change in Field of View
    public float shakeIntensity = 0.1f; // Intensity of the camera shake
    public float shakeFrequency = 25f; // Frequency of the camera shake
    public bool Fistperson;
    public bool Thurdperson;

    [Header("References Objects")]
    public MainPlayerScripted mainplayerscripted; // Reference to the MainPlayerScripted component
    public Transform orientation; // Transform for the orientation
    public Transform cameraPosFirst; // First position of the camera 
    public Transform cameraPosThurd; // third position of the camera 

    public Transform CamTransformeationStravaganza;

    private Camera cam; // Camera component
    public Camera Overlaycam; // Camera component
    private Vector3 originalPosition; // Original position of the camera
    private float currentVelocity; // Current velocity of the player

    private float yRotation; // Rotation around the Y-axis
    private float xRotation; // Rotation around the X-axis

    private GameObject player; // Reference to the player object
    private ParticleSystem particleSystem; // Particle system for speed lines

    void Start()
    {
        // Hide Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FailsSafeRunner(); // Initialize and validate references

        // Set the initial color for the particle system
        Color particleColor = new Color(startingColor.r, startingColor.g, startingColor.b, alpha);
        var mainModule = particleSystem.main;
        mainModule.startColor = particleColor;

        if (Thurdperson)
            Fistperson = false;

        if (Fistperson)
            Thurdperson = false;
    }

    private void Awake()
    {
        Time.timeScale = 1f;
    }
    private void FixedUpdate()
    {
        // Update the current velocity based on the player's rigidbody velocity
        currentVelocity = mainplayerscripted.GetComponent<Rigidbody>().velocity.magnitude;
    }

    void Update()
    {
        PlayerCamra(); // Update the camera position and rotation

        // Camera shake and FOV adjustments when moving
        if (currentVelocity > 0)
        {
            AdjustFOV(currentVelocity); // Adjust Field of View
            ApplyCameraShake(currentVelocity); // Apply camera shake
        }
        else
            cam.transform.localPosition = originalPosition; // Reset camera position when not moving
        

        Speedlines(currentVelocity); // Update speed lines effect
        Campos();

    }

    void Campos()
    {
        // camra positon = Playerpos
        cam.transform.position = cameraPosFirst.position;

        // capra postion = offcenter playerpos
        Vector3 offCenterPosition = cameraPosThurd.position;
        cam.transform.position = offCenterPosition;
    }

    void PlayerCamra()
    {
        // Get user input for mouse movement
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * snsX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * snsY;

        // Update rotation based on mouse input
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to the camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // Toggle cursor visibility with Escape and Mouse0 keys
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void AdjustFOV(float velocity)
    {
        // Adjust the Field of View based on the current velocity
        float fovChange = Mathf.Clamp(velocity, 0, maxFOVChange);
        cam.fieldOfView = baseFOV + fovChange;
        Overlaycam.fieldOfView = baseFOV + fovChange;
        Mathf.Lerp(baseFOV, 0, fovChange);
    }

    void ApplyCameraShake(float velocity)
    {
        // Apply camera shake based on the current velocity
        float shakeAmount = Mathf.Sin(Time.time * shakeFrequency) * shakeIntensity * (velocity / 2);
        cam.transform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;
    }

    void Speedlines(float velocity)
    {
        // Update the alpha value of the speed lines based on the current velocity
        float alpha = 1f / (1f + Mathf.Exp(-0.2f * (velocity - 40f))); // Formel: f(x) = 1 / (1 + e^(-0.2 * (x - 40)))

        // Set the color of the particle system
        Color particleColor = new Color(startingColor.r, startingColor.g, startingColor.b, alpha);
        var mainModule = particleSystem.main;
        mainModule.startColor = particleColor;
    }

    void FailsSafeRunner()
    {
        // Find the player object and get the QuakeMovement component
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            mainplayerscripted = player.GetComponent<MainPlayerScripted>();
            if (mainplayerscripted == null)
                Debug.LogError("QuakeMovement component not found on player object.");
        }
        else
            Debug.LogError("Player object not found with tag 'Player'.");

        // Find the camera component
        cam = GetComponentInChildren<Camera>();
        if (cam != null)
            originalPosition = cam.transform.localPosition;
        else
            Debug.LogError("Camera component not found on this game object.");

        // Find the Speedlines object and get the ParticleSystem component
        GameObject speedlinesObject = GameObject.FindGameObjectWithTag("Speedlines");
        if (speedlinesObject != null)
        {
            particleSystem = speedlinesObject.GetComponent<ParticleSystem>();
            if (particleSystem == null)
                Debug.LogError("ParticleSystem component not found on Speedlines object.");
        }
        else
            Debug.LogError("Speedlines object not found with tag 'Speedlines'.");
    }
}