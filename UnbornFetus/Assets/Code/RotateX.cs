using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public class RotateX : MonoBehaviour
{
    private AudioSource A_Source;
    public float rotationSpeed = 50f;
    private Rigidbody rb;
    void Update()
    {
        A_Source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DestroyAfterDelay(1f));
            Vector3 randomDirection = Random.onUnitSphere;
            rb.AddForce(randomDirection * 1.5f, ForceMode.Impulse);
        }
    }
    IEnumerator DestroyAfterDelay(float delay)
    {
        A_Source.Play();
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}