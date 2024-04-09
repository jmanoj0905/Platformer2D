using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiamondReset : MonoBehaviour
{
	public float coolDownTimer = 2f;
    private PlayerMovement playerMovement;
    private Collider2D diamondColl;
	private SpriteRenderer diamondSprite;
	
	void Start()
	{
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		diamondColl = GetComponent<BoxCollider2D>();
		diamondSprite = GetComponent<SpriteRenderer>();
	}

    private IEnumerator DeActivate()
	{
		yield return new WaitForSecondsRealtime(0.1f);
		diamondColl.enabled = false;
		diamondSprite.enabled = false;
        resetJumps();
		yield return new WaitForSecondsRealtime(coolDownTimer);
		diamondColl.enabled = true;
		diamondSprite.enabled = true;
	}

    public void resetJumps()
    {
        playerMovement.doubleJumpCount = playerMovement.maxDoubleJumps + 1;
        playerMovement.canDashCode = true;
		playerMovement.upDashCount = playerMovement.originalUpDashCount;

    }

    void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			StartCoroutine(DeActivate());
		}
	}
}
