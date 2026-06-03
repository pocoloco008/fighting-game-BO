using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player1Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Player Index (0 = P1, 1 = P2)")]
    public int playerIndex = 0; // Staat op 0 voor Player 1

    private Rigidbody2D rb;
    private Gamepad myGamepad;
    private Vector2 moveInput;

    private bool facingRight = false;
    private bool isGrounded;
    private bool previousJumpInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Haal *deze specifieke* controller op uit de manager
        myGamepad = ControllerManager.Instance.GetGamepad(playerIndex);

        if (myGamepad == null)
        {
            // Controller is nog niet toegewezen
            return;
        }

        // Lees D-Pad input van de specifieke actuele controller
        moveInput = myGamepad.dpad.ReadValue();

        HandleMovement();
        HandleFlip();
    }

    void HandleMovement()
    {
        // Links / rechts bewegen
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        bool currentJumpInput = moveInput.y > 0.5f;

        // Omhoog = springen (Alleen springen als je zojuist omhoog drukte en op de grond staat)
        if (currentJumpInput && !previousJumpInput && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        previousJumpInput = currentJumpInput;

        // Omlaag = bukken
        if (moveInput.y < -0.5f)
        {
            Debug.Log("Player 1 Crouching");
        }
    }

    void HandleFlip()
    {
        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}