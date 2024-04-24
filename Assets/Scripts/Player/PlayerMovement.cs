using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float speed = 8f;
    public bool canCrouch = true;
    public float crouchSpeed = 4f;
    [SerializeField] private Transform roofCheck;
    public Collider2D crouchDisableCollider;
    private bool isCrouching = false;

    [Header("Components")]
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private TrailRenderer dashTrail;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jumps")]
    public float jumpingPower = 20f;
    private float jumpBufferTime = 0.2f;
    public float coyoteTime = 1f;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;
    public bool canDoubleJump = true;
    public int maxDoubleJumps = 1; // Adjust this as needed
    [HideInInspector] public int doubleJumpCount; // Counter for double jumps

    [Header("Wall Maneuver")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    public float wallSlideSpeed = 2f;
    public float wallJumpDuration = 0.2f;
    public Vector2 wallJumpingPower = new Vector2(10f, 24f);
    private bool isWallSliding;
    public bool canWallJump = true;
    private bool isWallJumping;
    private float wallJumpDir;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;

    [Header("Dashing")]
    public bool canDash = true;
    [HideInInspector] public bool canDashCode;
    public float dashingPower = 24f;
    [HideInInspector] public bool isDashing = false;
    public float dashingTime = 0.2f;
    private float dashingCooldown = 0.25f;
    [HideInInspector]public bool wasDashing = false;  //? These will hold the dash values in last 2 frames
    //===================================//
    public bool canUpDash = true;
    [HideInInspector] public bool canUpDashCode;
    public float upDashingPower = 15f;
    public int upDashCount; // Counter for up dashes
    [HideInInspector] public bool isUpDashing = false;
    public float upDashingTime = 0.5f;
    private float upDashingCooldown = 0.5f;
	[HideInInspector] public int originalUpDashCount;
    [HideInInspector] public bool wasUpDashing = false;  //? These will hold the dash values in last 2 frames
    private float timeBtwSpawns;
    public float startTimeBtwSpawns;
    public GameObject echo;

    [Header("Gliding")]
    public bool canGlide = true;
    private bool canGlideCode = true;
    public float linearDrag = 0f;
    [HideInInspector] public bool isGliding = false;
    public float glideDrag = 10f;

    [Header("Grappling Gun")]
    public bool canGrapple = true;
    public GameObject grapplingChildObj;

    private bool isFacingRight = true;
    private float horizontal;
    private float originalSpeed;
    private bool isGroundedVar = true;

    void Start()
    {
        originalSpeed = speed;
		originalUpDashCount = upDashCount;
    }

    public void Update()
    {
        if(isDashing || isUpDashing){
            dashTrail.enabled = false;
            if(timeBtwSpawns <= 0){
                //spawn echo
                GameObject instance = (GameObject)Instantiate(echo, transform.position, Quaternion.identity);
                Destroy(instance, 1.5f);
                timeBtwSpawns = startTimeBtwSpawns;
            }
            else{
                timeBtwSpawns -= Time.deltaTime;
            }
        }
        
        else if(!wasDashing && !wasUpDashing) dashTrail.enabled = true;

        if (isDashing == true) return;

        if(!canGrapple) grapplingChildObj.SetActive(false);
        if(canGrapple)  grapplingChildObj.SetActive(true) ;

        if (IsGrounded())
        {
            if (!isDashing) canDashCode = canDash;

            if (!isUpDashing) canUpDashCode = canUpDash;

            if (!isGliding) canGlideCode = canGlide;

            isGliding = false;

			upDashCount = originalUpDashCount;
        }
        HandleJumping();
        WallSlide();
        UpdateDashVariables();
        if (canWallJump)
        {
            WallJump();
        }

        if (!isWallJumping)
        {
            Flip();
            HandleHorizontalMovement();
            Crouch();
        }

        if ((Input.GetKeyDown(KeyCode.LeftControl) || (Input.GetKeyDown(KeyCode.RightControl))) && canDashCode)
        {
            StartCoroutine(Dash());
        }

        if ((Input.GetKey(KeyCode.E)||(Input.GetKey(KeyCode.Return))) && canUpDashCode && !isGliding && upDashCount > 0)
        {
            StartCoroutine(UpDash());
        }

        if (IsOnWall())
        { //Doesn't allow gliding on wall
            isGliding = false;
            playerRB.drag = linearDrag;
        }

        if (!canDoubleJump)
        {
            doubleJumpCount = 0;
        }
        if (!isWallJumping && !isWallSliding && !isCrouching)
        {
            Glide();
        }
    }

    private void HandleHorizontalMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        playerRB.velocity = new Vector2(horizontal * speed, playerRB.velocity.y);
    }

    private void HandleJumping()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            doubleJumpCount = maxDoubleJumps; // Reset double jump count when grounded
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if ((coyoteTimeCounter > 0f) && (jumpBufferCounter > 0f) && (!isJumping))
        {
            Jump();
            StartCoroutine(JumpCooldown());
            if (!IsGrounded()) // Increment double jump count if coyote jumping
            {
                doubleJumpCount++;
            }
        }
        else if ((Input.GetButtonDown("Jump")|| Input.GetKeyDown(KeyCode.UpArrow)) && doubleJumpCount > 0)
        {
            Jump();
            StartCoroutine(JumpCooldown());
            doubleJumpCount--;
        }

        if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.UpArrow)) && playerRB.velocity.y > 0f)
        {
            SoftLand();
            coyoteTimeCounter = 0f;
        }
    }

    public void Jump()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpingPower);
        jumpBufferCounter = 0f;
    }

    private void SoftLand()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * 0.5f);
    }

    public bool IsTouchingRoof()
    {
        return Physics2D.OverlapCircle(roofCheck.position, 0.5f, groundLayer);
    }
    private void Crouch()
    {
        if (Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow))
        {
            if (canCrouch && IsGrounded())
            {
                isCrouching = true;
                speed = crouchSpeed;
                crouchDisableCollider.enabled = false;
                canDash = false;
                canUpDash = false;
                canDoubleJump = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.S)||Input.GetKeyUp(KeyCode.DownArrow) && !IsTouchingRoof())
        {
            isCrouching = false;
            speed = originalSpeed;
            crouchDisableCollider.enabled = true;
            canDash = true;
            canUpDash = true;
            canDoubleJump = true;
        }
    }

    private void WallSlide()
    {
        if (IsOnWall() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            float wallYvel = Mathf.Clamp(playerRB.velocity.y, -wallSlideSpeed, float.MaxValue);
            playerRB.velocity = new Vector2(playerRB.velocity.x, wallYvel);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding == true)
        {
            isWallJumping = false;
            wallJumpDir = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime; //You can jump even after turning away from the wall
        }

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && wallJumpCounter > 0f)
        {
            isWallJumping = true;
            playerRB.velocity = new Vector2(wallJumpDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpCounter = 0f;
            //canDash = true;
            //canDashCode = true;
            doubleJumpCount = maxDoubleJumps;

            if (transform.localScale.x != wallJumpDir)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
            grapplingChildObj.transform.localScale = localScale;
        }
    }

    public bool IsGrounded()
    {
        isGroundedVar = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        return isGroundedVar;
    }

    private bool IsOnWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.4f, wallLayer);
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    private IEnumerator Dash()
    {
        canDashCode = false;
        isDashing = true;
        float originalTRWidth = dashTrail.startWidth;
        float originalGravity = playerRB.gravityScale;
        playerRB.gravityScale = 0f;
        playerRB.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        dashTrail.startWidth = 0.78f;
        yield return new WaitForSeconds(dashingTime);
        dashTrail.startWidth = originalTRWidth;
        playerRB.gravityScale = originalGravity;
        isDashing = false;
        if (IsGrounded() && !isDashing)
        {
            yield return new WaitForSeconds(dashingCooldown);
            canDashCode = true;
        }
    }

    private IEnumerator UpDash()
    {
        if (!isGliding)
        {
            canUpDashCode = false;
            isUpDashing = true;
            float originalTRWidth = dashTrail.startWidth;
            float originalGravity = playerRB.gravityScale;
            playerRB.gravityScale = 0f;
            playerRB.velocity = new Vector2(0f, transform.localScale.y * upDashingPower);
            dashTrail.startWidth = 0.78f;
            yield return new WaitForSeconds(upDashingTime);
            dashTrail.startWidth = originalTRWidth;
            playerRB.gravityScale = originalGravity;
            isUpDashing = false;
            upDashCount--; // Decrease upDashCount after using an up dash
            if (!isUpDashing) //? If doesnt work add [ && IsGrounded() ]
            {
                yield return new WaitForSeconds(upDashingCooldown);
                canUpDashCode = true;
            }
        }
    }

    private void UpdateDashVariables()
    {
        // Track whether the player was dashing or up-dashing in the last two frames
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(UpdateWasDashing());
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Mouse0))
        {
            StartCoroutine(UpdateWasUpDashing());
        }
    }

    private IEnumerator UpdateWasDashing()
    {
        wasDashing = true;
        yield return new WaitForSeconds(0.5f);
        wasDashing = false;
    }

    private IEnumerator UpdateWasUpDashing()
    {
        wasUpDashing = true;
        yield return new WaitForSeconds(0.5f);
        wasUpDashing = false;
    }

    private void Glide()
    {
        if (!IsGrounded() && (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)) && canGlideCode && !isGliding && !isUpDashing)
        {
            isGliding = true;
            playerRB.drag = glideDrag;
        }
        if (IsGrounded() || Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            isGliding = false;
            playerRB.drag = linearDrag;
        }
    }
}
