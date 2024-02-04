using System.Collections;
using UnityEngine;

public class dissapearPlatform : MonoBehaviour
{
	public float dissolveDelay = 1f;
	public float respawnDelay = 1f;
	private Collider2D platfromCollider;
	private SpriteRenderer platformRenderer;
	
	void Start()
	{
		platfromCollider = GetComponent<BoxCollider2D>();
		platformRenderer = GetComponent<SpriteRenderer>();
	}
	private IEnumerator Dissolve()
	{
		yield return new WaitForSecondsRealtime(dissolveDelay);
		platfromCollider.enabled = false;
		platformRenderer.enabled = false;
		yield return new WaitForSecondsRealtime(respawnDelay);
		platfromCollider.enabled = true;
		platformRenderer.enabled = true;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			StartCoroutine(Dissolve());
		}
	}
}
