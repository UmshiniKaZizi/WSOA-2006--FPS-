using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NoteRaycast : MonoBehaviour
{
    [Header("Raycast Features")]
    [SerializeField]
    private float raylength = 5f;
    private Camera camera;
    private NoteController noteController;

    [Header("Crosshair")]
    [SerializeField]
    private Image crosshair;

    
    private PlayerInput playerInput;

    private InputAction readNoteAction;

    private void Awake()
    {
        
        playerInput = GetComponent<PlayerInput>();

        
        readNoteAction = playerInput.actions["Read"];
    }

    private void OnEnable()
    {
        
        readNoteAction.Enable();
        readNoteAction.performed += OnReadNotePerformed; 
    }

    private void OnDisable()
    {
        // Disable the read action
        readNoteAction.Disable();
        readNoteAction.performed -= OnReadNotePerformed; // Unsubscribe from the action
    }

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        // Perform raycasting to detect notes
        if (Physics.Raycast(camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, raylength))
        {
            var readableItem = hit.collider.GetComponent<NoteController>();
            if (readableItem != null)
            {
                noteController = readableItem;
                HighlightCrosshair(true);
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
        if (noteController != null)
        {
            noteController.ShowNote(); 
        }
    }

    void ClearNote()
    {
        if (noteController != null)
        {
            HighlightCrosshair(false);
            noteController = null;
        }
    }

    void HighlightCrosshair(bool on)
    {
        crosshair.color = on ? Color.red : Color.white;
    }
}
