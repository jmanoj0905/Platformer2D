using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthMagnet : MonoBehaviour
{
    public LayerMask southPole;
    public int numberOfRaycasts = 90;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsOppMagNearBy(); //Gives true or false based on if a magnet is nearby
        if(IsOppMagNearBy())
        {
                for (int i = 0; i < numberOfRaycasts; i++)
            {
                float angle = i * (360f / numberOfRaycasts);
                Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * Vector2.right;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 10, southPole);
                Debug.DrawRay(transform.position, rayDirection * 10, Color.green); // Draw the ray in the Scene view

                // Check if the ray hit something
                if (hit.collider != null)
                {
                    Debug.Log("Hit something at angle: " + angle);
                    // Do something with the hit object
                }
            }
        }
        
    }
    public bool IsOppMagNearBy()
    {
        return Physics2D.OverlapCircle(transform.position , 10f, southPole);
    }
}
