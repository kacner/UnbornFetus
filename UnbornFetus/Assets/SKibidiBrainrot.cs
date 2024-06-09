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
    private float velocity;
    public GameObject UmbilicalParticlesystem;

    void Start()
    {
        joint = GetComponent<HingeJoint>(); 
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("ChangeDirection", 0f, changeInterval);
        UmbilicalParticlesystem.SetActive(false);
        brakeAway = false;
        hasGameStarted = false;
        Time.timeScale = 0.5f;
    }

    public void Update()
    {
        Debug.Log(rb.velocity.magnitude);
        velocity = rb.velocity.magnitude;

        if (velocity > 10f && !brakeAway)
        {
            UmbilicalParticlesystem.SetActive(true);
            Debug.Log("Disconnect called");
            brakeAway = true;
            transform.SetParent(null);
            Destroy(joint);
            rb.drag = 0f;
            rb.freezeRotation = false;
        }

            if (hasGameStarted)
            {
                Time.timeScale = 0.7f;
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
        if (hasGameStarted && !brakeAway)
        {
            float randomDirectionX = UnityEngine.Random.Range(-1f, 1f);
            Vector3 randomDirection = new Vector3(randomDirectionX, 0f, 0f).normalized;
            rb.velocity = randomDirection * speed / 3;
        }
    }
}