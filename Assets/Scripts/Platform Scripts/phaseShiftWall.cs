using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phaseShiftWall : MonoBehaviour
{

    private PlayerMovement playerMovement;
    public bool isDashBreakable = false;
    public bool isUpDashBreakable = false;
    public float wallReformingTime = 10f;
    public Collider2D dashDisableColl;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerStay2D (Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if((playerMovement.wasDashing == true || playerMovement.isDashing == true) && isDashBreakable){
                StartCoroutine(destroyWall());
            }
            if((playerMovement.wasUpDashing == true || playerMovement.isUpDashing == true) && isUpDashBreakable){
                StartCoroutine(destroyWall());
            }
        }
    }

    public IEnumerator destroyWall()
    {
        dashDisableColl.enabled = false;
        yield return new WaitForSeconds(wallReformingTime);
        dashDisableColl.enabled = true;
    }
}
