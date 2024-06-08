using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PedoMeter : MonoBehaviour
{
    private Vector3 lastPosition;
    private float totalDistance;
    public TextMeshProUGUI textMeshPro;

    void Start()
    {
        lastPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        InvokeRepeating("DOSTUFF", 0.5f, 0.5f);
    }

    private void DOSTUFF()
    {
        while(totalDistance < 999)
        { 
            Vector3 currentPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            float distanceThisFrame = Vector3.Distance(currentPosition, lastPosition);

            totalDistance += distanceThisFrame;

            lastPosition = currentPosition;

            totalDistance /= 100f; 

            textMeshPro.text = totalDistance.ToString("F0");
        }
        totalDistance = 0f;
        lastPosition = Vector3.zero;
    }
}
