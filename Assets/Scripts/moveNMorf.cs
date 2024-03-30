using UnityEngine;
using System.Collections;
using DG.Tweening;

public class moveNMorf : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRB;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public bool canCrouch = true;
	public float crouchSpeed = 4f;
	[SerializeField] private Transform roofCheck;
	public Collider2D crouchDisableCollider;
	//private bool isCrouching = false;
    public float speed = 8f;

    [Header("Jumps")]
    public float jumpingPower = 20f; // Power of the jump
    public float jumpBufferTime = 0.2f; // Time window for buffered jump input
    public float coyoteTime = 0.2f; // Grace period after leaving ground where jump is still allowed
    private float coyoteTimeCounter; // Counter for coyote time
    private float jumpBufferCounter; // Counter for jump buffer time
    private bool isJumping; // Flag indicating if the player is currently jumping
    public bool canDoubleJump = true; // Flag indicating if double jumping is allowed
    public int maxDoubleJumps = 1; // Maximum number of allowed double jumps
    private int doubleJumpCount; // Counter for double jumps

    private bool wasDescending = false;
    private float originalSpeed;

	void Start()
	{
		originalSpeed = speed;
	}
    private void Update()
    {
        HandleJumping();
        Crouch();

        if(playerRB.velocity.y < 0.1f){ //? If the player is descending
            EndSqiush();
        }

        if(wasDescending && IsGrounded()){
            Splatter();
        }
        wasDescending = playerRB.velocity.y < -13.0f;
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
            //? Start of the jump
            Jump();
            StartCoroutine(JumpCooldown());
        }
        else if (Input.GetButtonDown("Jump") && doubleJumpCount > 0)
        {
            //? Start of a double jump
            Jump();
            StartCoroutine(JumpCooldown());
            doubleJumpCount--;
        }

        if (Input.GetButtonUp("Jump") && playerRB.velocity.y > 0f)
        {
            //? Peak of the jump
            SoftLand();
            coyoteTimeCounter = 0f;
        }
    }
    private void StartSquish()
    {
        transform.DOScaleY(2,0.3f);
        transform.DOScaleX(0.5f,0.3f);
    }
    private void EndSqiush()
    {
        transform.DOScale(new Vector3(1,1.5f,1),0.3f);
    }
    private void Splatter()
    {
        transform.DOScaleX(1.5f,0.3f);
        transform.DOScaleY(0.5f,0.3f);
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
				//isCrouching = true;
				speed = crouchSpeed;
				crouchDisableCollider.enabled = false;
				transform.localScale = new Vector2(transform.localScale.x, 1f);
				canDoubleJump = false;
			}
		}
		if((Input.GetKeyUp(KeyCode.S) || !Input.GetKey(KeyCode.S)) && !IsTouchingRoof() )
		{
			//isCrouching = false;
			speed = originalSpeed;
			crouchDisableCollider.enabled = true;
			transform.localScale = new Vector2(transform.localScale.x, 1.5f);
			canDoubleJump = true;
		}
	}
    public void Jump()
    {
        StartSquish();
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpingPower);
        jumpBufferCounter = 0f;
    }

    private void SoftLand()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * 0.5f);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private IEnumerator JumpCooldown()
	{
		isJumping = true;
		yield return new WaitForSeconds(0.4f);
		isJumping = false;
	}
}
