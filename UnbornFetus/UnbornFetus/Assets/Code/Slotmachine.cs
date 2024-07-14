using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slotmachine : MonoBehaviour
{
    public GameObject LeftWheel;
    public GameObject MidWheel;
    public GameObject RightWheel;

    public float rotationDuration = 1.0f;
    public bool pulling = false;
    public bool cooldown = false;

    public int SpinsPerTime = 5;

    private Animator animator;

    public MainPlayerScripted quakemovement;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("pulling", pulling);
        if (Input.GetKey(KeyCode.E) && !cooldown && quakemovement.coinCount > 5f)
        {
            pulling = true;
            quakemovement.coinCount -= 5;
            cooldown = true;
            StartCoroutine(SpinWheels());
        }
        else if (quakemovement.coinCount < 5) 
        {
            Debug.Log("NO MONEY!!!!");
        }
    }

    IEnumerator SpinWheels()
    {
        yield return new WaitForSeconds(0.5f);

        // Randomize and round rotations for each wheel
        float leftRotation = Random.Range(180f, 360f);
        float roundedLeftRotation = RoundToNearestDivision(leftRotation, 45f);

        float midRotation = Random.Range(180f, 360f);
        float roundedMidRotation = RoundToNearestDivision(midRotation, 45f);

        float rightRotation = Random.Range(180f, 360f);
        float roundedRightRotation = RoundToNearestDivision(rightRotation, 45f);

        // Start rotating each wheel with delays
        StartCoroutine(RotateWheel(LeftWheel, roundedLeftRotation, 0f));
        StartCoroutine(RotateWheel(MidWheel, roundedMidRotation, 0.5f));
        StartCoroutine(RotateWheel(RightWheel, roundedRightRotation, 1f));

        // Wait for all rotations to complete
        yield return new WaitForSeconds(rotationDuration * SpinsPerTime + 1f);

        // Check win condition
        CheckWinCondition(roundedLeftRotation, roundedMidRotation, roundedRightRotation);

        pulling = false;
        StartCoroutine(CooldownDelay());
    }

    IEnumerator RotateWheel(GameObject wheel, float targetRotation, float delay)
    {
        yield return new WaitForSeconds(delay);

        float startRotationZ = wheel.transform.localRotation.eulerAngles.z;
        float elapsedTime = 0f;

        float adjustedRotationDuration = rotationDuration * Mathf.Abs(targetRotation) / 180f;

        while (elapsedTime < adjustedRotationDuration)
        {
            elapsedTime += Time.deltaTime;

            //float newRotation = Mathf.Lerp(startRotation, -targetRotation * SpinsPerTime, elapsedTime / adjustedRotationDuration);
            float newRotationZ = Mathf.Lerp(startRotationZ, -targetRotation * SpinsPerTime, elapsedTime / adjustedRotationDuration);

            //wheel.transform.rotation = Quaternion.Euler(0f, -90f, newRotation);
            wheel.transform.localRotation = Quaternion.Euler(0f, -90f, newRotationZ);

            yield return null;
        }
        //wheel.transform.rotation = Quaternion.Euler(0f, -90f, -targetRotation * SpinsPerTime);
        wheel.transform.localRotation = Quaternion.Euler(0f, -90f, -targetRotation * SpinsPerTime);
    }
    void CheckWinCondition(float leftRotation, float midRotation, float rightRotation)
    {
        //chanse of loosing ~~ 65% || chanse of winning ~~ 35%
        if (Mathf.Approximately(leftRotation, midRotation) && Mathf.Approximately(midRotation, rightRotation))
        {
            Debug.Log("Holy GYATTTTTT! All wheels match.");
            //100x the betted money
            quakemovement.coinCount += 500;
        }
        else if (Mathf.Approximately(leftRotation, midRotation) || Mathf.Approximately(midRotation, rightRotation) || Mathf.Approximately(rightRotation, leftRotation))
        {
            Debug.Log("Win! Two Wheels Match.");
            quakemovement.coinCount += 25;
        }
        else
        {
            Debug.Log("AW DaNG iT!");
        }
    }

    float RoundToNearestDivision(float value, float division)
    {
        return Mathf.Round(value / division) * division;
    }

    IEnumerator CooldownDelay()
    {
        yield return new WaitForSeconds(4f);
        cooldown = false;
    }
}
