using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private string buttonValue; 
    [SerializeField] private KeypadController keypadController;  

    private Vector3 originalPosition;
    private Color originalColor;
    private Renderer buttonRenderer;
    private float animationDuration = 0.2f;  

    private void Start()
    {
        
        originalPosition = transform.localPosition;
        buttonRenderer = GetComponent<Renderer>();

        if (buttonRenderer != null)
        {
            originalColor = buttonRenderer.material.color;
        }
    }

    private void OnMouseDown()
    {
        
        StartCoroutine(AnimateButtonPress());

       
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
       
        float elapsedTime = 0f;

        Vector3 pressedPosition = originalPosition + new Vector3(0, -0.05f, 0); 
        Color pressedColor = Color.green;  

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

        
        transform.localPosition = pressedPosition;
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = pressedColor;
        }

       
        yield return new WaitForSeconds(0.1f);

        
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

       
        transform.localPosition = originalPosition;
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = originalColor;
        }
    }
}
