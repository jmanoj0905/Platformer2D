using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncePlatform : MonoBehaviour
{
    private GameObject player;
    PlayerMovement charController;
    public float jumpBoostVal = 20f;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		charController = player.GetComponent<PlayerMovement>();
	}
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == player)
		{
			float originalJumpPow = charController.jumpingPower;
			charController.jumpingPower += jumpBoostVal;
			charController.maxDoubleJumps += 1;
			charController.Jump();
			charController.jumpingPower = originalJumpPow;
			charController.maxDoubleJumps--;
		}
	}
}
