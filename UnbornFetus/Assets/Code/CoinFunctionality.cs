using System.Collections;
using UnityEngine;
public class CoinFunctionality : MonoBehaviour
{
    private AudioSource A_Source;
    public bool shouldSonicCoinFX = true;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        A_Source = GetComponent<AudioSource>();
    }
    public void collition()
    { 
            if (shouldSonicCoinFX) 
            {
                StartCoroutine(DestroyAfterDelay(1f));
                Vector3 randomDirection = Random.onUnitSphere;
                rb.AddForce(randomDirection * 1.5f, ForceMode.Impulse);
            }
            else
                StartCoroutine(DestroyAfterDelay(0.3f));
    }
    IEnumerator DestroyAfterDelay(float delay)
    {
        A_Source.Play();
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}