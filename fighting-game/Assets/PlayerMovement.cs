using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float doubleTapThreshold = 0.3f;

    [Header("Player 1 Input Configuration")]
    [Tooltip("Map these exact names in Edit -> Project Settings -> Input Manager")]
    public string p1HorizontalAxis = "P1_Horizontal";
    public string p1VerticalAxis = "P1_Vertical";

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // --- STATES ---
    [Header("Current States")]
    public bool isGrounded;
    public bool isCrouching;
    public bool isDashing;

    private Rigidbody2D rb;
    private float horizontalInput;
    private float verticalInput;
    private float previousHorizontalInput;

    // Dash Tracking
    private float lastTapTime;
    private float lastTapDirection;
    private float currentDashTime;

    // Anti-spam variabelen voor de Console Logs
    private bool wasCrouching;
    private bool wasMovingRight;
    private bool wasMovingLeft;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGrounded();

        if (isDashing)
        {
            HandleDashState();
            return;
        }

        HandleInput();
        HandleDashInput();
    }

    void FixedUpdate()
    {
        if (isDashing) return;
        ApplyMovement();
    }

    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }

    private void HandleInput()
    {
        try
        {
            // Read Player 1 Axes for movement
            horizontalInput = Input.GetAxisRaw(p1HorizontalAxis);
            verticalInput = Input.GetAxisRaw(p1VerticalAxis);
        }
        catch (UnityException)
        {
            Debug.LogWarning("WARNING: Input Axes niet ingesteld. Doe dit via Edit -> Project Settings -> Input Manager.");
            return;
        }

        // Crouch check (Left Joystick Down)
        isCrouching = verticalInput < -0.5f && isGrounded;

        // Jump check (Left Joystick Up)
        if (verticalInput > 0.5f && isGrounded && !isCrouching)
        {
            Jump();
        }

        LogMovementInputs();
    }

    private void LogMovementInputs()
    {
        // Console log: Loop check met anti-spam
        if (horizontalInput > 0.5f && !wasMovingRight) { Debug.Log("Running Right!"); wasMovingRight = true; wasMovingLeft = false; }
        else if (horizontalInput < -0.5f && !wasMovingLeft) { Debug.Log("Running Left!"); wasMovingLeft = true; wasMovingRight = false; }
        else if (Mathf.Abs(horizontalInput) < 0.1f) { wasMovingRight = false; wasMovingLeft = false; }

        // Console log: Crouch check met anti-spam
        if (isCrouching && !wasCrouching) { Debug.Log("Crouching!"); wasCrouching = true; }
        else if (!isCrouching && wasCrouching) { wasCrouching = false; }
    }

    private void ApplyMovement()
    {
        // Kan niet lopen tijdens het bukken
        if (isCrouching)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        Debug.Log("Jumped!");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    private void HandleDashInput()
    {
        // Double-tap detection for Dashing (Flicking Joystick)
        bool pressedRight = horizontalInput > 0.5f && previousHorizontalInput <= 0.5f;
        bool pressedLeft = horizontalInput < -0.5f && previousHorizontalInput >= -0.5f;

        if (pressedRight) CheckDoubleTap(1f);
        else if (pressedLeft) CheckDoubleTap(-1f);

        previousHorizontalInput = horizontalInput;
    }

    private void CheckDoubleTap(float direction)
    {
        if (Time.time - lastTapTime < doubleTapThreshold && direction == lastTapDirection && isGrounded)
        {
            StartDash(direction);
        }

        lastTapTime = Time.time;
        lastTapDirection = direction;
    }

    private void StartDash(float direction)
    {
        Debug.Log(direction > 0 ? "Dashed Right!" : "Dashed Left!");
        isCrouching = false;

        isDashing = true;
        currentDashTime = dashDuration;
        rb.linearVelocity = new Vector2(direction * dashSpeed, 0f);
    }

    private void HandleDashState()
    {
        currentDashTime -= Time.deltaTime;

        // Zorg dat je puur horizontaal dash't (geen zwaartekracht tijdens de dash)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        if (currentDashTime <= 0)
        {
            isDashing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
}