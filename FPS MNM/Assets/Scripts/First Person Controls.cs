using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FirstPersonControls : MonoBehaviour
{
    public Animator mc;

    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    public float moveSpeed;
    public float lookSpeed;
    public float multiply;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    public Transform playerCamera;

    [Header("LOOK SETTINGS")]
    public float mouseLookSpeed = 0.15f; // Mouse look speed
    public float gamepadLookSpeed = 1.0f; // Gamepad look speed
    private bool isUsingGamepad = false; // Track if the gamepad is being used

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalLookRotation = 0f;
    private Vector3 velocity;
    private CharacterController characterController;

    [Header("SPRINTING SETTINGS")]
    [Space(5)]
    public float doubletaptime = 0.5f;
    public float movementspeedmultiplier = 2f;
    public float sprintTime = 4f;

    private float taptime;
    private bool sprinting = false;

    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;
    public float pickUpRange = 40f;
    public bool holdingGun = false;
    private Weapon weapon;

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition;
    public Transform holdPositionPickUp; // Add a new hold position for generic pickup items
    private GameObject heldObject;

    [Header("SECOND WEAPON HOLD POSITION")]
    public Transform holdPositionSecondWeapon;

    [Header("TELEPORT SETTINGS")]
    [Space(5)]
    public string sceneToLoad;  
    public bool canTeleport = false;

    [Header("WEAPON SETTINGS")]
    public List<Weapon> weapons = new List<Weapon>();
    private int currentWeaponIndex = 0;

    [SerializeField]
    public Weapon Weapon;
    public PlayerAttributes playerAttributes;


    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with an object tagged "IF"
        if (other.CompareTag("IF"))
        {
            EndGame(); // Call the method to end the game
        }
    }

    // Method to end the game
    public void EndGame()
    {
        Debug.Log("Game Over: Player collided with 'IF' tagged object.");
        Application.Quit();
    }

    private void Awake()
    {
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

        playerInput.Player.Look.performed += ctx => {
            lookInput = ctx.ReadValue<Vector2>();
            isUsingGamepad = ctx.control.device is Gamepad;
        };
        playerInput.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        playerInput.Player.Jump.performed += ctx => Jump();
        playerInput.Player.Shoot.performed += ctx => {
            if (weapon != null) weapon.Shoot();
            else Debug.LogError("No weapon equipped!");
        };
        playerInput.Player.PickUp.performed += ctx => PickUpObject();
        playerInput.Player.Teleport.performed += ctx => Teleport();
        playerInput.Player.SwitchWeapon.performed += ctx => SwitchWeapon();
    }

    private void Update()
    {
        Move();
        LookAround();
        ApplyGravity();
    }

    public void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);

        // Determine the current speed
        float currentSpeed = sprinting ? moveSpeed * movementspeedmultiplier : moveSpeed;

        // Apply movement
        characterController.Move(move * currentSpeed * Time.deltaTime);

        // Update Animator Bools
        bool isMoving = moveInput != Vector2.zero;
        mc.SetBool("Walk", isMoving && !sprinting); // Walk when moving and not sprinting
        mc.SetBool("Run", isMoving && sprinting);   // Run when moving and sprinting
    }




    public IEnumerator StartSprint()
    {
        sprinting = true;
        mc.SetBool("Run", true);
        playerAttributes.StartSprinting(); // Notify PlayerAttributes
        yield return new WaitForSeconds(sprintTime);
        sprinting = false;
        mc.SetBool("Run", false);
        playerAttributes.StopSprinting(); // Notify PlayerAttributes
    }




    private bool isFirstTap = false; // Tracks if the first tap has occurred

    public void Sprint()
    {
        // If player is moving
        if (moveInput != Vector2.zero)
        {
            if (!isFirstTap) // First tap detected
            {
                isFirstTap = true;
                taptime = Time.time; // Record the time of the first tap
            }
            else // Check for double-tap
            {
                if (Time.time - taptime <= doubletaptime) // Second tap within allowed time
                {
                    sprinting = true; // Start sprinting
                    playerAttributes.StartSprinting(); // Notify PlayerAttributes
                    Debug.Log("Sprinting started!");
                }
                isFirstTap = false; // Reset for future double-tap detection
            }
        }
        else
        {
            // Reset when player stops moving
            isFirstTap = false;
            sprinting = false;
            playerAttributes.StopSprinting(); // Notify PlayerAttributes
            Debug.Log("Sprinting stopped!");
        }
    }



    public void Teleport()
    {
        if (canTeleport)
        {
            LoadNewScene();  // Call the new method to load a scene
        }
    }

    private void LoadNewScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
            Debug.Log("Loading scene: " + sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene name not set. Please specify a scene to load in the Inspector.");
        }
    }

    public void ResetTeleport()
    {
        canTeleport = false;
    }

   /* public IEnumerator StartSprint()
    {
        sprinting = true;
        Debug.Log("Sprinting");
        yield return new WaitForSeconds(sprintTime);
        sprinting = false;
    }*/

    public void LookAround()
    {
        float LookX = lookInput.x * (isUsingGamepad ? gamepadLookSpeed : mouseLookSpeed);
        float LookY = lookInput.y * (isUsingGamepad ? gamepadLookSpeed : mouseLookSpeed);

        transform.Rotate(0, LookX, 0);

        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            mc.SetTrigger("Jump");
        }
    }

    public void PickUpObject()
    {
        if (heldObject != null)
        {
            DropCurrentWeapon();
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("PickUp"))
            {
                GameObject objectToPickUp = hit.collider.gameObject;
                Rigidbody objectRb = objectToPickUp.GetComponent<Rigidbody>();

                // Set the Rigidbody to kinematic if it has one
                if (objectRb != null)
                {
                    objectRb.isKinematic = true;
                }

                // Place the picked-up object at the designated hold position for generic items
                objectToPickUp.transform.position = holdPositionPickUp.position;
                objectToPickUp.transform.rotation = holdPositionPickUp.rotation;
                objectToPickUp.transform.parent = holdPositionPickUp;

                heldObject = objectToPickUp; // Set heldObject to the picked-up object

                Debug.Log("Picked up a generic object and placed it at the hold position.");
            }
            else if (hit.collider.CompareTag("Gun"))
            {
                GameObject objectToPickUp = hit.collider.gameObject;
                Rigidbody objectRb = objectToPickUp.GetComponent<Rigidbody>();

                Weapon newWeapon = objectToPickUp.GetComponent<Weapon>();
                if (newWeapon != null && weapons.Count < 2 && !weapons.Contains(newWeapon))
                {
                    weapons.Add(newWeapon);

                    EquipWeapon(weapons.Count - 1);

                    if (objectRb != null)
                    {
                        objectRb.isKinematic = true;
                    }

                    Transform holdPositionToUse = weapons.Count == 1 ? holdPosition : holdPositionSecondWeapon;
                    objectToPickUp.transform.position = holdPositionToUse.position;
                    objectToPickUp.transform.rotation = holdPositionToUse.rotation;
                    objectToPickUp.transform.parent = holdPositionToUse;

                    Debug.Log("Weapon picked up and equipped.");
                }
            }
        }
    }

    public void EquipWeapon(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].gameObject.SetActive(i == index);
            }

            currentWeaponIndex = index;
            weapon = weapons[currentWeaponIndex];

            if (weapon != null)
            {
                Rigidbody weaponRb = weapon.GetComponent<Rigidbody>();
                if (weaponRb != null)
                {
                    weaponRb.isKinematic = true;
                    
                }
                
                Debug.Log("Equipped weapon: " + weapon.gameObject.name);
            }
        }
    }

    public void DropCurrentWeapon()
    {
        if (weapon != null)
        {
            Rigidbody weaponRb = weapon.GetComponent<Rigidbody>();
            if (weaponRb != null)
            {
                weaponRb.isKinematic = false;
            }

            weapon.transform.parent = null;
            weapon.gameObject.SetActive(false);
            weapons.Remove(weapon);
            weapon = null;

            Debug.Log("Weapon dropped and removed from the list.");
        }
    }

    public void SwitchWeapon()
    {
        if (weapons.Count > 0)
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            EquipWeapon(currentWeaponIndex);
        }
    }
}
