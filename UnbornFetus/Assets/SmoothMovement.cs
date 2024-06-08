using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SmoothMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        targetPosition = rb.position;
        targetRotation = rb.rotation;
    }

    void FixedUpdate()
    {
        // Smoothly move towards the target position and rotation
        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, Time.fixedDeltaTime * 10f));
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void SetTargetRotation(Quaternion rotation)
    {
        targetRotation = rotation;
    }
}
