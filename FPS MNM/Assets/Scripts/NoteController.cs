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
            inputActionAsset.FindActionMap("Player").FindAction("Look").Enable();
        }
    }

    private void OnClosePerformed(InputAction.CallbackContext context)
    {
        if (isNoteOpen)
        {
            DisableNote();
        }
    }

    private void OnEnable()
    {
        inputActionAsset.FindActionMap("Player").FindAction("CloseNote").Enable();
        inputActionAsset.FindActionMap("Player").FindAction("CloseNote").performed += OnClosePerformed;
    }

    private void OnDisable()
    {
        inputActionAsset.FindActionMap("Player").FindAction("CloseNote").Disable();
        inputActionAsset.FindActionMap("Player").FindAction("CloseNote").performed -= OnClosePerformed;
    }
}
