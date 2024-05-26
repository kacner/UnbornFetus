using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScale : MonoBehaviour
{
    public float targetScale = 10f;
    public float duration = 5f;

    private Vector3 initialScale;
    private float elapsedTime = 0f;
    private bool scalingUp = true;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (scalingUp)
        {
            float scaleFactor = Mathf.Lerp(1f, targetScale, elapsedTime / duration);
            transform.localScale = initialScale * scaleFactor;

            if (elapsedTime >= duration)
            {
                elapsedTime = 0f; 
                scalingUp = false; 
            }
        }
        else
        {
            float scaleFactor = Mathf.Lerp(targetScale, 1f, elapsedTime / duration);
            transform.localScale = initialScale * scaleFactor;

            if (elapsedTime >= duration)
            {
                elapsedTime = 0f;
                scalingUp = true; 
            }
        }
    }
}
