using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RaycastInteraction : MonoBehaviour
{
    [Header("Raycast Features")]
    [SerializeField] private float rayLength = 50f;  // Length of the raycast
    [SerializeField] private Camera playerCamera;  // Reference to the camera component
    private InteractableObject interactableObject;  // Currently detected interactable object

    [Header("Crosshair")]
    [SerializeField] private Image crosshair;  // UI element to show crosshair status

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset inputActionAsset;  // Input actions asset

    private InputAction interactAction;  // Input action for interacting with objects

    [Header("Inventory")]
    [SerializeField] private Inventory inventory;  // Reference to the player's inventory

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;  // Assign the main camera if not set in the Inspector
        }

        // Get the "Interact" action from the Player action map
        interactAction = inputActionAsset.FindActionMap("Player").FindAction("Interact");

        if (interactAction == null)
        {
            Debug.LogError("Interact action not found in the Player action map.");
        }
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.Enable();
            interactAction.performed += OnInteractPerformed;
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.Disable();
            interactAction.performed -= OnInteractPerformed;
        }
    }

    private void Update()
    {
        PerformRaycast();
    }

    private void PerformRaycast()
    {
        // Raycast from the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
        {
            var hitObject = hit.collider.gameObject;

            // Prioritize key interaction over any other interactable object
            if (hitObject.CompareTag("Key"))
            {
                SetInteractable(hitObject.GetComponent<InteractableObject>());
            }
            else if (hitObject.GetComponent<InteractableObject>() != null && interactableObject == null)
            {
                // Only set a non-key interactable if no key is detected
                SetInteractable(hitObject.GetComponent<InteractableObject>());
            }
            else if (hitObject.GetComponent<InteractableObject>() == null)
            {
                ClearInteraction();
            }
        }
        else
        {
            ClearInteraction();
        }
    }

    private void SetInteractable(InteractableObject newInteractable)
    {
        if (newInteractable != null && newInteractable != interactableObject)
        {
            ClearInteraction();
            interactableObject = newInteractable;
            HighlightCrosshair(true);
            interactableObject.ShowInteractionUI();
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (interactableObject != null)
        {
            Debug.Log("Interacting with object: " + interactableObject.name);

            // Interacting with a door
            var door = interactableObject.GetComponent<DoorController>();
            if (door != null)
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

            // Interacting with a drawer
            var drawer = interactableObject.GetComponent<DrawerController>();
            if (drawer != null)
            {
                drawer.ToggleDrawer();  // Toggle the drawer open or closed
                Debug.Log("Drawer toggled.");
                return;
            }

            // Interacting with a key
            var key = interactableObject.GetComponent<KeyItem>();
            if (key != null)
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

            // Interacting with a keypad
            var keypad = interactableObject.GetComponent<KeypadController>();
            if (keypad != null)
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

    private void ClearInteraction()
    {
        if (interactableObject != null)
        {
            HighlightCrosshair(false);  // Reset crosshair color
            interactableObject.HideInteractionUI();  // Hide interaction UI
            interactableObject = null;
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
