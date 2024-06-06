using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class QuakeMovement : MonoBehaviour
{
    public float accel = 200f;
    public float airAccel = 200f;
    public float maxSpeed = 6.4f;
    public float maxAirSpeed = 0.6f;
    public float friction = 8f;
    public float jumpForce = 5f;
    private Vector3 movement;
    public Transform mainCamera;

    public LayerMask groundLeyer;
    public GameObject Cramernan;

    [Header("I kinda mighthave like Stole it a lil bit :3")]
    //changed up the variables a bit in case 
    //can you make a better job at it

    public bool IsGrounded = false;

    public GameObject Karambit;
    bool KarambitPickedup = false;

    public float rotationSpeed = 5f;
    private void Start()
    {
        if (KarambitPickedup == false)
        Karambit.SetActive(false);
    }
    private void Update()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > 1f)
        {
            FaceCamera();
        }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 playerVelocity = GetComponent<Rigidbody>().velocity;
        playerVelocity = CalculateFriction(playerVelocity);
        playerVelocity += CalculateMovement(input, playerVelocity);
        GetComponent<Rigidbody>().velocity = playerVelocity;
    }

    private Vector3 CalculateFriction(Vector3 currentVelocity)
    {
        IsGrounded = CheckGround();
        float speed = currentVelocity.magnitude;

        if (!IsGrounded || Input.GetButton("Jump") || speed == 0f)
            return currentVelocity;

        float drop = speed * friction * Time.deltaTime;
        return currentVelocity * (Mathf.Max(speed - drop, 0f) / speed);
    }

    private Vector3 CalculateMovement(Vector2 input, Vector3 velocity)
    {
        IsGrounded = CheckGround();

        float curAccel = accel;
        if (!IsGrounded)
            curAccel = airAccel;

        float curMaxSpeed = maxSpeed;

        if (!IsGrounded)
            curMaxSpeed = maxAirSpeed;

        Vector3 camRotation = new Vector3(0f, Cramernan.transform.rotation.eulerAngles.y, 0f);
        Vector3 inputVelocity = Quaternion.Euler(camRotation) *
                                new Vector3(input.x * curAccel, 0f, input.y * curAccel);

        Vector3 alignedInputVelocity = new Vector3(inputVelocity.x, 0f, inputVelocity.z) * Time.deltaTime;

        Vector3 currentVelocity = new Vector3(velocity.x, 0f, velocity.z);

        float max = Mathf.Max(0f, 1 - (currentVelocity.magnitude / curMaxSpeed));

        float velocityDot = Vector3.Dot(currentVelocity, alignedInputVelocity);

        Vector3 modifiedVelocity = alignedInputVelocity * max;

        Vector3 correctVelocity = Vector3.Lerp(alignedInputVelocity, modifiedVelocity, velocityDot);

        correctVelocity += GetJumpVelocity(velocity.y);

        return correctVelocity;
    }

    private Vector3 GetJumpVelocity(float yVelocity)
    {
        Vector3 jumpVelocity = Vector3.zero;

        if (Input.GetButton("Jump") && CheckGround())
        {
            jumpVelocity = new Vector3(0f, jumpForce - yVelocity, 0f);
        }

        return jumpVelocity;
    }

    public bool CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        bool result = Physics.Raycast(ray, GetComponent<Collider>().bounds.extents.y + 0.1f, groundLeyer);
        return result;
    }

    public void EnableKarambit()
    {
        Karambit.SetActive(true);
        KarambitPickedup = true;
    }

    void FaceCamera()
    {
        Vector3 directionToCamera = mainCamera.position - transform.position;
        directionToCamera.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
        
        //offset V
        Quaternion rotationOffset = Quaternion.Euler(0, -90f, 0);

        targetRotation *= rotationOffset;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

/*The MIT License (MIT)

Copyright (c) 2015 Till W.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
}