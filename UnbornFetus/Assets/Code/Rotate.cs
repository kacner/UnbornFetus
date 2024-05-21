using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 roationspeed = new Vector3(15, 30, 45);
    void Update()
    {
        Vector3 Cuberotation = roationspeed * Time.deltaTime;

        transform.Rotate(Cuberotation);
    }
}
