using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int coins;

    // --- Movement & Animation ---
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public float moveSpeed = 4f;

    // Speed boost
    private float originalSpeed;
    private bool isSpeedBoosted = false;

    // --- Jump variables ---
    public float jumpForce = 8f;
    public int extraJumpsValue = 1;
    private int extraJumps;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalSpeed = moveSpeed;

        // Movement feel polish
        rb.freezeRotation = true;
        rb.gravityScale = 3f;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        //############################ for  Smooth horizontal movement
        float targetSpeed = moveInput * moveSpeed;
        rb.linearVelocity = new Vector2(
            Mathf.Lerp(rb.linearVelocity.x, targetSpeed, 0.15f),
            rb.linearVelocity.y
        );

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        // Jump & double jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                SoundManager.Instance.PlaySFX("JUMP");
            }
            else if (extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
                SoundManager.Instance.PlaySFX("JUMP");
            }
        }

        //  ++++++++++++++++Faster fall for better jump feel
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;
        }

        // ______________________ Flip sprite when moving
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }

        SetAnimation(moveInput);
    }

    // Speed power-up 
    public void ActivateSpeedBoost(float boostMultiplier, float duration)
    {
        if (!isSpeedBoosted)
        {
            StartCoroutine(SpeedBoostCoroutine(boostMultiplier, duration));
        }
    }

    private System.Collections.IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        isSpeedBoosted = true;
        moveSpeed = originalSpeed * multiplier;

        // Optional visual feedback
        spriteRenderer.color = Color.yellow;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;
        spriteRenderer.color = Color.white;
        isSpeedBoosted = false;
    }

    private void SetAnimation(float moveInput)
    {
        if (isGrounded)
        {
            if (moveInput == 0)
                animator.Play("Player_Idle");
            else
                animator.Play("Player_Run");
        }
        else
        {
            if (rb.linearVelocityY > 0)
                animator.Play("Player_Jump");
            else
                animator.Play("Player_Fall");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BouncePad"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 2f);
        }
    }
}
