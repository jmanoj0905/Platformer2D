using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
    public GameObject spawnPoint;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RespawnPlayer(collision.gameObject));
        }
    }

    IEnumerator RespawnPlayer(GameObject player)
    {
        // Disable player's sprite renderer and movement script
        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerRenderer.enabled = false;
        playerMovement.enabled = false;

        // Move player to spawn point
        player.transform.position = spawnPoint.transform.position;

        // Wait for a short delay
        yield return new WaitForSeconds(0.2f);

        // Enable player's sprite renderer and movement script
        playerRenderer.enabled = true;
        playerMovement.enabled = true;
    }
}
