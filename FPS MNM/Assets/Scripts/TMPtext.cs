using TMPro;
using UnityEngine;

public class TMPTest : MonoBehaviour
{
    public TextMeshPro textMeshPro;

    void Start()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = "Hello World!";
        }
    }
}
