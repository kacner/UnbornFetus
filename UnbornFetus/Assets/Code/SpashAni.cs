using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpashAni : MonoBehaviour
{
    // Public variable to control the speed of scaling down and when to destroy object.
    public float speed;
    public float colapsValue;

    // Private variables for tracking time, scaling, and the object's state
    private float timeTime;
    private Vector3 scaling;
    private bool isGone;

    private void Start()
    {
    }

    void Update()
    {

        timeTime = Time.deltaTime;

        // Calculate the new scale values by interpolating between current scale and zero
        scaling = new Vector3(Mathf.Lerp(transform.localScale.x, 0, timeTime * speed),
                              Mathf.Lerp(transform.localScale.y, 0, timeTime * speed),
                              Mathf.Lerp(transform.localScale.z, 0, timeTime * speed));

        if (scaling.x < colapsValue) // Check if the new x scale is below a certain threshold to consider the object "gone"
        {
            isGone = true;
        }

        if (!isGone)// If the object is not "gone", update its scale
            transform.localScale = scaling;
        else if (isGone)// If the object is "gone", destroy the game object
            Destroy(gameObject);
    }
}
