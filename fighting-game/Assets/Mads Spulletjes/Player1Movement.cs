using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player1Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float gravity = -30f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float doubleTapTime = 0.25f;

    [Header("Player Index (0 = P1, 1 = P2)")]
    public int playerIndex = 0;

    private CharacterController controller;
    private Gamepad myGamepad;
    private Vector2 moveInput;
    private Vector3 currentVelocity;

    private bool facingRight = false;
    private bool previousJumpInput;

    // Dash variabelen
    private bool isDashing;
    private float dashTimeLeft;
    private int dashDirection;
    private float lastLeftTapTime;
    private float lastRightTapTime;
    private int previousHorizontalInput;

    // Mortal Kombat sprong variabelen (Committed Jump)
    private bool wasGroundedLastFrame;
    private float lockedAirHorizontalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        myGamepad = ControllerManager.Instance.GetGamepad(playerIndex);
        if (myGamepad == null) return;

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
            }
            else
            {
                controller.Move(new Vector3(dashDirection * dashSpeed, 0f, 0f) * Time.deltaTime);
                return;
            }
        }

        moveInput = myGamepad.dpad.ReadValue();

        HandleDashInput();
        HandleMovement();

        // Alleen flippen als je op de grond staat (vaak standaard in arcade fighters)
        if (controller.isGrounded)
        {
            HandleFlip();
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    void HandleDashInput()
    {
        if (!controller.isGrounded) return; // Niet in de lucht dashen

        int currentHorizontal = 0;
        if (moveInput.x > 0.5f) currentHorizontal = 1;
        else if (moveInput.x < -0.5f) currentHorizontal = -1;

        if (currentHorizontal == -1 && previousHorizontalInput != -1)
        {
            if (Time.time - lastLeftTapTime < doubleTapTime) StartDash(-1);
            lastLeftTapTime = Time.time;
        }

        if (currentHorizontal == 1 && previousHorizontalInput != 1)
        {
            if (Time.time - lastRightTapTime < doubleTapTime) StartDash(1);
            lastRightTapTime = Time.time;
        }

        previousHorizontalInput = currentHorizontal;
    }

    void StartDash(int direction)
    {
        isDashing = true;
        dashDirection = direction;
        dashTimeLeft = dashDuration;

        if (direction == 1 && !facingRight) Flip();
        else if (direction == -1 && facingRight) Flip();
    }

    void HandleMovement()
    {
        bool isGrounded = controller.isGrounded;

        // Als we net landen, reset the Y velocity
        if (isGrounded && currentVelocity.y < 0)
        {
            currentVelocity.y = -2f;
        }

        float horizontalMove = 0f;
        bool currentJumpInput = moveInput.y > 0.5f;

        if (isGrounded)
        {
            // Op de grond: We reageren op de controller joystick/D-pad
            horizontalMove = moveInput.x * moveSpeed;

            // Bukken instellen
            if (moveInput.y < -0.5f)
            {
                Debug.Log("Player 1 Crouching");
                horizontalMove = 0f;
            }

            // Springen initialiseren (Committed jump)
            if (currentJumpInput && !previousJumpInput)
            {
                currentVelocity.y = jumpForce;

                // Sla de horizontale snelheid op precies op het moment dat je springt, zelfs als je diagonaal begint
                lockedAirHorizontalVelocity = moveInput.x * moveSpeed;
            }
        }
        else
        {
            // In de Lucht: Negeer input, vervolg de "gelockte" horizontale snelheid van de sprong
            horizontalMove = lockedAirHorizontalVelocity;
        }

        previousJumpInput = currentJumpInput;
        wasGroundedLastFrame = isGrounded;

        // Zwaartekracht berekenen en toepassen
        currentVelocity.y += gravity * Time.deltaTime;

        Vector3 moveVector = new Vector3(horizontalMove, currentVelocity.y, 0f);
        controller.Move(moveVector * Time.deltaTime);
    }

    void HandleFlip()
    {
        if (moveInput.x > 0 && !facingRight) Flip();
        else if (moveInput.x < 0 && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}