using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    public float jumpBoostVal = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Apply immediate jump force upwards
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpBoostVal);
                
                if(Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space))
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, jumpBoostVal*1.5f);
                }
            }
        }
    }
}
