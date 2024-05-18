using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class gameController : MonoBehaviour
{
    private Vector2 checkpointPos;
    public float respawnTime = .2f;
    private Rigidbody2D playerRb;
    private Vector3 playerScale;
    private TrailRenderer playerTrail;

    private void Start(){
        playerScale = transform.localScale;
        Debug.Log(playerScale);
        checkpointPos = transform.position;
        playerRb = GetComponent<Rigidbody2D>();
        playerTrail = GetComponent<TrailRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("spike")){
            //playerScale = transform.localScale;
            //Debug.Log(playerScale);
            Die();
        }
    }
    public void Die(){
        StartCoroutine(Respawn(respawnTime));
    }
    public void UpdateCheckpoint(Vector2 pos){
        Debug.Log("Checkpoint saved!");
        checkpointPos = pos;
    }
    private IEnumerator Respawn(float duration){
        playerRb.simulated = false;
        playerTrail.enabled = false;
        transform.localScale = new Vector3(0,0,0);
        playerRb.velocity = new Vector2(0,0);
        yield return new WaitForSeconds(duration);
        transform.position = checkpointPos;
        transform.localScale = playerScale;
        playerTrail.enabled = true;
        playerRb.simulated = true;
    }
}
