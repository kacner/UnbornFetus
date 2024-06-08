using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterSpawner : MonoBehaviour
{
    QuakeMovement quakeMovement;

    public GameObject landingSplatter;
    public GameObject walkingSplatter;
    public Transform Point;

    public float waitingValue;
    private float waitTimer; // Add a timer variable

    private bool repater;

    void Start()
    {
        quakeMovement = GetComponent<QuakeMovement>();

        if (quakeMovement == null)
        {
            Debug.LogError("QuakeMovement component not found on " + gameObject.name);
        }
        waitTimer = waitingValue; // Initialize the wait timer
    }

    // Update is called once per frame
    void Update()
    {

        float timeTime = Time.deltaTime;
        if (quakeMovement != null && quakeMovement.CheckGround() != true)
        {
            repater = true;
        }

        if(repater)
        {
            if (quakeMovement != null && quakeMovement.CheckGround())
            {
                Instantiate(landingSplatter, Point.transform.position, landingSplatter.transform.rotation);
                repater = false;
            }
        }

            if (waitTimer > 0)
            {
                waitTimer -= timeTime; // Decrement the timer
                Debug.Log("timer :" + waitTimer);
            }
            else
            {
                if (quakeMovement != null && quakeMovement.CheckGround())
                    Instantiate(walkingSplatter, Point.transform.position, walkingSplatter.transform.rotation);
                waitTimer = waitingValue;
            }
    }
}
