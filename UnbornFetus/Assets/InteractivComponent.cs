using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractivComponent : MonoBehaviour
{
    PlayerInterractiv playerInteractive;

    [Header("Audio Interface")]
    public bool activateAudio; // Flag to activate audio
    public AudioSource audioSystem; // Reference to the AudioSource component
    public AudioClip audioClip; // Reference to the AudioClip to be played

    [Header("UI")]
    public GameObject uiText; // Reference to the UI text object

    private bool isPlaying = false; // Flag to check if the audio is playing
    private bool uiShow = false; // Flag to control UI visibility

    public RawImage CursorImage;

    // Start is called before the first frame update
    void Start()
    {
        uiShow = false; // Initialize UI visibility to false
        UIImplement(); // Call to update the UI visibility

        playerInteractive = FindObjectOfType<PlayerInterractiv>(); // Find the PlayerInteractive component in the scene

        if (playerInteractive == null)
        {
            Debug.LogError("PlayerInteractive component not found in the scene."); // Log an error if PlayerInteractive is not found
        }

        // Check if audioSystem is assigned
        if (audioSystem == null)
        {
            Debug.LogError("AudioSource not assigned in the Inspector."); // Log an error if AudioSource is not assigned
        }

        // Optionally assign the audioClip to the audioSystem if not done in the Inspector
        if (audioSystem != null && audioClip != null)
        {
            audioSystem.clip = audioClip; // Assign the audio clip to the audio source
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInteractive.Triggered)
        {
            if (activateAudio == true && playerInteractive.TargetName == gameObject.name)
            {
                uiShow = true; // Show UI if conditions are met
                UIImplement(); // Update the UI
                AudioImplement(); // Play the audio
            }
            else
            {
                uiShow = false; // Hide UI if conditions are not met
                UIImplement(); // Update the UI
            }
        }
        else
        {
            uiShow = false; // Hide UI if the player is not interacting
            UIImplement(); // Update the UI
        }
    }

    // Method to update the UI visibility
    void UIImplement()
    {
        if (uiShow)
        {
            CursorImage.color = Color.green;
            uiText.SetActive(true); // Show the UI text
        }
        else
        {
            uiText.SetActive(false); // Hide the UI text
            CursorImage.color = Color.white;
        }
    }

    // Method to handle audio playback
    void AudioImplement()
    {
        if (playerInteractive != null && audioSystem != null && Input.GetKeyDown("e")) // Check if the 'e' key is pressed
        {
            Debug.LogWarning("Audio Active");

            if (playerInteractive.TargetName == gameObject.name && !audioSystem.isPlaying) // Check if the correct object is interacted with and audio is not playing
            {
                audioSystem.Play(); // Play the audio
                isPlaying = true; // Set the flag to true
                uiShow = false; // Hide the UI
                UIImplement(); // Update the UI
            }

            if (!audioSystem.isPlaying) // Check if the audio is not playing
            {
                isPlaying = false; // Set the flag to false
                uiShow = true; // Show the UI
                UIImplement(); // Update the UI
            }
        }
    }
}