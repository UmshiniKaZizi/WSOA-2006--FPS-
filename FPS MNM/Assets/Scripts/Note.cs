using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private GameObject interactionCanvas; 
    [SerializeField]
    private Text interactionText; 

    [Header("Interaction Info")]
    [SerializeField]
    [TextArea]
    private string interactionMessage; 

    private void Start()
    {
        interactionCanvas.SetActive(false); 
    }

    public void ShowInteractionUI()
    {
        interactionText.text = interactionMessage;
        interactionCanvas.SetActive(true); 
    }

    public void HideInteractionUI()
    {
        interactionCanvas.SetActive(false); 
    }

    public void Interact()
    {
        
        Debug.Log($"Interacted with: {gameObject.name}");
        
    }
}
