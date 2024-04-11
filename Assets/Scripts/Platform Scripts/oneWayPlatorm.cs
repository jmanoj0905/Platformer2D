using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class oneWayPlatorm : MonoBehaviour
{
    public Collider2D disableCollider;

    public void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(Input.GetKey(KeyCode.S))
            {
                StartCoroutine(disableColl());
            }
        }
    }

    private IEnumerator disableColl()
    {
        disableCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        disableCollider.enabled = true;
    }
}
