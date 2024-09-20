using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class NoteController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private GameObject noteCanvas;
    [SerializeField]
    private Text NoteTextGO;

    [SerializeField]
    [TextArea] private string NoteText;
    [SerializeField]
    private UnityEvent open;

    private bool isNoteOpen = false;

    // Reference to the Input Action Asset
    [SerializeField]
    private InputActionAsset inputActionAsset; // Drag your Input Action Asset here

    private InputAction closeNoteAction;
    private InputAction readNoteAction;

    private void Awake()
    {
        // Get the input actions from the action map
        closeNoteAction = inputActionAsset.FindActionMap("Player").FindAction("CloseNote");
        readNoteAction = inputActionAsset.FindActionMap("Player").FindAction("Read");
    }

    private void OnEnable()
    {
        // Enable input actions
        closeNoteAction.Enable();
        closeNoteAction.performed += OnClosePerformed;

        readNoteAction.Enable();
        readNoteAction.performed += OnReadPerformed;
    }

    private void OnDisable()
    {
        // Disable input actions
        closeNoteAction.Disable();
        closeNoteAction.performed -= OnClosePerformed;

        readNoteAction.Disable();
        readNoteAction.performed -= OnReadPerformed;
    }

    private void OnReadPerformed(InputAction.CallbackContext context)
    {
        if (!isNoteOpen)
        {
            ShowNote();
        }
    }

    public void ShowNote()
    {
        // Show the note UI
        NoteTextGO.text = NoteText;
        noteCanvas.SetActive(true);
        open.Invoke();
        isNoteOpen = true;

        // Disable player movement and looking around
        DisablePlayerMovement(true);
    }

    private void OnClosePerformed(InputAction.CallbackContext context)
    {
        if (isNoteOpen)
        {
            DisableNote();
        }
    }

    private void DisableNote()
    {
        // Hide the note UI
        noteCanvas.SetActive(false);
        isNoteOpen = false;

        // Re-enable player movement and looking around
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
}
