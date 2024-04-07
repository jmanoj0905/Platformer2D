using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Horizontal_Movement")]
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
	public float jumpBufferTime = 0.2f;
	public float coyoteTime = 1f;
	public float coyoteTimeCounter;
	private float jumpBufferCounter;
	private bool isJumping;
	public bool canDoubleJump = true;
	public int maxDoubleJumps = 1; // Adjust this as needed
	[HideInInspector]public int doubleJumpCount; // Counter for double jumps


	[Header("Wall_Maneuver")]
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
	[HideInInspector]public bool canDashCode;
	public float dashingPower = 24f;
	private bool isDashing = false;
	public float dashingTime = 0.2f;
	private float dashingCooldown = 0.25f;
	//===================================//
	public bool canUpDash = true;
	[HideInInspector]public bool canUpDashCode;
	public float upDashingPower = 15f;
	private bool isUpDashing = false;
	public float upDashingTime = 0.5f;
	private float upDashingCooldown = 1f;

	[Header("Gliding")]
	public bool canGlide = true;
	private bool canGlideCode = true;
	public float linearDrag = 0f;
	[HideInInspector] public bool isGliding = false;
	public float glideDrag = 10f;

	private bool isFacingRight = true;
	private float horizontal;
	private float originalSpeed;
	private bool isGroundedVar = true;

	void Start()
	{
		originalSpeed = speed;
	}

	public void Update()
	{
		if (isDashing == true)
		{
			return; // Prevents player from moving while dashing
		}

		if (IsGrounded())
		{
			if(!isDashing)	canDashCode = canDash;

			if(!isUpDashing) canUpDashCode = canUpDash;

			if(!isGliding) canGlideCode = canGlide;

			isGliding = false;
		}
		HandleJumping();
		WallSlide();
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
		
		if (Input.GetKeyDown(KeyCode.LeftControl) && canDashCode)
		{
			StartCoroutine(Dash());
		}
		
		if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Mouse0) && canUpDashCode && !isGliding)
		{
			StartCoroutine(UpDash());
		}
		if(IsOnWall()){ //Does't allow gliding on wall
			isGliding = false;
			playerRB.drag = linearDrag;
		}

		if (!canDoubleJump)
		{
			doubleJumpCount = 0;
		}
		if(!isWallJumping && !isWallSliding && !isCrouching)
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
	
	    if (Input.GetButtonDown("Jump"))
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
	    else if (Input.GetButtonDown("Jump") && doubleJumpCount > 0)
	    {
	        Jump();
	        StartCoroutine(JumpCooldown());
	        doubleJumpCount--;
	    }
	
	    if (Input.GetButtonUp("Jump") && playerRB.velocity.y > 0f)
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
		if (Input.GetKey(KeyCode.S))
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
		if((Input.GetKeyUp(KeyCode.S) || !Input.GetKey(KeyCode.S)) && !IsTouchingRoof() )
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

		///WALLCLIMB CODE ONLY TWEAK LEFT SHIFT TO GLIDE AND THE GRAVITYSCALE
			/*if (IsOnWall() && !IsGrounded() && horizontal != 0f)
			{
				playerRB.gravityScale = canWallClimb ? 0f : 5f;
				isWallSliding = true;

				if (canWallClimb && Input.GetKey(KeyCode.LeftShift))
				{
					float verticalInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
					float wallYvel = Mathf.Clamp(playerRB.velocity.y, -wallSlideSpeed, wallSlideSpeed) + verticalInput * wallSlideSpeed;
					playerRB.velocity = new Vector2(playerRB.velocity.x, wallYvel);
				}
			}
			else
			{
				isWallSliding = false;
				playerRB.gravityScale = 5f;
			}*/
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

		if (Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
		{
			isWallJumping = true;
			playerRB.velocity = new Vector2(wallJumpDir * wallJumpingPower.x, wallJumpingPower.y);
			wallJumpCounter = 0f;
			canDash = true;
			canDashCode = true;
			doubleJumpCount = maxDoubleJumps;

			if(transform.localScale.x != wallJumpDir)
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
		float dir = transform.localScale.x;
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
		if(!isGliding){
			canUpDashCode = false;
			isUpDashing = true;
			float originalTRWidth = dashTrail.startWidth;
			float originalGravity = playerRB.gravityScale;
			playerRB.gravityScale = 0f;
			float dir = transform.localScale.x;
			playerRB.velocity = new Vector2(0f, transform.localScale.y * upDashingPower);
			dashTrail.startWidth = 0.78f;
			yield return new WaitForSeconds(upDashingTime);
			dashTrail.startWidth = originalTRWidth;
			playerRB.gravityScale = originalGravity;
			isUpDashing = false;
			if (IsGrounded() && !isUpDashing)
			{
				yield return new WaitForSeconds(upDashingCooldown);
				canUpDashCode = true;
			}
		}
	}

	private void Glide()
	{
		if (!IsGrounded() && Input.GetKey(KeyCode.LeftShift) && canGlideCode && !isGliding && !isUpDashing)
		{
			isGliding = true;
			playerRB.drag = glideDrag;
		}
		if(IsGrounded() || Input.GetKeyUp(KeyCode.LeftShift))
		{
			isGliding = false;
			playerRB.drag = linearDrag;
		}
	}
}