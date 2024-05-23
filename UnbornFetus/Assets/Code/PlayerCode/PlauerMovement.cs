using UnityEngine;

public class PlauerMovement : MonoBehaviour
{

    [Header(":3")]

    public float moveSpeed;
    public float JumpForce;

    public LayerMask GroundLayer;
    public bool isGrounded = false;
    public GameObject rayObject;

    public Transform orientation;
    
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    public float gravityScale = 2f;
    public float globalGravity = -9.82f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(rayObject.transform.position, Vector3.down, 1f, GroundLayer))
            isGrounded = true;
        else
            isGrounded = false;

        if (Input.GetKeyDown(KeyCode.LeftControl) && rb.velocity.y < 0f)
        {
            Vector3 gravity = globalGravity * gravityScale * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        
        if (isGrounded == false && Input.GetMouseButton(0))
        {
            rb.AddForce(transform.up * JumpForce * 1.5f);
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
