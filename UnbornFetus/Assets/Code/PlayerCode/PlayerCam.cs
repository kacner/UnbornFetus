using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerCam : MonoBehaviour
{
    public float snsX;
    public float snsY;

    public Transform orientation;

    float yRotation;
    float xRotation;

    public Transform cameraPos;


    private GameObject player;
    private ParticleSystem particleSystem;
    public Color startingColor;
    public float alpha = 0f;

    public QuakeMovement quakemovment;
    public float baseFOV = 60f;
    public float maxFOVChange = 30f;
    public float shakeIntensity = 0.1f;
    public float shakeFrequency = 25f;
    private Camera cam;
    private Vector3 originalPosition;

    private float currentVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            quakemovment = player.GetComponent<QuakeMovement>();
            if (quakemovment == null)
            {
                Debug.LogError("QuakeMovement component not found on player object.");
            }
        }
        else
            Debug.LogError("Player object not found with tag 'Player'.");

        cam = GetComponentInChildren<Camera>();
        if (cam != null)
            originalPosition = cam.transform.localPosition;
        else
            Debug.LogError("Camera component not found on this game object.");

        GameObject speedlinesObject = GameObject.FindGameObjectWithTag("Speedlines");
        if (speedlinesObject != null)
        {
            particleSystem = speedlinesObject.GetComponent<ParticleSystem>();
            if (particleSystem == null)
            {
                Debug.LogError("ParticleSystem component not found on Speedlines object.");
            }
        }
        else
            Debug.LogError("Speedlines object not found with tag 'Speedlines'.");

        Color particleColor = new Color(startingColor.r, startingColor.g, startingColor.b, alpha);
        var mainModule = particleSystem.main;
        mainModule.startColor = particleColor;
    }
    private void FixedUpdate()
    {
        currentVelocity = quakemovment.GetComponent<Rigidbody>().velocity.magnitude;
    }

    void Update()
    {
        transform.position = cameraPos.position;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * snsX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * snsY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        alpha = Mathf.Clamp(currentVelocity / 10f, 0, 1);

        // Camera shake and FOV
        if (currentVelocity > 0)
        {
            AdjustFOV(currentVelocity);
            ApplyCameraShake(currentVelocity);
        }
        else
        {
            cam.transform.localPosition = originalPosition;
        }

        Speedlines(currentVelocity);
    }

    void AdjustFOV(float velocity)
    {
        float fovChange = Mathf.Clamp(velocity, 0, maxFOVChange);
        cam.fieldOfView = baseFOV + fovChange;
        Mathf.Lerp(baseFOV, 0, fovChange);

    }

    void ApplyCameraShake(float velocity)
    {
        float shakeAmount = Mathf.Sin(Time.time * shakeFrequency) * shakeIntensity * (velocity / 2);
        cam.transform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;
    }

    void Speedlines(float velocity)
    {
        float alpha = CalculateAlpha(velocity);

        Color particleColor = new Color(startingColor.r, startingColor.g, startingColor.b, alpha);
        var mainModule = particleSystem.main;
        mainModule.startColor = particleColor;
    }

    float CalculateAlpha(float velocity)
    {

        //      Formel: f(x) = 1 / (1 + e^(-0.2 * (x - 40)))
        return 1f / (1f + Mathf.Exp(-0.2f * (velocity - 40f)));
    }
}
