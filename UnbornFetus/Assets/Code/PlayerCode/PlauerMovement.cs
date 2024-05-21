using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlauerMovement : MonoBehaviour
{

    [Header(":3")]

    public float moveSpeed;
    public float JumpForce;

    public LayerMask GroundLayer;
    private bool isGrounded = false;
    public GameObject rayObject;

    public Transform orientation;
    
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(rayObject.transform.position, Vector3.down, out hit, 0.5f, GroundLayer))
        {
            isGrounded = true;
        }
        else isGrounded = false;

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

            OwerInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OwerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, JumpForce, rb.velocity.z);
    }
}
