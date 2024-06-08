using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKibidiBrainrot : MonoBehaviour
{
    public float speed = 10f;
     private float changeInterval = 0.7f;
    private Rigidbody rb;
    public bool hasGameStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("ChangeDirection", 0f, changeInterval);
    }

    void Update()
    {
        if(rb.velocity.magnitude > 10)
        {
           //make child dissconnect from parten which is the bone_end
        }
        Debug.Log(rb.velocity.magnitude);

        if (hasGameStarted)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            rb.AddForce(movement * speed);
        }
    }
    void ChangeDirection()
    {
        if (hasGameStarted)
        {
            float randomDirectionX = UnityEngine.Random.Range(-1f, 1f);
            Vector3 randomDirection = new Vector3(randomDirectionX, 0f, 0f).normalized;
            rb.velocity = randomDirection * speed / 3;
        }
    }
}