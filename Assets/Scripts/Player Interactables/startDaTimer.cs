using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startDaTimer : MonoBehaviour
{
    public Patient Patient;

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            Patient.canWeStart = true;
        }
    }
}
