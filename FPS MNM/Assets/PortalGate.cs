using UnityEngine;

public class PortalGate : MonoBehaviour
{
    private Vector3 savedPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SavePlayerLocation();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TeleportToSavedLocation();
        }
    }

    void SavePlayerLocation()
    {
        savedPosition = transform.position;
        PlayerPrefs.SetFloat("SavedX", savedPosition.x);
        PlayerPrefs.SetFloat("SavedY", savedPosition.y);
        PlayerPrefs.SetFloat("SavedZ", savedPosition.z);
        PlayerPrefs.Save();
        Debug.Log("Location Saved: " + savedPosition);
    }

    void TeleportToSavedLocation()
    {
        if (PlayerPrefs.HasKey("SavedX") && PlayerPrefs.HasKey("SavedY") && PlayerPrefs.HasKey("SavedZ"))
        {
            float x = PlayerPrefs.GetFloat("SavedX");
            float y = PlayerPrefs.GetFloat("SavedY");
            float z = PlayerPrefs.GetFloat("SavedZ");
            transform.position = new Vector3(x, y, z);
            Debug.Log("Teleported to: " + transform.position);
        }
        else
        {
            Debug.Log("No saved location found.");
        }
    }
}
