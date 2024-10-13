using System.Collections;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    [Header("Drawer Movement Settings")]
    [SerializeField] private Vector3 openPositionOffset = new Vector3(0.0f, 0.0f, 0.5f);  
    [SerializeField] private float animationSpeed = 2f; 

    private Vector3 closedPosition; 
    private Vector3 openPosition;   
    private bool isOpen = false;     

    private void Start()
    {
        
        closedPosition = transform.localPosition;
        openPosition = closedPosition + openPositionOffset;
    }

    public void ToggleDrawer()
    {
       
        StopAllCoroutines();
               
        Vector3 targetPosition = isOpen ? closedPosition : openPosition;
        StartCoroutine(MoveDrawer(targetPosition));
        isOpen = !isOpen;
    }

    private IEnumerator MoveDrawer(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * animationSpeed);
            yield return null;
        }

       transform.localPosition = targetPosition;
    }
}
