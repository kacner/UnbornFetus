using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShakeFOV : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Color startingColor;
    public float alpha = 0.5f;

    public QuakeMovement quakemovment;
    public float baseFOV = 60f;
    public float maxFOVChange = 30f;
    public float shakeIntensity = 0.1f;
    public float shakeFrequency = 25f;
    private Camera cam;
    private Vector3 originalPosition;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        quakemovment = player.GetComponent<QuakeMovement>();
        cam = GetComponent<Camera>();
        originalPosition = cam.transform.localPosition;


        Color particleColor = new Color(startingColor.r, startingColor.g, startingColor.b, alpha);
        var mainModule = particleSystem.main;
        mainModule.startColor = particleColor;
    }

    // Update is called once per frame
    void Update()
    {
        Mathf.Clamp(alpha, 0, 1);
        alpha = quakemovment.GetComponent<Rigidbody>().velocity.magnitude / 10f;
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
           Debug.Log("x = velocity " + velocity + "|| alpha becomes " + alpha);

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