using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamoMatric5000 : MonoBehaviour
{
    private bool isActive;
    private bool isInactive;

    public bool isFirstPov;
    public bool isThurdPov;

    public Transform firstPov;
    public Transform thurdPov;

    // Start is called before the first frame update
    void Start()
    {
        if(isFirstPov)
            isThurdPov = false;
        else if(thurdPov)
            isFirstPov = false;

        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isActive = true;
            isFirstPov = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            isActive = true;
            isThurdPov = true;
        }
        Povchanger90somthingidk();
    }

    void Povchanger90somthingidk()
    {
        if (isActive)
        {
            if(isFirstPov)
            {
                transform.position = firstPov.position;
                isFirstPov = false;
            }
            if(isThurdPov)
            {
                transform.position = thurdPov.position;
                isThurdPov = false;
            }

            isActive = false;
            isInactive = true;
        }
    }
}
