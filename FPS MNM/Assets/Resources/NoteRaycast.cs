using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NoteRaycast : MonoBehaviour
{
    [Header("Raycast Features")]
    [SerializeField]
    private float raylength = 50f;
    private Camera camera;
    private InteractableObject interactableObject;

    [Header("Crosshair")]
    [SerializeField]
    private Image crosshair;

    [Header("Input Action Asset")]
    [SerializeField]
    private InputActionAsset inputActionAsset; // Drag your Input Action Asset here

    private InputAction readNoteAction;

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
        if (readNoteAction != null)
        {
            readNoteAction.Enable();
            readNoteAction.performed += OnReadNotePerformed;
        }
    }

    private void OnDisable()
    {
        if (readNoteAction != null)
        {
            readNoteAction.Disable();
            readNoteAction.performed -= OnReadNotePerformed;
        }
    }

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, raylength))
        {
            var interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                interactableObject = interactable;
                HighlightCrosshair(true);
                interactable.ShowInteractionUI(); // Show the interaction UI
            }
            else
            {
                ClearNote();
            }
        }
        else
        {
            ClearNote();
        }
    }

    private void OnReadNotePerformed(InputAction.CallbackContext context)
    {
        if (interactableObject != null)
        {
            interactableObject.Interact(); // Call the interact method
        }
    }

    void ClearNote()
    {
        if (interactableObject != null)
        {
            HighlightCrosshair(false);
            interactableObject.HideInteractionUI(); // Hide the interaction UI
            interactableObject = null;
        }
    }

    void HighlightCrosshair(bool on)
    {
        crosshair.color = on ? Color.red : Color.white;
    }
}
