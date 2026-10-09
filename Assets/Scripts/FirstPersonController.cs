using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [Header("Keyboard")]
    public float moveSpeed = 5.0f;
    public float jumpHeight = 0.5f;
    public float gravity = -50.0f;

    [Header("Jump Settings")]
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    [Header("Mouse")]
    public float sensX = 100.0f;
    public float sensY = 100.0f;
    public Transform playerRotation;
    public Transform cameraTransform;

    [Header("Input Action")]
    private CharacterController controller;
    private InputAction lookAction;
    private InputAction moveAction;
    private InputAction jumpAction;

    private float xRotation;
    private float verticalVelocity;

    private float coyoteTimer;
    private float jumpBufferTimer;

    [Header("Mug Dependencies")]
    public CafeManager cafeManager;
    public Transform onTheTray;

    public NPCTrash npcTrash;
    public RubbishTrash currentRubbishTrash;

    public GameObject CubeTester;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        lookAction = new InputAction(type: InputActionType.Value, binding: "<Mouse>/delta");

        moveAction = new InputAction(type: InputActionType.Value, binding: "<Keyboard>/w");
        moveAction.AddCompositeBinding("2DVector")
        .With("Up", "<Keyboard>/w")
        .With("Down", "<Keyboard>/s")
        .With("Left", "<Keyboard>/a")
        .With("Right", "<Keyboard>/d");

        jumpAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/space");
    }

    void OnEnable()
    {
        lookAction.Enable();
        moveAction.Enable();
        jumpAction.Enable();
    }

    void OnDisable()
    {
        lookAction.Disable();
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentRubbishTrash.isInAreaRubbishBin)
            {
                currentRubbishTrash.HandleWasteTrash();
            }
            else
            {
                npcTrash.HandlePlayerPickingUpCup();
            }

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rigidbody rb = CubeTester.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cafeManager.thereIsMugOnTray && cafeManager.playerInsideCafe)
            {
                cafeManager.PutBackMug();
            }
            else
            {
                cafeManager.GenerateMug();
            }
        }

        Look();
        Move();
    }

    void Move()
    {
        if (controller.isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (jumpAction.WasPressedThisFrame())
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // gravity
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2.0f;
        }

        // jump
        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            verticalVelocity = Mathf.Sqrt(
                jumpHeight * -2.0f * gravity
            );

            jumpBufferTimer = 0;
            coyoteTimer = 0;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 moveCharacter = transform.right * input.x + transform.forward * input.y;

        Vector3 velocity = moveCharacter * moveSpeed;

        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    void Look()
    {
        Vector2 mouseInput = lookAction.ReadValue<Vector2>();

        float mouseX = mouseInput.x * sensX * Time.deltaTime;
        float mouseY = mouseInput.y * sensY * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        playerRotation.Rotate(Vector3.up * mouseX);
    }
}