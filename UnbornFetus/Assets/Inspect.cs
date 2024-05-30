using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspect : MonoBehaviour
{
    public float rotationSpeed = 360f;
    private bool Inspecting = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.F) && !Inspecting)
        {
            Inspecting = true;
            StartCoroutine(Inspec1());
        }
    }

    public IEnumerator Inspec1()
    {
        float totalRotation = 0f;

        while (totalRotation < 360f)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, -rotationAmount, 0);
            totalRotation += rotationAmount;

            yield return null; 
        }
        Inspecting = false;
    }
}
