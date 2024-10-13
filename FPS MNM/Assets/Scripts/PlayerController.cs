using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public NoteController noteController; // Assign this in the Inspector

    private InputActionAsset inputActionAsset;
    private InputAction readNoteAction;  // Reference for reading the note
    private InputAction closeNoteAction; // Reference for closing the note

    private void Awake()
    {
        // Load the InputActionAsset
        inputActionAsset = Resources.Load<InputActionAsset>("Controls");

        if (inputActionAsset == null)
        {
            Debug.LogError("InputActionAsset not found. Ensure it is located in the Resources folder.");
            return; // Early exit if the asset isn't loaded
        }

        // Find actions in the InputActionAsset
        var playerMap = inputActionAsset.FindActionMap("Player");
        if (playerMap == null)
        {
            Debug.LogError("Player action map not found. Check the InputActionAsset configuration.");
            return; // Early exit if the action map isn't found
        }

        readNoteAction = playerMap.FindAction("Read");
        closeNoteAction = playerMap.FindAction("CloseNote");

        if (readNoteAction == null || closeNoteAction == null)
        {
            Debug.LogError("One or more input actions not found. Check the action map and action names.");
            return; // Early exit if actions aren't found
        }

        // Subscribe to input actions
        readNoteAction.performed += OnReadNotePerformed;
        closeNoteAction.performed += OnCloseNotePerformed;
    }

    private void OnEnable()
    {
        if (inputActionAsset != null) inputActionAsset.Enable();
        if (closeNoteAction != null) closeNoteAction.Enable();
    }

    private void OnDisable()
    {
        if (inputActionAsset != null) inputActionAsset.Disable();

        if (readNoteAction != null) readNoteAction.performed -= OnReadNotePerformed;
        if (closeNoteAction != null)
        {
            closeNoteAction.Disable();
            closeNoteAction.performed -= OnCloseNotePerformed;
        }
    }

    public void SetCanReadNote(bool canRead)
    {
        if (readNoteAction == null)
        {
            Debug.LogError("ReadNote action is not set up correctly.");
            return;
        }

        if (canRead)
        {
            Debug.Log("Read note action enabled");
            readNoteAction.Enable();
        }
        else
        {
            Debug.Log("Read note action disabled");
            readNoteAction.Disable();
        }
    }

    private void OnReadNotePerformed(InputAction.CallbackContext context)
    {
        if (noteController == null)
        {
            Debug.LogError("NoteController is null! Ensure it is assigned in the PlayerController.");
            return;
        }
        Debug.Log("Read note performed");
        noteController.ShowNote(); // Show the note if the player is in range
    }

    private void OnCloseNotePerformed(InputAction.CallbackContext context)
    {
        if (noteController == null)
        {
            Debug.LogError("NoteController is null! Ensure it is assigned in the PlayerController.");
            return;
        }

        //if (noteController.IsNoteOpen())
        {
            // noteController.DisableNote(); // Close the note if it's open
        }
    }
}
