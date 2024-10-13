using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<string> keys = new List<string>();

    public void AddKey(KeyItem key)
    {
        if (!keys.Contains(key.keyID))
        {
            keys.Add(key.keyID);
            Debug.Log("Key added to inventory: " + key.keyID);
        }
    }

    public bool HasKey(KeyItem key)
    {
        Debug.Log("Checking inventory for key: " + key.keyID);
        bool hasKey = keys.Contains(key.keyID);
        Debug.Log(hasKey ? "Key found in inventory" : "Key not found in inventory");
        return hasKey;
    }

}
