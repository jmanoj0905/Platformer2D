using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveAbility : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public bool giveDoubleJump;
    public int noOfDoubleJumps;
    public bool giveWallJump;
    public bool giveDash;
    public bool giveUpDash;
    public bool giveGlide;
    //public bool giveGrapple;
    private BoxCollider2D trigger;
    void Start(){
        trigger = gameObject.GetComponent<BoxCollider2D>();
        trigger.enabled = true;
    }

    public void GivePlayerAbility()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if(playerMovement == null) Debug.Log("player movement not there !");
        playerMovement.canDoubleJump = giveDoubleJump;
        playerMovement.maxDoubleJumps = noOfDoubleJumps;

        playerMovement.canWallJump = giveWallJump;
        playerMovement.canDash = giveDash;
        playerMovement.canUpDash = giveUpDash;
        playerMovement.canGlide = giveGlide;
        trigger.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            GivePlayerAbility();
        }
    }
}
