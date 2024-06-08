using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PointPointMovie : MonoBehaviour
{
    public Transform moviePoint;
    public float moveSpeed;

    private float timTime;
    private Vector3 movement;

    public bool CanMovie;

    void Start()
    {
        CanMovie = false;   
    }

    // Update is called once per frame
    void Update()
    {
        timTime = Time.deltaTime;

        movement = new Vector3(Mathf.Lerp(transform.localScale.x, moviePoint.transform.position.x, timTime * moveSpeed),
                               Mathf.Lerp(transform.localScale.y, moviePoint.transform.position.y, timTime * moveSpeed),
                               Mathf.Lerp(transform.localScale.z, moviePoint.transform.position.z, timTime * moveSpeed));

        if (CanMovie)
            transform.position = movement;
    }
}
