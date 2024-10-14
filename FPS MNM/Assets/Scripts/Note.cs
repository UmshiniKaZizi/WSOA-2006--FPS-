using UnityEngine;
using TMPro;  // Import TextMeshPro namespace

public class InteractableObject : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private GameObject interactionCanvas;
    [SerializeField]
    private TextMeshProUGUI interactionText;  // Change to TextMeshProUGUI for TMP text

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
