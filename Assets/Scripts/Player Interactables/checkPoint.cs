using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    private gameController GameController;
    private BoxCollider2D trigger;
    
    private void Start(){
        trigger = GetComponent<BoxCollider2D>();
        trigger.enabled = true;
        GameController = GameObject.FindGameObjectWithTag("Player").GetComponent<gameController>();
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            GameController.UpdateCheckpoint(transform.position);
            trigger.enabled = false;
        }
    }
}
