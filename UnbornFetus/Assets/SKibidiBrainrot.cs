using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKibidiBrainrot : MonoBehaviour
{
    public float speed = 10f;
    private float changeInterval = 0.7f;

    public bool hasGameStarted;
    public bool brakeAway;

    private Rigidbody rb;
    private HingeJoint joint;

    void Start()
    {
        joint = GetComponent<HingeJoint>(); 
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("ChangeDirection", 0f, changeInterval);

        brakeAway = false;
        hasGameStarted = false;
    }

    public void Update()
    {

        float velocity = rb.velocity.magnitude;

        if (velocity > 10)
        {
            Debug.Log("Disconnect called");
            brakeAway = true;

            transform.SetParent(null);
            Destroy(joint);
            
            Vector3 movement = new Vector3(0f, 0.0f, 0f);
            speed = 0;

            rb.velocity = movement; 
            rb.AddForce(movement * speed);
        }

            if (hasGameStarted)
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");
                if(!brakeAway)
                {
                    Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

                    rb.AddForce(movement * speed);
                }
                else
                {
                    moveHorizontal = 0;
                    moveVertical = 0;
                }
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