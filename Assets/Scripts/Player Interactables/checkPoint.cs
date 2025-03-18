using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    private gameController gameController;
    private BoxCollider2D trigger;

    private void Start()
    {
        trigger = GetComponent<BoxCollider2D>();
        trigger.enabled = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            gameController = player.GetComponent<gameController>();
        }
        else
        {
            Debug.LogError("Player not found! Make sure the Player has the 'Player' tag.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameController != null)
        {
            gameController.UpdateCheckpoint(transform.position);
            gameObject.SetActive(false);
        }
    }
}
