using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterSpawner : MonoBehaviour
{
    QuakeMovement quakeMovement;

    public GameObject Spatter;
    public Transform Point;

    private bool repater;

    void Start()
    {
        quakeMovement = GetComponent<QuakeMovement>();
        if (quakeMovement == null)
        {
            Debug.LogError("QuakeMovement component not found on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(quakeMovement != null && quakeMovement.CheckGround() != true)
        {
            repater = true;
        }

        if(repater)
        {
            if (quakeMovement != null && quakeMovement.CheckGround())
            {
                Instantiate(Spatter, Point.transform.position, Spatter.transform.rotation);
                repater = false;
            }
        }


    }
}
