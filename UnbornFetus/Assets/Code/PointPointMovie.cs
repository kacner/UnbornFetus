using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointPointMovie : MonoBehaviour
{
    public Transform moviePoint;
    public Transform ButtonsMovingPoint;
    public float moveSpeed;
    public GameObject Buttons;

    private float timTime;
    private float timTime2;
    private bool isMoving = false;
    private bool isQuitting = false;
    public Button startButton;
    public Button QuitButton;
    private bool HasgameStarted = false;

    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;

    public SKibidiBrainrot skibidibrainrot;
    public float exposureValue = 0.0f;
    public Transform target;

    private void Start()
    {
        if (globalVolume == null)
        {
            Debug.LogError("Global volume not assigned");
            return;
        }

        if (!globalVolume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogError("Color Adjustments component not found in the global volume");
            return;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            timTime += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(transform.position, moviePoint.position, timTime);

            Buttons.transform.position = Vector3.Lerp(Buttons.transform.position, ButtonsMovingPoint.position, timTime);

            if (timTime >= 1f)
            {
                isMoving = false;
            }
        }
        if (isQuitting)
        {
            timTime2 += Time.deltaTime * moveSpeed;

            Buttons.transform.position = Vector3.Lerp(Buttons.transform.position, ButtonsMovingPoint.position, timTime2);

            if (timTime2 >= 1f)
            {
                isMoving = false;
            }
        }
    }

    public void EnableMovement()
    {
        skibidibrainrot.hasGameStarted = true;
        startButton.enabled = false;
        QuitButton.enabled = false;
        isMoving = true;
        timTime = 0f;
        HasgameStarted = true;
    }

    public void EnableQuit()
    {
        StartCoroutine(Quit());
        startButton.enabled = false;
        QuitButton.enabled = false;
    }
    public IEnumerator FadeOut()
    {
        
        float startExposure = 0f;
        float endExposure = -10.0f;
        float lerpDuration = 2f;

        float startTime = Time.time;

        while (Time.time - startTime < lerpDuration)
        {
            float t = (Time.time - startTime) / lerpDuration;
            float lerpedExposure = Mathf.Lerp(startExposure, endExposure, t);
            colorAdjustments.postExposure.value = lerpedExposure;

            yield return null;
        }

        colorAdjustments.postExposure.value = endExposure;
    }
    public IEnumerator Quit()
    {
        isQuitting = true;

        yield return StartCoroutine(FadeOut());

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
            yield return null;
    }
    public IEnumerator MoveDown()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("UTERIS");
    }
    public IEnumerator StartMoveDown()
    {
        yield return StartCoroutine(MoveDown());
    }
}