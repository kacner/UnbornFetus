using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedoMeter : MonoBehaviour
{
    public Rigidbody Player;
    public float MaxVelocity = 100.0f;
    public float MinTilt = -149f;
    public float MaxTilt = -28f;
    public RectTransform arrow;
    private float velocity = 0.0f;

    private void Update()
    {
        velocity = Player.velocity.magnitude;

        velocity = Mathf.Clamp(velocity, 0, MaxVelocity);

        float tilt = Mathf.Lerp(MinTilt, MaxTilt, velocity / MaxVelocity);

        arrow.localEulerAngles = new Vector3(0, 0, tilt);
    }
}
