using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;

    [Header("Input Filters")]
    public float doubleTapThreshold = 0.3f;
    public float deadzone = 0.3f;

    [Header("Player 1 Input Configuration")]
    public string p1HorizontalAxis = "P1_Horizontal";
    public string p1VerticalAxis = "P1_Vertical";

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // STATES
    public bool isGrounded;
    public bool isDashing;

    private Rigidbody2D rb;
    private float horizontalInput;
    private float verticalInput;
    private float previousHorizontalInput;

    private bool isJumpAxisInUse;
    private float lastTapTime;
    private float lastTapDirection;
    private float currentDashTime;

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
            horizontalInput = Input.GetAxisRaw(p1HorizontalAxis);
            verticalInput = Input.GetAxisRaw(p1VerticalAxis);
        }
        catch (UnityException)
        {
            return;
        }

        // Deadzone check (forceert naar 0 bij loslaten)
        if (Mathf.Abs(horizontalInput) < deadzone) horizontalInput = 0f;
        if (Mathf.Abs(verticalInput) < deadzone) verticalInput = 0f;

        // Springen (stick omhoog = negatief in Unity)
        if (verticalInput < -0.5f)
        {
            if (isGrounded && !isJumpAxisInUse)
            {
                Jump();
                isJumpAxisInUse = true;
            }
        }
        else
        {
            // Lock eraf zodra de stick terugveert
            isJumpAxisInUse = false;
        }
    }

    private void ApplyMovement()
    {
        // De player beweegt nu altijd soepel op basis van de input
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    private void HandleDashInput()
    {
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
        isDashing = true;
        currentDashTime = dashDuration;
        rb.linearVelocity = new Vector2(direction * dashSpeed, 0f);
    }

    private void HandleDashState()
    {
        currentDashTime -= Time.deltaTime;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        if (currentDashTime <= 0)
        {
            isDashing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    // --- VISUELE DEBUGGER OP JE SCHERM ---
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.red;

        GUI.Label(new Rect(10, 10, 400, 30), "Vloer geraakt? " + isGrounded, style);
        GUI.Label(new Rect(10, 40, 400, 30), "Verticaal (Springen): " + verticalInput, style);
        GUI.Label(new Rect(10, 70, 400, 30), "Horizontaal (Lopen): " + horizontalInput, style);
    }
}