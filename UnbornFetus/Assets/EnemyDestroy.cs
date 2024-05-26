using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    private QuakeMovement quakemovementScript;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        quakemovementScript = player.GetComponent<QuakeMovement>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && quakemovementScript.GetComponent<Rigidbody>().velocity.magnitude > 20)
        {
                Destroy(gameObject);
        }
    }
}
