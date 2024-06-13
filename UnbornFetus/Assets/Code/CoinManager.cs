using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public GameObject[] coins;

    public float rotationSpeed = 50f;

    void Update()
    {
        foreach (GameObject coin in coins)
        {
            if (coin != null)
            {
                coin.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
