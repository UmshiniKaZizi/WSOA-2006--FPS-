using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class NoteController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private GameObject noteCanvas;  // Shared note canvas for displaying the text
    [SerializeField]
    private Text NoteTextGO;  // Shared UI Text object that displays the note content

    [SerializeField]
    [TextArea] private string NoteText;  // The text specific to this note
    [SerializeField]
    private UnityEvent open;

    private bool isNoteOpen = false;
    private static NoteController currentlyOpenNote;  // Static reference to track the currently open note

    // InputActionAsset reference for disabling player movement while reading
    [SerializeField]
    private InputActionAsset inputActionAsset;

    private InputAction readNoteAction;  // Define a new action for reading the note
    private PlayerController playerController;

    private void Awake()
    {
        // Find and cache the "ReadNote" action from the "Player" action map
        readNoteAction = inputActionAsset.FindActionMap("Player").FindAction("Read");
    }

    private void OnEnable()
    {
        // Enable "CloseNote" actions when the component is active
        inputActionAsset.FindActionMap("Player").FindAction("CloseNote").Enable();
        inputActionAsset.FindActionMap("Player").FindAction("CloseNote").performed += OnClosePerformed;
    }

    private void OnDisable()
    {
        // Disable "CloseNote" actions when the component is inactive
        inputActionAsset.FindActionMap("Player").FindAction("CloseNote").Disable();
        inputActionAsset.FindActionMap("Player").FindAction("CloseNote").performed -= OnClosePerformed;

        if (playerController != null)
        {
            playerController.SetCanReadNote(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("Player is in range to read note.");
                playerController.SetCanReadNote(true);  // Enable the read action when the player is in range
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerController != null)
        {
            Debug.Log("Player left the range of the note.");
            playerController.SetCanReadNote(false);  // Disable the read action when the player leaves range
            playerController = null;
        }
    }

    public void ShowNote()
    {
        // Close the previously open note, if any
        if (currentlyOpenNote != null && currentlyOpenNote != this)
        {
            currentlyOpenNote.DisableNote();
        }

        // Update the text in the shared NoteTextGO UI element
        NoteTextGO.text = NoteText;
        Debug.Log("Showing note with text: " + NoteText);

        // Show the note canvas
        noteCanvas.SetActive(true);
        open.Invoke();
        isNoteOpen = true;

        // Mark this note as the currently open one
        currentlyOpenNote = this;

        DisablePlayerMovement(true);
    }

    private void DisableNote()
    {
        // Hide the note canvas
        noteCanvas.SetActive(false);
        isNoteOpen = false;

        // Re-enable player movement
        DisablePlayerMovement(false);
    }

    private void DisablePlayerMovement(bool disable)
    {
        if (disable)
        {
            // Disable movement and looking around
            inputActionAsset.FindActionMap("Player").FindAction("Movement").Disable();
            inputActionAsset.FindActionMap("Player").FindAction("Look").Disable();
        }
        else
        {
            // Re-enable movement and looking around
            inputActionAsset.FindActionMap("Player").FindAction("Movement").Enable();
            inputActionAsset.FindAction("Look").Enable();
        }
    }

    public void OnClosePerformed(InputAction.CallbackContext context)
    {
        if (isNoteOpen)
        {
            DisableNote();
        }
    }
}
