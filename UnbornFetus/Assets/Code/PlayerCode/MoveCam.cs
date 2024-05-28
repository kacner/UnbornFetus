using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{

    public Transform cameraPos;
    void FixedUpdate()
    {
        transform.position = cameraPos.position;
    }
}
