using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncePlatform : MonoBehaviour
{
    private GameObject player;
	private Rigidbody2D playerRb;
    //PlayerMovement charController;

    public float jumpBoostVal = 20f;
	private Vector2 platformDir;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
		//charController = player.GetComponent<PlayerMovement>();
		platformDir = transform.up;
	}
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == player)
		{
			playerRb.AddForce(platformDir*jumpBoostVal, ForceMode2D.Impulse);
		}
	}
}
