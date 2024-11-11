using UnityEngine;

public class InteractionActivator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject targetObject;  // Object to be activated on interaction
    [SerializeField] private AudioClip interactionSound;  // Sound to play on interaction
    [SerializeField] private AudioSource audioSource;  // AudioSource to play the sound

    private bool hasInteracted = false;  // Track if interaction has occurred

    private void Start()
    {
        // Ensure the target object is inactive at the start
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }

        // Ensure audio source is set
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Interact()
    {
        // Check if interaction has already occurred to prevent multiple activations
        if (hasInteracted) return;

        hasInteracted = true;

        // Activate the target object
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            Debug.Log("Target object activated.");
        }

        // Play the interaction sound
        if (interactionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(interactionSound);
            Debug.Log("Interaction sound played.");
        }
    }
}
