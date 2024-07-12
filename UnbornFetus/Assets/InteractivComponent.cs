using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivComponent : MonoBehaviour
{
    public PlayerInterractiv playerInterractiv;

    public AudioSource audioSystem;
    public AudioClip audioClip;

    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        playerInterractiv = FindObjectOfType<PlayerInterractiv>();

        if (playerInterractiv == null)
        {
            Debug.LogError("PlayerInterractiv component not found in the scene.");
        }

        // Check if playerInterractiv is null
        if (playerInterractiv == null)
        {
            Debug.LogError("PlayerInterractiv component not found on the same GameObject.");
        }

        // Check if audioSystem is assigned
        if (audioSystem == null)
        {
            Debug.LogError("AudioSource not assigned in the Inspector.");
        }

        // Optionally assign the audioClip to the audioSystem if not done in the Inspector
        if (audioSystem != null && audioClip != null)
        {
            audioSystem.clip = audioClip;
        }
    }

    void Update()
    {
        if (playerInterractiv != null && audioSystem != null)
        {
            if (playerInterractiv.TargetName == gameObject.name && !audioSystem.isPlaying)
            {
                audioSystem.Play();
                isPlaying = true;
            }
             
            if (!audioSystem.isPlaying)
            {
                isPlaying = false;
            }
        }
    }
}