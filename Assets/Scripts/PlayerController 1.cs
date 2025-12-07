using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int coins;

    // --- Movement & Animation ---
    private Animator animator;             // Reference to Animator for controlling animations
    public float moveSpeed = 4f;           // How fast the player moves left/right

    // --- Jump variables ---
    public float jumpForce = 8f;           // Base jump force (vertical speed)
    public int extraJumpsValue = 1;        // How many extra jumps allowed (1 = double jump, 2 = triple jump)
    private int extraJumps;                // Counter for jumps left
    
    public Transform groundCheck;          // Empty child object placed at the player's feet
    public float groundCheckRadius = 0.2f; // Size of the circle used to detect ground
    public LayerMask groundLayer;          // Which layer counts as "ground" (set in Inspector)

    // --- Internal state ---
    private Rigidbody2D rb;                // Reference to the Rigidbody2D component
    private bool isGrounded;               // True if player is standing on ground

    void Start()
    {
        // Grab references once at the start
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // --- Horizontal movement ---
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // --- Ground check ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        // --- Jump & Double Jump ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                // Normal jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                SoundManager.Instance.PlaySFX("JUMP");          // 🔊 NEW
            }
            else if (extraJumps > 0)
            {
                // Extra jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
                SoundManager.Instance.PlaySFX("JUMP");          // 🔊 NEW
            }
        }

        // --- Animations ---
        SetAnimation(moveInput);
    }

    private void SetAnimation(float moveInput)
    {
        if (isGrounded)
        {
            if (moveInput == 0)
            {
                animator.Play("Player_Idle");
            }
            else
            {
                animator.Play("Player_Run");
            }
        }
        else
        {
            if (rb.linearVelocityY > 0)
            {
                animator.Play("Player_Jump");
            }
            else
            {
                animator.Play("Player_Fall");
            }
        }
    }
}
