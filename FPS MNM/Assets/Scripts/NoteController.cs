using UnityEngine;
using TMPro;  // Import TextMeshPro namespace
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;

public class NoteController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private GameObject noteCanvas;  // Shared note canvas for displaying the text
    [SerializeField]
    private TextMeshProUGUI NoteTextGO;  // TextMeshProUGUI for TMP text component
    [SerializeField]
    public GameObject interactionCanvas;  // UI Canvas for showing interaction prompt
    [SerializeField]
    public TextMeshProUGUI interactionText;  // TMP text component for interaction prompt

    [Header("Note Settings")]
    [SerializeField]
    private int noteID;  // Unique identifier for each note
    [SerializeField]
    [TextArea] private string NoteText;  // The text specific to this note
    [SerializeField]
    private UnityEvent open;
    [SerializeField]
    public string interactionMessage = "PRESS 'R' TO READ AND 'X' TO CLOSE.";  // Interaction message to set in the Inspector

    private bool isNoteOpen = false;
    private static NoteController currentlyOpenNote;  // Static reference to track the currently open note
    private static Dictionary<int, string> noteTexts = new Dictionary<int, string>();  // Static dictionary for note texts

    // InputActionAsset reference for disabling player movement while reading
    [SerializeField]
    private InputActionAsset inputActionAsset;

    private InputAction readNoteAction;  // Define a new action for reading the note
    private PlayerController playerController;

    private void Awake()
    {
        // Populate the dictionary with the note text for this instance
        if (!noteTexts.ContainsKey(noteID))
        {
            noteTexts.Add(noteID, NoteText);
        }
        else
        {
            Debug.LogWarning($"Note ID {noteID} is already in use. Make sure each note has a unique ID.");
        }

        interactionCanvas.SetActive(false);
        noteCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        var closeNoteAction = inputActionAsset.FindActionMap("Player").FindAction("CloseNote");
        closeNoteAction.Enable();
        closeNoteAction.performed += OnClosePerformed;

        if (readNoteAction != null)
        {
            readNoteAction.Enable();
        }
    }

    private void OnDisable()
    {
        var closeNoteAction = inputActionAsset.FindActionMap("Player").FindAction("CloseNote");
        closeNoteAction.Disable();
        closeNoteAction.performed -= OnClosePerformed;

        if (readNoteAction != null)
        {
            readNoteAction.Disable();
        }

        if (playerController != null)
        {
            playerController.SetCanReadNote(false);
        }
    }

    public void ShowNote()
    {
        // Close the previously open note, if any
        if (currentlyOpenNote != null && currentlyOpenNote != this)
        {
            currentlyOpenNote.DisableNote();
        }

        // Update the text in the shared NoteTextGO TMP UI element using the noteID
        if (noteTexts.ContainsKey(noteID))
        {
            NoteTextGO.text = noteTexts[noteID];
        }
        else
        {
            NoteTextGO.text = "Note text not found.";
        }

        // Show the note canvas
        noteCanvas.SetActive(true);
        open.Invoke();
        isNoteOpen = true;

        // Mark this note as the currently open one
        currentlyOpenNote = this;

        DisablePlayerMovement(true);
        HideInteractionUI();  // Hide interaction UI when note is opened
    }

    private void DisableNote()
    {
        // Clear the text to avoid leftover content when opening another note
        NoteTextGO.text = "";

        // Hide the note canvas
        noteCanvas.SetActive(false);
        isNoteOpen = false;

        DisablePlayerMovement(false);
        currentlyOpenNote = null;
    }

    private void DisablePlayerMovement(bool disable)
    {
        if (disable)
        {
            inputActionAsset.FindActionMap("Player").FindAction("Movement").Disable();
            inputActionAsset.FindActionMap("Player").FindAction("Look").Disable();
        }
        else
        {
            inputActionAsset.FindActionMap("Player").FindAction("Movement").Enable();
            inputActionAsset.FindActionMap("Player").FindAction("Look").Enable();
        }
    }

    public void OnClosePerformed(InputAction.CallbackContext context)
    {
        interactionCanvas.SetActive(true);
        if (isNoteOpen)
        {   
            DisableNote();
        }
    }

    private void ShowInteractionUI()
    {
        interactionText.text = interactionMessage;
        interactionCanvas.SetActive(true);
        interactionText.gameObject.SetActive(true);
    }

    private void HideInteractionUI()
    {
        //interactionCanvas.SetActive(false);
        interactionText.gameObject.SetActive(false);
    }
}
