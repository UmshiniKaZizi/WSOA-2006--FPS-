using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionEndGame : MonoBehaviour
{
    [Header("End Game Settings")]
    [SerializeField] private string interactionMessage = "Press 'E' to end the game"; 
    [SerializeField] private InputActionAsset inputActionAsset;  

    private InputAction interactAction;
    private bool isPlayerInRange = false;

    private void Awake()
    {
       
        interactAction = inputActionAsset.FindActionMap("Player").FindAction("Interact");

        if (interactAction == null)
        {
            Debug.LogError("Interact action not found in the Player action map.");
            return;
        }

        
        interactAction.performed += OnInteractPerformed;
    }

    private void OnEnable()
    {
        interactAction?.Enable();
    }

    private void OnDisable()
    {
        interactAction?.Disable();
        interactAction.performed -= OnInteractPerformed;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log(interactionMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Left interaction range.");
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        
        if (isPlayerInRange)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Over: Player interacted with the object.");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
