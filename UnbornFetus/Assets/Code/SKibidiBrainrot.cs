using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SKibidiBrainrot : MonoBehaviour
{
    public float speed = 7f;
    private float changeInterval = 0.7f;

    public bool hasGameStarted;
    public bool brakeAway;

    private Rigidbody rb;
    private HingeJoint joint;
    private float velocity;
    public GameObject UmbilicalParticlesystem;
    public PointPointMovie pointpointmove;

    public float helpTimer;
    public GameObject HelpingHeand;
    public GameObject helpingText;
    private bool canRedo = true;

    void Start()
    {
        joint = GetComponent<HingeJoint>(); 
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("ChangeDirection", 0f, changeInterval);
        UmbilicalParticlesystem.SetActive(false);
        brakeAway = false;
        hasGameStarted = false;
        Time.timeScale = 0.5f;
        HelpingHeand.SetActive(false);
        helpingText.SetActive(false);

        InvokeRepeating("HelpChecker", 1, 1);
    }

    public void Update()
    {
        velocity = rb.velocity.magnitude;

        //Debug.Log(velocity);
        //Debug.Log(speed);
        Debug.Log(helpTimer);

        if (hasGameStarted)
        helpTimer += Time.deltaTime;

        if (velocity > 10f && !brakeAway && hasGameStarted)
        {
            UmbilicalParticlesystem.SetActive(true);
            Debug.Log("Disconnect called");
            brakeAway = true;
            transform.SetParent(null);
            Destroy(joint);
            rb.drag = 0f;
            rb.freezeRotation = false;
            pointpointmove.StartCoroutine(pointpointmove.StartMoveDown());
        }
        else if (!brakeAway && hasGameStarted && speed < 10f && velocity > 4f)
        {
            speed += 0.004f;
        }
        else if (!brakeAway && hasGameStarted && speed < 10f)
            speed += 0.0005f;

            if (hasGameStarted && !brakeAway)
            {
                Time.timeScale = 0.7f;
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");
                if(!brakeAway)
                {
                    Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

                    rb.AddForce(movement * speed);
                    rb.AddForce(Vector3.down * 15f);
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
    void HelpChecker()
    {
        if (hasGameStarted && !brakeAway && velocity < 5f && helpTimer > 15f && canRedo)
        {
            HelpingHeand.SetActive(true);
            helpingText.SetActive(true);
            if (velocity > 5)
                canRedo = false;
        }
        else
        {
            HelpingHeand.SetActive(false);
            helpingText.SetActive(false);
        }
    }
}