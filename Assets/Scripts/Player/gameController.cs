using System.Collections;
using UnityEngine;

public class gameController : MonoBehaviour
{
    private Vector2 checkpointPos;
    public float respawnTime = 0.2f;
    private Rigidbody2D playerRb;
    private Vector3 playerScale;
    private TrailRenderer playerTrail;
    public PlayerMovement playerMovement;
    private bool facingRightBeforeRespawn;

    public GameObject playerVisuals; // 🎯 Drag & drop Player sprite/model here

    private void Start()
    {
        playerScale = transform.localScale;
        checkpointPos = transform.position;

        playerRb = GetComponent<Rigidbody2D>();
        playerTrail = GetComponent<TrailRenderer>();

        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        if (playerRb == null || playerTrail == null || playerMovement == null)
        {
            Debug.LogError("One or more required components are missing from Player.");
        }

        if (playerVisuals == null)
        {
            Debug.LogError("Player visuals (sprite/model) are not assigned!");
        }
    }

    private void Update()
    {
        if (playerRb == null || playerMovement == null) return;

        if (playerRb.velocity.x > 0 && !playerMovement.isFacingRight)
        {
            playerMovement.isFacingRight = true;
        }
        else if (playerRb.velocity.x < 0 && playerMovement.isFacingRight)
        {
            playerMovement.isFacingRight = false;
        }
    }

    public void Die()
    {
        StartCoroutine(Respawn(respawnTime));
    }

    public void UpdateCheckpoint(Vector2 pos)
    {
        Debug.Log("Checkpoint saved at: " + pos);
        checkpointPos = pos;
    }

    private IEnumerator Respawn(float duration)
    {
        playerRb.simulated = false;
        playerTrail.enabled = false;
        
        if (playerVisuals != null) playerVisuals.SetActive(false); // ❌ Hide visuals instead

        playerRb.velocity = Vector2.zero;
        facingRightBeforeRespawn = playerMovement.isFacingRight;

        yield return new WaitForSeconds(duration);

        transform.position = checkpointPos;
        
        if (playerVisuals != null) playerVisuals.SetActive(true); // ✅ Show visuals again
        playerTrail.enabled = true;
        playerRb.simulated = true;

        if (playerMovement.isFacingRight != facingRightBeforeRespawn)
        {
            //Flip();
        }

        Debug.Log("Respawned at: " + checkpointPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("spike"))
        {
            Die();
        }
    }
}
