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
    public bool giveGrapple;

    void Start ()
    {
        playerMovement = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
    }
    public void GivePlayerAbility()
    {
        playerMovement.canDoubleJump = giveDoubleJump;
        playerMovement.maxDoubleJumps = noOfDoubleJumps;

        playerMovement.canWallJump = giveWallJump;
        playerMovement.canDash = giveDash;
        playerMovement.canUpDash = giveUpDash;
        playerMovement.canGlide = giveGlide;
        playerMovement.canGrapple = giveGrapple;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            GivePlayerAbility();
        }
    }
}
