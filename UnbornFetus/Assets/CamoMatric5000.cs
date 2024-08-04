using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamoMatric5000 : MonoBehaviour
{
    private bool isActive;

    public bool isFirstPov;

    public Transform firstPov;
    public Transform thurdPov;
    public MeshRenderer PlayerOBJMeshrenderer;

    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        UpdateCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchPov();
            Debug.LogWarning("switched");
        }
           /* if (Input.GetKeyDown(KeyCode.Q)) // firstperson
            {
                isActive = true;
                isFirstPov = true;
            }
            if (Input.GetKeyDown(KeyCode.E)) //thirdperson
            {
                isActive = true;
                isThurdPov = true;
            }
            UpdateCamera();*/
    }

    void UpdateCamera()
    {
        if (isActive)
        {
            if(isFirstPov)
            {
                transform.position = firstPov.position;
                PlayerOBJMeshrenderer.enabled = false;
            }
            else
            {
                transform.position = thurdPov.position;
                PlayerOBJMeshrenderer.enabled = true;
            }

            isActive = false;
        }
    }

    void SwitchPov()
    {
        isFirstPov = !isFirstPov;
        isActive = true;
        UpdateCamera();
    }
}
