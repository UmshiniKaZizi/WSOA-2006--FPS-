using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private string buttonValue;  // The value this button represents (0-9, Enter, Clear)
    [SerializeField] private KeypadController keypadController;  // Reference to the KeypadController

    private Vector3 originalPosition;
    private Color originalColor;
    private Renderer buttonRenderer;
    private float animationDuration = 0.2f;  // Duration for the button press animation

    private void Start()
    {
        // Store the original position and color of the button
        originalPosition = transform.localPosition;
        buttonRenderer = GetComponent<Renderer>();

        if (buttonRenderer != null)
        {
            originalColor = buttonRenderer.material.color;
        }
    }

    private void OnMouseDown()
    {
        // Start the button press animation
        StartCoroutine(AnimateButtonPress());

        // Handle the button logic (Add digit, enter code, or clear code)
        if (keypadController == null)
        {
            Debug.LogError("KeypadController reference is missing. Please assign it in the Inspector.");
            return;
        }

        if (buttonValue == "Enter")
        {
            keypadController.EnterCode();
        }
        else if (buttonValue == "Clear")
        {
            keypadController.ClearCode();
        }
        else
        {
            keypadController.ActivateKeypad();
            keypadController.AddDigit(buttonValue);
        }
    }

    private System.Collections.IEnumerator AnimateButtonPress()
    {
        // Animate the button moving inward and turning green
        float elapsedTime = 0f;

        Vector3 pressedPosition = originalPosition + new Vector3(0, -0.05f, 0);  // Move slightly downwards
        Color pressedColor = Color.green;  // Color when pressed

        while (elapsedTime < animationDuration / 2)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, pressedPosition, (elapsedTime / (animationDuration / 2)));
            if (buttonRenderer != null)
            {
                buttonRenderer.material.color = Color.Lerp(originalColor, pressedColor, (elapsedTime / (animationDuration / 2)));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the button reaches the pressed state
        transform.localPosition = pressedPosition;
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = pressedColor;
        }

        // Wait for a short duration before moving back
        yield return new WaitForSeconds(0.1f);

        // Animate the button moving back to the original position and color
        elapsedTime = 0f;

        while (elapsedTime < animationDuration / 2)
        {
            transform.localPosition = Vector3.Lerp(pressedPosition, originalPosition, (elapsedTime / (animationDuration / 2)));
            if (buttonRenderer != null)
            {
                buttonRenderer.material.color = Color.Lerp(pressedColor, originalColor, (elapsedTime / (animationDuration / 2)));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the button returns to the original state
        transform.localPosition = originalPosition;
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = originalColor;
        }
    }
}
