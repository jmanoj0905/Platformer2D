using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Vector2 spawnPoint; // Hardcoded spawn point
    public float respawnTime = 0.2f;
    private Rigidbody2D playerRb;
    private Vector3 playerScale;
    private TrailRenderer playerTrail;
    private bool isFacingRight = true;

    private void Start()
    {
        playerScale = transform.localScale;
        playerRb = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position; // Set initial spawn point
    }

    private void Update()
    {
        if(playerRb.velocity.x > 0){
            isFacingRight = true;
        }
        else{
            isFacingRight = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("spike"))
        {
            Die();
        }
    }

    public void Die()
    {
        StartCoroutine(Respawn(respawnTime));
    }

    private IEnumerator Respawn(float duration)
    {
        playerRb.simulated = false;
        if (playerTrail != null) playerTrail.enabled = false;
        transform.localScale = Vector3.zero;
        playerRb.velocity = Vector2.zero;
        yield return new WaitForSeconds(duration);
        transform.position = spawnPoint;
        transform.localScale = playerScale;
        transform.localScale = new Vector3(isFacingRight ? Mathf.Abs(playerScale.x) : -Mathf.Abs(playerScale.x), playerScale.y, playerScale.z);
        if (playerTrail != null) playerTrail.enabled = true;
        playerRb.simulated = true;
    }
}