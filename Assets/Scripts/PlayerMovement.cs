using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public Animator FeatherAnimator;
    private TrailRenderer trailRenderer;
    private AudioManager audioManager;
    [SerializeField] private UnlockEffect unlockEffect;
    [SerializeField] private TextMeshProUGUI counterText;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public Vector2 SpawnPoint = Vector2.zero;
    public float CameraSize = 10f;
    private float horizontalMovement;
    private bool isFacingRight = true;
    private bool isDying;

    [Header("Jumping & Gravity")]
    public float jumpPower = 10f;
    public int maxJumps = 1;
    public float baseGravity = 2f;
    public float fallSpeedMultiplier = 2f;
    public float maxFallSpeed = 18f;
    public float coyoteTimeSeconds = 0.1f;
    private float coyoteTimeCounter;
    private int jumpsRemaining;

    [Header("Dashing")]
    public bool hasDash;
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    private bool isDashing;
    private bool canDash = true;

    [Header("Wall & Climb")]
    public bool hasClimb;
    public float wallSlideSpeed = 2f;
    public LayerMask wallLayer;
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.5f);
    private bool isWallSliding;

    [Header("Collision Checks")]
    public LayerMask groundLayer;
    public LayerMask cloudLayer;
    public LayerMask spikeLayer;
    public LayerMask thornLayer;
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public Transform groundCheck2Pos;
    public Vector2 groundCheck2Size = new Vector2(0.5f, 0.5f);
    public Transform spikeCheckPos;
    public Vector2 spikeCheckSize = new Vector2(0.5f, 0.5f);

    public bool isGrounded;
    public SaveManager featherNum;
    public bool hasHit = false;
    private bool isAboveSpike;
    public float displace = 0.1f;
    public int num = 10; // Steps for death return sequence

    private int score = 0;

    public void Awake()
    {
        // Finding the AudioManager by tag
        GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj != null) audioManager = audioObj.GetComponent<AudioManager>();
    }

    public void Start()
    {
        // Check if we have a valid save
        if (SaveManager.instance != null && SaveManager.instance.currentSave.lastSavePos != Vector3.zero)
        {
            SpawnPoint = SaveManager.instance.currentSave.lastSavePos;

            // Force the Rigidbody to move immediately
            rb.position = SpawnPoint;
            transform.position = SpawnPoint;
        }
        else
        {
            SpawnPoint = transform.position;
        }

        // Rest of your start code...
        if (Camera.main != null) Camera.main.orthographicSize = CameraSize;
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        UpdateAnimations();

        // Stop all logic if dashing or dying
        if (isDashing || isDying) return;

        HandleGroundAndSpikeChecks();
        ApplyGravityLogic();
        HandleWallSliding();

        // Horizontal Movement logic
        if (!isWallSliding)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
            Flip();
        }

        counterText.text = "Score: " + score.ToString();
    }

    private void UpdateAnimations()
    {
        animator.SetFloat("yvelocity", rb.velocity.y);
        animator.SetFloat("xVelocity", rb.velocity.x);
        animator.SetBool("isWallsliding", isWallSliding);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (isDashing || isDying) return;
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!wallCheck() && !isGrounded)
        {
            jumpsRemaining = 0;
        }

        // Keeping your exact original Jump logic
        if (context.performed && jumpsRemaining > 0 && !wallCheck())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            animator.SetBool("isJumping", true);
            audioManager?.PlaySFX(audioManager.jump);
        }

        if (context.performed && jumpsRemaining > 0 && wallCheck())
        {
            WallJump();
        }
        else if (context.canceled && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && hasDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public void Hit(InputAction.CallbackContext context)
    {
        if (context.performed && hasHit && !isGrounded)
        {
            animator.SetTrigger("Dhit");
            if (isAboveSpike)
            {
                audioManager?.PlaySFX(audioManager.spikeJump);
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }
    }

    // --- PHYSICS & CUSTOM LOGIC ---

    private void ApplyGravityLogic()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    // Your untouched Wall Jump code
    public void WallJump()
    {
        if(hasClimb)
        {
            //horizontalMovement = horizontalMovement * -1;
            if (isFacingRight)
            {
                rb.velocity = new Vector2(-jumpPower, jumpPower);
            }
            if (!isFacingRight)
            {
                rb.velocity = new Vector2(jumpPower, jumpPower);
            }
            animator.SetBool("isJumping", true);
            Debug.Log("WJump triggered");
            audioManager?.PlaySFX(audioManager.jump);
        }
    }

    private void HandleWallSliding()
    {
        if (!isGrounded && wallCheck() && horizontalMovement != 0 && hasClimb)
        {
            isWallSliding = true;
            jumpsRemaining = maxJumps;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void HandleGroundAndSpikeChecks()
    {
        // Ground Detection
        bool hitGround = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);
        bool hitCloud = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, cloudLayer);

        bool wasGrounded = isGrounded;
        isGrounded = hitGround || hitCloud;

        if (isGrounded)
        {
            if (!wasGrounded) audioManager?.PlaySFX(audioManager.land);
            jumpsRemaining = maxJumps;
            animator.SetBool("isJumping", false);
            coyoteTimeCounter = coyoteTimeSeconds;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Handle the "Ground 2" anti-stuck displacement
        if (Physics2D.OverlapBox(groundCheck2Pos.position, groundCheck2Size, 0, groundLayer))
        {
            rb.transform.position += new Vector3(0, displace, 0);
        }

        // Spike Detection
        isAboveSpike = Physics2D.OverlapBox(spikeCheckPos.position, spikeCheckSize, 0, spikeLayer);

        bool hitSpikeDirectly = Physics2D.OverlapBox(groundCheck2Pos.position, groundCheck2Size, 0, spikeLayer) ||
                               (Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, spikeLayer) && !wallCheck()) ||
                               Physics2D.OverlapBox(groundCheck2Pos.position, groundCheck2Size, 0, thornLayer);

        if (hitSpikeDirectly && !isDying)
        {
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        isDying = true;
        Vector2 startPos = rb.position;
        float originalGravity = rb.gravityScale;

        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("died");

        // Smoothly lerp back to spawn point
        for (int i = 0; i <= num; i++)
        {
            rb.position = Vector2.Lerp(startPos, SpawnPoint, (float)i / num);
            yield return new WaitForSeconds(0.03f);
        }

        rb.position = SpawnPoint;
        rb.gravityScale = originalGravity;
        isDying = false;
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        trailRenderer.emitting = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float dashDirection = isFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        isDashing = false;
        trailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private bool wallCheck() => Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);

    private void Flip()
    {
        if ((isFacingRight && horizontalMovement < 0) || (!isFacingRight && horizontalMovement > 0))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Feather()
    {
        if (SaveManager.instance.currentSave.collectedFeatherIDs.Count == 5)
        {
            animator.SetBool("HasHat", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collider")) SpawnPoint = other.transform.position;

        if (other.CompareTag("DashPower")) { hasDash = true; Destroy(other.gameObject); }

        if (other.CompareTag("ClimbPower"))
        {
            hasClimb = true;
            unlockEffect?.PlayClimbAnimation();
            Destroy(other.gameObject);
        }

        if(other.CompareTag("End"))
        {
            FindObjectOfType<CreditsController>().StartCredits();
        }

        if (other.CompareTag("HitPower"))
        {
            hasHit = true;
            animator.SetBool("HasHit", true);
            unlockEffect?.PlayDeathAnimation();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Feather")) 
        {
            other.GetComponent<Collider2D>().enabled = false;
            FeatherAnimator.SetTrigger("Collect");
            Feather();
            score += 1000; Destroy(other.gameObject, 2.3f);
            
        }

        if (other.CompareTag("Bench"))
        {
            animator.SetTrigger("Bench");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white; Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.magenta; Gizmos.DrawWireCube(groundCheck2Pos.position, groundCheck2Size);
        Gizmos.color = Color.blue; Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
        Gizmos.color = Color.red; Gizmos.DrawWireCube(spikeCheckPos.position, spikeCheckSize);
    }
}