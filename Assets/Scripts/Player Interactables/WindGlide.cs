using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindGlide : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerController;
    private Rigidbody2D playerRB;

    public float windForce = 3f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        playerController = player.GetOrAddComponent<PlayerMovement>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.LeftShift) && playerController.isGliding)
            {
                playerRB.AddForce(new Vector2(0,windForce), ForceMode2D.Impulse); // Set gravity to 0 to make the player float
            }
        }
    }
}
