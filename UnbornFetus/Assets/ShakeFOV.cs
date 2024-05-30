using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class ShakeFOV : MonoBehaviour
{
    private GameObject player;
    public TMP_Text textMeshPro;
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

    void Start()
    {
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


        cam = GetComponent<Camera>();
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

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = "x = velocity " + quakemovment.GetComponent<Rigidbody>().velocity.magnitude.ToString("F2") + " || alpha becomes " + alpha.ToString("F2");
        alpha = Mathf.Clamp(quakemovment.GetComponent<Rigidbody>().velocity.magnitude / 10f, 0, 1);
        AdjustFOV(quakemovment.GetComponent<Rigidbody>().velocity.magnitude);
        ApplyCameraShake(quakemovment.GetComponent<Rigidbody>().velocity.magnitude);
        Speedlines(quakemovment.GetComponent<Rigidbody>().velocity.magnitude);
    }
    void AdjustFOV(float velocity)
    {
        float fovChange = Mathf.Clamp(velocity, 0, maxFOVChange);
        cam.fieldOfView = baseFOV + fovChange;
        if (cam.fieldOfView <= 0)
        {
            Mathf.Lerp(baseFOV, 0, fovChange);
        }
    }

    void ApplyCameraShake(float velocity)
    {
        if (velocity > 15)
        {
            float shakeAmount = Mathf.Sin(Time.time * shakeFrequency) * shakeIntensity * (velocity / 2);
            cam.transform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;
        }
        else
        {
            cam.transform.localPosition = originalPosition;
        }
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
        // Formel: f(x) = 1 / (1 + e^(-0.2 * (x - 40)))
        return 1f / (1f + Mathf.Exp(-0.2f * (velocity - 40f)));
    }
}