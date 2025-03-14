using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    private gameController GameController;
    private PlayerMovement PlayerMovement;
    private BoxCollider2D trigger;
    
    private void Start(){
        trigger = GetComponent<BoxCollider2D>();
        trigger.enabled = true;
        GameController = GameObject.FindGameObjectWithTag("Player").GetComponent<gameController>();
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            GameController.UpdateCheckpoint(transform.position);
            PlayerMovement.isFacingRight = true;
            trigger.enabled = false;
        }
    }
}
