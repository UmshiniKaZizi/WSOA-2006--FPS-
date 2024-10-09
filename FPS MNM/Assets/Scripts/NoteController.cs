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
    private InputAction closeNoteAction;

    private void Awake()
    {
        if (inputActionAsset == null)
        {
            Debug.LogError("InputActionAsset not assigned in the Inspector.");
            return;
        }

        // Find and cache the "Read" and "CloseNote" actions from the "Player" action map
        readNoteAction = inputActionAsset.FindActionMap("Player")?.FindAction("Read");
        closeNoteAction = inputActionAsset.FindActionMap("Player")?.FindAction("CloseNote");

        if (readNoteAction == null || closeNoteAction == null)
        {
            Debug.LogError("Read or CloseNote actions not found. Check the Input Action Map setup.");
        }
    }

    private void OnEnable()
    {
        if (inputActionAsset == null || readNoteAction == null || closeNoteAction == null)
        {
            return; // Skip if any input actions are null
        }

        // Enable "CloseNote" and "ReadNote" actions when the component is active
        closeNoteAction.Enable();
        closeNoteAction.performed += OnClosePerformed;

        readNoteAction.Enable();  // Enable the "ReadNote" action
        readNoteAction.performed += OnReadNotePerformed;  // Subscribe to the ReadNote action event
    }

    private void OnDisable()
    {
        if (inputActionAsset == null || readNoteAction == null || closeNoteAction == null)
        {
            return; // Skip if any input actions are null
        }

        // Disable "CloseNote" and "ReadNote" actions when the component is inactive
        closeNoteAction.Disable();
        closeNoteAction.performed -= OnClosePerformed;

        readNoteAction.Disable();
        readNoteAction.performed -= OnReadNotePerformed;
    }

    private void OnReadNotePerformed(InputAction.CallbackContext context)
    {
        // When the "ReadNote" button is pressed, show the note
        ShowNote();
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

    public void DisableNote()
    {
        // Hide the note canvas
        noteCanvas.SetActive(false);
        isNoteOpen = false;

        // Re-enable player movement
        DisablePlayerMovement(false);
    }

    private void DisablePlayerMovement(bool disable)
    {
        if (inputActionAsset == null) return;

        var playerMap = inputActionAsset.FindActionMap("Player");
        if (playerMap != null)
        {
            var movementAction = playerMap.FindAction("Movement");
            var lookAction = playerMap.FindAction("Look");

            if (movementAction != null)
            {
                if (disable)
                    movementAction.Disable();
                else
                    movementAction.Enable();
            }

            if (lookAction != null)
            {
                if (disable)
                    lookAction.Disable();
                else
                    lookAction.Enable();
            }
        }
    }


    private void OnClosePerformed(InputAction.CallbackContext context)
    {
        if (isNoteOpen)
        {
            DisableNote();
        }
    }

    public bool IsNoteOpen()
    {
        return isNoteOpen;
    }
}
