using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControls : MonoBehaviour
{

    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
                                   // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component
   
    
    [Header("SPRINTING SETTINGS")]
    [Space(5)]

    public float doubletaptime = 0.5f;
    public float movementspeedmultiplier = 2f;
    public float sprintTime = 4f;

    private float taptime;
    private bool  sprinting = false;
    
    

    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired
    public float pickUpRange = 5f; // Range within which objects can be picked up
    public bool holdingGun = false;
    private Weapon weapon;
    

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    private GameObject heldObject; // Reference to the currently held object


    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        var playerInput = new Controls();
        playerInput.Player.Enable();

        playerInput.Player.Movement.performed += ctx => {
            moveInput = ctx.ReadValue<Vector2>();
            Sprint();
        };
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        playerInput.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        playerInput.Player.Jump.performed += ctx => Jump();

        playerInput.Player.Shoot.performed += ctx => {
            if (weapon != null)
            {
                weapon.Shoot();
            }
            else
            {
                Debug.LogError("No weapon equipped!");
            }
        };

        playerInput.Player.PickUp.performed += ctx => PickUpObject();
    }


    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
       
    }

    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        float currentSpeed = sprinting ? moveSpeed * movementspeedmultiplier : moveSpeed;
        // Move the character controller based on the movement vector and speed
        characterController.Move(move * currentSpeed * Time.deltaTime);
    }
    public void Sprint()
    {
        if (!sprinting && moveInput != Vector2.zero)
        {   if(Time.time - taptime < doubletaptime)
            {
                StartCoroutine(StartSprint());
            }
            taptime = Time.time;
        }
    }
    public IEnumerator StartSprint()
    {
        sprinting = true;
        Debug.Log("Sprinting");
        yield return new WaitForSeconds(sprintTime);
        sprinting = false;
    }
    public void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based on sensitivity
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

   /* public void Shoot()
    {
        if (holdingGun == false || weapon == null)
        {
            Debug.LogError("No weapon equipped!");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, weapon.Range))
        {
            Debug.Log("Hit: " + hit.transform.name); // Add this line
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.enemydamage(weapon.Damage);
            }
        }
    }/*

    //if (holdingGun == true)
    // {

    // Vector3 direction;

    /*if (Input.mousePresent)
    {
        Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            direction = (hit.point - firePoint.position).normalized;
        }
        else
        {
            direction = ray.direction;
        }

    }
    else
    {
        direction = firePoint.forward;
    }
    // Instantiate the projectile at the fire point
    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

    // Get the Rigidbody component of the projectile and set its velocity
    Rigidbody rb = projectile.GetComponent<Rigidbody>();
    rb.velocity = direction * projectileSpeed;

    // Destroy the projectile after 3 seconds
    Destroy(projectile, 3f);*/
    // }
    //else
    /// {
    //     Debug.Log("you dont have a gun");
    // }

    public void PickUpObject()
{
    if (heldObject != null)
    {
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.transform.parent = null;
        holdingGun = false;
        weapon = null; 
    }

    Ray ray = new Ray(playerCamera.position, playerCamera.forward);
    RaycastHit hit;

    Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);

    if (Physics.Raycast(ray, out hit, pickUpRange))
    {
        // Check if you have picked up an object or the gun
        if (hit.collider.CompareTag("PickUp") || hit.collider.CompareTag("Gun"))
        {
            heldObject = hit.collider.gameObject;
            heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

            heldObject.transform.position = holdPosition.position;
            heldObject.transform.rotation = holdPosition.rotation;
            heldObject.transform.parent = holdPosition;

            if (hit.collider.CompareTag("Gun"))
            {
                weapon = heldObject.GetComponent<Weapon>();
                if (weapon != null)
                {
                    holdingGun = true;
                    Debug.Log("Weapon equipped!");
                }
                else
                {
                    Debug.LogError("No Weapon script found on the gun!");
                }
            }
        }
    }
}




}
