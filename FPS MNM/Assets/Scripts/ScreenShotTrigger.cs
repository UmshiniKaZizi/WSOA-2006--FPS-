using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotTrigger : MonoBehaviour
{
    public ScreenShot screenShotComponent;

    void Update()
    {
        // Take a screenshot when the player presses the "P" key
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Define the path where the screenshot will be saved
            string path = Application.dataPath + "/Screenshot.png";

            // Call the TakeScreenshot method from the ScreenShot component
            screenShotComponent.TakeScreenshot(path);

            Debug.Log("Screenshot taken and saved to: " + path);
        }
    }
}
