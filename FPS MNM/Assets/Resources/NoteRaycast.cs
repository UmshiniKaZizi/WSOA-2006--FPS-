using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NoteRaycast : MonoBehaviour
{
    [Header("Raycast Features")]
    [SerializeField]
    private float raylength = 50f;  // Length of the raycast to detect interactable objects
    private Camera camera;  // Reference to the camera component
    private InteractableObject interactableObject;  // Currently detected interactable object

    [Header("Crosshair")]
    [SerializeField]
    private Image crosshair;  // UI element to show crosshair status

    [Header("Input Action Asset")]
    [SerializeField]
    private InputActionAsset inputActionAsset;  // Drag your Input Action Asset here

    private InputAction readNoteAction;  // Input action for reading a note

    private void Awake()
    {
        // Get the "Read" action from the action map in the InputActionAsset
        readNoteAction = inputActionAsset.FindActionMap("Player").FindAction("Read");

        if (readNoteAction == null)
        {
            Debug.LogError("Read action not found in the Player action map.");
        }
    }

    private void OnEnable()
    {
        camera = GetComponent<Camera>();  // Initialize the camera reference
    }

    private void OnDisable()
    {
        DisableReadInput();
    }

    void Update()
    {
        // Perform a raycast from the center of the screen
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, raylength))
        {
            // Check if the object hit by the raycast is an interactable object
            var interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                // If the interactable object is a note and it's different from the current one, update it
                if (interactableObject != interactable)
                {
                    ClearNote();  // Clear the previous note
                    interactableObject = interactable;
                    EnableReadInput();  // Enable the Read input only when a note is detected
                    HighlightCrosshair(true);  // Highlight the crosshair
                    interactable.ShowInteractionUI();  // Show interaction UI for the note
                }
            }
            else
            {
                ClearNote();  // Clear if the object is not interactable
            }
        }
        else
        {
            ClearNote();  // Clear if no object is detected
        }
    }

    private void OnReadNotePerformed(InputAction.CallbackContext context)
    {
        if (interactableObject != null)
        {
            // Try to get the NoteController component and show the note
            var noteController = interactableObject.GetComponent<NoteController>();
            if (noteController != null)
            {
                noteController.ShowNote();
            }
        }
    }

    private void EnableReadInput()
    {
        if (readNoteAction != null && !readNoteAction.enabled)
        {
            readNoteAction.Enable();
            readNoteAction.performed += OnReadNotePerformed;
        }
    }

    private void DisableReadInput()
    {
        if (readNoteAction != null && readNoteAction.enabled)
        {
            readNoteAction.Disable();
            readNoteAction.performed -= OnReadNotePerformed;
        }
    }

    private void ClearNote()
    {
        if (interactableObject != null)
        {
            HighlightCrosshair(false);  // Reset the crosshair color
            interactableObject.HideInteractionUI();  // Hide the interaction UI
            interactableObject = null;
        }

        DisableReadInput();  // Disable the read input when no note is in sight
    }

    private void HighlightCrosshair(bool on)
    {
        crosshair.color = on ? Color.red : Color.white;
    }
}
