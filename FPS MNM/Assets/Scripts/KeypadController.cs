using TMPro;
using UnityEngine;

public class KeypadController : MonoBehaviour
{
    [Header("Keypad Settings")]
    [SerializeField] private string correctCode = "1234";  
    [SerializeField] private DoorController door;  
    [SerializeField] private TextMeshPro textMeshProDisplay;  

    private string enteredCode = ""; 
    private bool isActive = false;  

    private void Start()
    {
        UpdateDisplay();  
        Debug.Log("Keypad initialized. Waiting for activation.");
    }

   
    public void ActivateKeypad()
    {
        isActive = true;
        Debug.Log("Keypad activated. Ready for input.");
        UpdateDisplay();
    }

    
    public void AddDigit(string digit)
    {
        if (!isActive)
        {
            Debug.LogWarning("Keypad is inactive. Cannot add digit.");
            return;  
        }

        if (enteredCode.Length < 4)  
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

    
    public void ClearCode()
    {
        if (!isActive)
        {
            Debug.LogWarning("Keypad is inactive. Cannot clear code.");
            return;  
        }

        enteredCode = "";  
        Debug.Log("Code cleared.");
        UpdateDisplay();  
    }

    
    public void EnterCode()
    {
        if (!isActive)
        {
            Debug.LogWarning("Keypad is inactive. Cannot enter code.");
            return;  
        }

        if (enteredCode == correctCode)  
        {
            Debug.Log("Correct code entered! Opening door...");
            door.OpenDoorFromKeypad();  
            ClearCode();  
            DeactivateKeypad();  
        }
        else
        {
            Debug.LogWarning("Incorrect code entered. Try again.");
            ClearCode();  
        }
    }

    
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

    
    public void DeactivateKeypad()
    {
        isActive = false;
        Debug.Log("Keypad deactivated.");
        UpdateDisplay();
    }
}
