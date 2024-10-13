using TMPro;
using UnityEngine;

public class KeypadController : MonoBehaviour
{
    [Header("Keypad Settings")]
    [SerializeField] private string correctCode = "1234";  // The correct code for the door
    [SerializeField] private DoorController door;  // Reference to the door to be opened
    [SerializeField] private TextMeshPro textMeshProDisplay;  // Text display for showing the entered code

    private string enteredCode = "";  // Track the entered code
    private bool isActive = false;  // Track if the keypad is currently active

    private void Start()
    {
        UpdateDisplay();  // Initialize display to be empty
        Debug.Log("Keypad initialized. Waiting for activation.");
    }

    // This method will be called when the player interacts with the keypad
    public void ActivateKeypad()
    {
        isActive = true;
        Debug.Log("Keypad activated. Ready for input.");
        UpdateDisplay();
    }

    // This method is called to add a digit to the entered code
    public void AddDigit(string digit)
    {
        if (!isActive)
        {
            Debug.LogWarning("Keypad is inactive. Cannot add digit.");
            return;  // Only add digits if the keypad is active
        }

        if (enteredCode.Length < 4)  // Limit the entered code to 4 characters
        {
            enteredCode += digit;
            Debug.Log($"Digit '{digit}' added. Current entered code: {enteredCode}");
            UpdateDisplay();
        }
        else
        {
            Debug.LogWarning("Maximum code length reached. Cannot add more digits.");
        }
    }

    // This method is called to clear the code
    public void ClearCode()
    {
        if (!isActive)
        {
            Debug.LogWarning("Keypad is inactive. Cannot clear code.");
            return;  // Only clear if the keypad is active
        }

        enteredCode = "";  // Clear the entered code
        Debug.Log("Code cleared.");
        UpdateDisplay();  // Update the display
    }

    // This method is called when the player presses Enter
    public void EnterCode()
    {
        if (!isActive)
        {
            Debug.LogWarning("Keypad is inactive. Cannot enter code.");
            return;  // Only submit if the keypad is active
        }

        if (enteredCode == correctCode)  // If the entered code matches the correct one
        {
            Debug.Log("Correct code entered! Opening door...");
            door.OpenDoorFromKeypad();  // Use the DoorController's keypad method to open the door
            ClearCode();  // Clear the code after successfully opening
            DeactivateKeypad();  // Deactivate keypad after success
        }
        else
        {
            Debug.LogWarning("Incorrect code entered. Try again.");
            ClearCode();  // Clear the code if incorrect
        }
    }

    // Update the TextMeshPro display to show the current entered code
    private void UpdateDisplay()
    {
        if (textMeshProDisplay != null)
        {
            textMeshProDisplay.text = enteredCode;
            Debug.Log($"Display updated: {enteredCode}");
        }
        else
        {
            Debug.LogError("TextMeshPro display reference is null.");
        }
    }

    // Deactivate keypad interaction after use
    public void DeactivateKeypad()
    {
        isActive = false;
        Debug.Log("Keypad deactivated.");
        UpdateDisplay();
    }
}
