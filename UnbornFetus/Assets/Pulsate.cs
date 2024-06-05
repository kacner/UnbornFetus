using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsate : MonoBehaviour
{
    public float changefactor = 1.25f;
    private Vector3 originalScale;
    private float pulse = 2.0f;
    private float timer = 0.0f;

    private void Start()
    {
        originalScale = this.transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime * pulse;
        float scaleMultiplier = Mathf.Lerp(1.0f, changefactor, (Mathf.Sin(timer) + 1.0f) / 2.0f);
        this.transform.localScale = originalScale * scaleMultiplier;
    }
}
