using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActionAsset;
    private Door currentDoor = null;

    private void Awake()
    {
        // Initialize the Input Actions
        inputActionAsset.FindActionMap("Player").FindAction("PickUp");
    }

    private void OnEnable()
    {
        // Enable the Interact action
       
        inputActionAsset.FindActionMap("Player").FindAction("PickUp").performed += OnInteract;
        inputActionAsset.FindActionMap("Player").FindAction("PickUp").Enable();
    }

    private void OnDisable()
    {
        // Disable the Interact action
       
        inputActionAsset.FindActionMap("Player").FindAction("PickUp").performed += OnInteract;
        inputActionAsset.FindActionMap("Player").FindAction("PickUp").Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (currentDoor != null)
        {
            currentDoor.ToggleDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered a door's interaction zone
        if (other.gameObject.CompareTag("Door"))
        {
            currentDoor = other.GetComponent<Door>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove reference when the player leaves the interaction zone
        if (other.gameObject.CompareTag("Door"))
        {
            if (currentDoor == other.GetComponent<Door>())
            {
                currentDoor = null;
            }
        }
    }
}
