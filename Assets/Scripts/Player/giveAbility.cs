using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveAbility : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public int noOfDoubleJumps;

    void Start ()
    {
        playerMovement = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerMovement.canDoubleJump = true;
            playerMovement.maxDoubleJumps = noOfDoubleJumps;
        }
    }
}
