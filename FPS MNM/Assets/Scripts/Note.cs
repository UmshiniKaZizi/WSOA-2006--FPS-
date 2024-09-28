using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private GameObject interactionCanvas; // Canvas to display interaction info
    [SerializeField]
    private Text interactionText; // Text component to display the object's info

    [Header("Interaction Info")]
    [SerializeField]
    [TextArea]
    private string interactionMessage; // Message to display when interacted with

    private void Start()
    {
        interactionCanvas.SetActive(false); // Hide the interaction UI at the start
    }

    public void ShowInteractionUI()
    {
        interactionText.text = interactionMessage;
        interactionCanvas.SetActive(true); // Show interaction UI
    }

    public void HideInteractionUI()
    {
        interactionCanvas.SetActive(false); // Hide interaction UI
    }

    // Call this function to perform the interaction (e.g., pick up the object)
    public void Interact()
    {
        // Logic for interacting with the object
        Debug.Log($"Interacted with: {gameObject.name}");
        // Implement further functionality, like picking up the object
    }
}
