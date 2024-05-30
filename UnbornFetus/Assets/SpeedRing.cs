using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRing : MonoBehaviour
{
    private Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rb = other.GetComponentInParent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x * 1.5f, rb.velocity.y, rb.velocity.z * 1.5f);
        }
        else
            Debug.Log(other + "dosent have a rb");
    }

}
