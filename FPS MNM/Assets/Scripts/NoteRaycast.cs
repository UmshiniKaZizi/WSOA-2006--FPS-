using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class RaycastInteraction : MonoBehaviour
{
    [Header("Raycast Features")]
    [SerializeField] private float rayLength = 50f;  // Length of the raycast
    [SerializeField] private Camera playerCamera;  // Reference to the camera component

    [Header("Crosshair")]
    [SerializeField] private Image crosshair;  // UI element to show crosshair status

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset inputActionAsset;  // Input actions asset

    [Header("Inventory")]
    [SerializeField] private Inventory inventory;  // Reference to the player's inventory

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI interactionText;

    private InputAction interactAction;  // Input action for interacting with objects
    private InputAction readNoteAction;  // Input action for reading notes

    private GameObject currentTarget;
    private InteractableObject interactableObject;  // Currently detected interactable object
    private NoteController noteObject;  // Currently detected note object

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;  // Assign the main camera if not set in the Inspector
        }

        // Get the "Interact" and "ReadNote" actions from the Player action map
        interactAction = inputActionAsset.FindActionMap("Player").FindAction("Interact");
        readNoteAction = inputActionAsset.FindActionMap("Player").FindAction("Read");

        if (interactAction == null || readNoteAction == null)
        {
            Debug.LogError("Interact or ReadNote action not found in the Player action map.");
        }
    }

    private void OnEnable()
    {
        interactAction?.Enable();
        readNoteAction?.Enable();
    }

    private void OnDisable()
    {
        interactAction?.Disable();
        readNoteAction?.Disable();
    }

    private void Update()
    {
        PerformRaycast();

        if (interactAction != null && interactAction.triggered)
        {
            OnInteractPerformed();
        }

        if (readNoteAction != null && readNoteAction.triggered)
        {
            OnReadNotePerformed();
        }
    }

    private void PerformRaycast()
    {
        // Raycast from the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
        {
            if (hit.collider.gameObject != currentTarget)
            {
                Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");  // Debug the hit object

                currentTarget = hit.collider.gameObject;
                HandleNewTarget(currentTarget);
            }
        }
        else
        {
            if (currentTarget != null)
            {
                Debug.Log("Raycast did not hit any target, clearing interaction.");
                ClearInteraction();
                ClearNote();
                currentTarget = null;
            }
        }
    }

    private void HandleNewTarget(GameObject target)
    {
        // Reset previous interaction
        ClearInteraction();
        ClearNote();

        // Handle the new interaction
        if (target.CompareTag("Key"))
        {
            Debug.Log("Hit a Key object.");
            SetInteractable(target.GetComponent<InteractableObject>());
        }
        else if (target.TryGetComponent(out NoteController newNote))
        {
            Debug.Log("Hit a Note object.");
            SetNoteObject(newNote);
        }
        else if (target.TryGetComponent(out InteractableObject newInteractable))
        {
            Debug.Log("Hit an InteractableObject.");
            SetInteractable(newInteractable);
        }
        else
        {
            Debug.Log("No valid interactable component found on target.");
        }
    }

    private void SetInteractable(InteractableObject newInteractable)
    {
        if (newInteractable != null)
        {
            interactableObject = newInteractable;
            HighlightCrosshair(true);
            interactionText.text = "Press 'E' to interact";
            interactionText.enabled = true;
            interactableObject.ShowInteractionUI();
        }
    }

    private void SetNoteObject(NoteController newNote)
    {
        if (newNote != null)
        {
            // Set the reference to the note object
            noteObject = newNote;

            // Highlight the crosshair
            HighlightCrosshair(true);

            // Set the interaction message on the note's UI
            newNote.interactionText.text = newNote.interactionMessage;

            // Activate the interaction canvas and text in the NoteController
            newNote.interactionCanvas.SetActive(true);
            newNote.interactionText.gameObject.SetActive(true);

            // Set the prompt text for interacting with the note
            interactionText.text = "PRESS 'R' TO READ AND 'X' TO CLOSE.";
            interactionText.enabled = true;

            Debug.Log("Note object detected. Interaction UI activated.");
        }
    }


    private void OnInteractPerformed()
    {
        if (interactableObject != null)
        {
            Debug.Log("Interacting with object: " + interactableObject.name);

            // Interacting with various objects
            if (interactableObject.TryGetComponent(out DoorController door))
            {
                if (inventory.HasKey(door.requiredKey))
                {
                    door.ToggleDoor(door.requiredKey);  // Pass the player's key to the door
                    Debug.Log("Door opened successfully.");
                }
                else
                {
                    Debug.Log("You do not have the correct key to open this door.");
                }
                return;
            }

            if (interactableObject.TryGetComponent(out DrawerController drawer))
            {
                drawer.ToggleDrawer();  // Toggle the drawer open or closed
                Debug.Log("Drawer toggled.");
                return;
            }

            if (interactableObject.TryGetComponent(out KeyItem key))
            {
                if (inventory != null)
                {
                    inventory.AddKey(key);  // Add the key to the inventory
                    Debug.Log("Key added to inventory successfully.");
                }
                else
                {
                    Debug.LogError("Inventory reference is null. Ensure the inventory component is assigned properly.");
                }

                Destroy(key.gameObject);  // Remove the key from the scene
                return;
            }

            if (interactableObject.TryGetComponent(out KeypadController keypad))
            {
                Debug.Log("Interacting with keypad.");
                keypad.ActivateKeypad();  // Activate the keypad interaction mode
                return;
            }
        }
        else
        {
            Debug.Log("No interactable object detected.");
        }
    }

    private void OnReadNotePerformed()
    {
        if (noteObject != null)
        {
            Debug.Log("Reading the note...");
            noteObject.ShowNote();
        }
        else
        {
            Debug.Log("No note detected to read.");
        }
    }

    private void ClearInteraction()
    {
        if (interactableObject != null)
        {
            HighlightCrosshair(false);  // Reset crosshair color
            interactionText.enabled = false;
            interactableObject.HideInteractionUI();  // Hide interaction UI
            interactableObject = null;
        }
    }

    private void ClearNote()
    {
        if (noteObject != null)
        {
            HighlightCrosshair(false);  // Reset crosshair color
            interactionText.enabled = false;
            noteObject = null;
        }
    }

    private void HighlightCrosshair(bool isActive)
    {
        if (crosshair != null)
        {
            crosshair.color = isActive ? Color.red : Color.white;
        }
    }
}
