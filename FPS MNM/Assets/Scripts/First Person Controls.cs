using System.Collections;
using UnityEngine;

public class FirstPersonControls : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    public float moveSpeed;
    public float lookSpeed;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    public Transform playerCamera;

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
    public float pickUpRange = 5f;
    public bool holdingGun = false;
    private Weapon weapon;

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition;
    private GameObject heldObject;

    [Header("TELEPORT SETTINGS")]
    [Space(5)]
    public Transform TeleportLocation;
    public bool canTeleport = false;

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
        playerInput.Player.Teleport.performed += ctx => Teleport();
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

        float currentSpeed = sprinting ? moveSpeed * movementspeedmultiplier : moveSpeed;
        characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    public void Sprint()
    {
        if (!sprinting && moveInput != Vector2.zero)
        {
            if (Time.time - taptime < doubletaptime)
            {
                StartCoroutine(StartSprint());
            }
            taptime = Time.time;
        }
    }

    public void Teleport()
    {
        if (canTeleport)
        {
            StartCoroutine(TeleportPlayer()); // Use Coroutine for teleportation
        }
    }



    private IEnumerator TeleportPlayer()
    {
        characterController.enabled = false; // Disable CharacterController before teleporting
        transform.position = TeleportLocation.position;
        yield return null; // Wait for one frame
        characterController.enabled = true; // Re-enable CharacterController after teleporting
        Debug.Log("Player teleported to " + TeleportLocation.position);
        ResetTeleport();
    }

    public void ResetTeleport()
    {
        canTeleport = false;
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
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

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
        }
    }

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
            if (hit.collider.CompareTag("PickUp") || hit.collider.CompareTag("Gun"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;

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
