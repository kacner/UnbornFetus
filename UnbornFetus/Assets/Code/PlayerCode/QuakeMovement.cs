using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuakeMovement : MonoBehaviour
{
    //I kinda mighthave like Stole it a lil bit :3
    //changed up the variables a bit in case 
    //can you make a better job at it

    QuakeMovement quakeMovement;

    [Header("Movement Settings")]
    public float accel = 200f;
    public float airAccel = 200f;
    public float maxSpeed = 6.4f;
    public float maxAirSpeed = 0.6f;
    public float friction = 8f;
    public float jumpForce = 5f;
    public bool IsGrounded = false;

    private Rigidbody rb;

    [Space]

    [Header("Camera Settings")]

    public GameObject Cameran;
    public Transform mainCamera;

    [Space]

    [Header("PickUps!")]
    public GameObject Karambit;
    public float rotationSpeed = 5f;
    public TextMeshProUGUI CoinCountTmp;

    private RotateX rotateXscript;
    private bool CanPickupSpeedCoin = true;
    private bool CanPickupCoin = true;
    private int coinCount = 0;
    private bool KarambitPickedup = false;

    [Space]

    [Header("PedoMeter Settings")]
    public TextMeshProUGUI textMeshPro;

    private Vector3 lastPosition;
    private float totalDistance;

    [Space]

    [Header("SplatterSettings")]
    public GameObject landingSplatter;
    public GameObject walkingSplatter;
    public Transform Point;
    public float waitingValue;

    private float waitTimer; // Add a timer variable
    private bool repater;

    [Space]

    [Header("Karambit Settings")]
    public float KarambitrotationSpeed = 360f;
    private bool Inspecting = false;
    private void Start()
    {
        quakeMovement = GetComponent<QuakeMovement>();

        if (quakeMovement == null)
        {
            Debug.LogError("QuakeMovement component not found on " + gameObject.name);
        }
        waitTimer = waitingValue; // Initialize the wait timer

        lastPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        InvokeRepeating("DOSTUFF", 0.5f, 0.5f);

        rb = GetComponent<Rigidbody>();

        if (KarambitPickedup == false)
        Karambit.SetActive(false);

        InvokeRepeating("UpdateCoinCount", 1, 1);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.F) && !Inspecting)
        {
            Inspecting = true;
            StartCoroutine(Inspec1());
        }

        float timeTime = Time.deltaTime;
        if (quakeMovement != null && quakeMovement.CheckGround() != true)
        {
            repater = true;
        }

        if (repater)
        {
            if (quakeMovement != null && quakeMovement.CheckGround())
            {
                Instantiate(landingSplatter, Point.transform.position, landingSplatter.transform.rotation);
                repater = false;
            }
        }

        if (waitTimer > 0)
        {
            waitTimer -= timeTime; // Decrement the timer
            Debug.Log("timer :" + waitTimer);
        }
        else
        {
            if (quakeMovement != null && quakeMovement.CheckGround())
                Instantiate(walkingSplatter, Point.transform.position, walkingSplatter.transform.rotation);
            waitTimer = waitingValue;
        }

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

        Vector3 camRotation = new Vector3(0f, Cameran.transform.rotation.eulerAngles.y, 0f);
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
        bool result = Physics.Raycast(ray, GetComponent<Collider>().bounds.extents.y + 0.1f, ~0, QueryTriggerInteraction.Ignore);
        return result;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpeedCoin" && CanPickupSpeedCoin)
        {
            CanPickupSpeedCoin = false;
            rotateXscript = other.GetComponent<RotateX>();
            rotateXscript.collition();
            rb.velocity = new Vector3(rb.velocity.x * 1.5f, rb.velocity.y, rb.velocity.z * 1.5f);
            StartCoroutine(SpeedCoinTimer());
        }
        if (!KarambitPickedup && other.gameObject.tag == "KarambitPickup")
        {
            rotateXscript = other.GetComponent<RotateX>();
            rotateXscript.collition();
            
            Karambit.SetActive(true);
            KarambitPickedup = true;
        }
        if (other.gameObject.tag == "Coin" && CanPickupCoin)
        {

            CanPickupCoin = false;
            rotateXscript = other.GetComponent<RotateX>();
            rotateXscript.collition();
            coinCount++;
            StartCoroutine(CoinTimer());
        }
    }

    private IEnumerator SpeedCoinTimer()
    {
        yield return new WaitForSeconds(0.5f);
        CanPickupSpeedCoin = true;
    }
    private IEnumerator CoinTimer()
    {
        yield return new WaitForSeconds(0.02f);
        CanPickupCoin = true;
    }

    void UpdateCoinCount()
    {
        CoinCountTmp.text = "coins : " + coinCount.ToString();
    }
    private void DOSTUFF()
    {

        Vector3 currentPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        float distanceThisFrame = Vector3.Distance(currentPosition, lastPosition);

        totalDistance += distanceThisFrame;

        lastPosition = currentPosition;

        textMeshPro.text = totalDistance.ToString("F0");

        totalDistance = 0f;
        lastPosition = Vector3.zero;
    }

    public IEnumerator Inspec1()
    {
        float totalRotation = 0f;

        while (totalRotation < 360f)
        {
            float rotationAmount = KarambitrotationSpeed * Time.deltaTime;
            Karambit.transform.Rotate(0, -rotationAmount, 0);
            totalRotation += rotationAmount;

            yield return null;
        }
        Inspecting = false;
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