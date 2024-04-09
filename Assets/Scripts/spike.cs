using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

public class spike : MonoBehaviour
{
    private GameObject player;
    //private SpriteRenderer playerSpriteRenderer;
    //private PlayerMovement playerMovementScript;

    public GameObject SpawnPoint;
    //[HideInInspector]public bool isPlayerDead = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator Respawn()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        player.transform.position = new Vector2(SpawnPoint.transform.position.x,SpawnPoint.transform.position.y);
        player.SetActive(true);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //isPlayerDead = true;
            StartCoroutine(Respawn());
        }
    }
}
