using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarambitPickup : MonoBehaviour
{
    private QuakeMovement quakemovementScript;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        quakemovementScript = player.GetComponent<QuakeMovement>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("PlayerOBJ"))
        {
            quakemovementScript.EnableKarambit();
            Destroy(this);
        }
    }
}
