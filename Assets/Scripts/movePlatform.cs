using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlatformv2 : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float checkDistance = 0.05f;
    private int noOfTimesOnPlatform = 0;
    private bool moveOnlyIfPlayerIsOn = false;
    private Transform targetWaypoint;
    private int currentWaypointIndex = 0;
    private float originalSpeed;


    // Start is called before the first frame update
    void Start()
    {
        targetWaypoint = waypoints[0];
        originalSpeed = speed;
    }

    public Transform GetNextWaypoint()
    {
        currentWaypointIndex++;
        if(currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
        }
        return waypoints[currentWaypointIndex];
    }

	public void Update()
	{
		if (!moveOnlyIfPlayerIsOn)
        {
            MovePlatform();
        }
        if(Vector2.Distance(transform.position, targetWaypoint.position) < checkDistance+10)
        {
            speed = originalSpeed/2;
        }else{
            speed = originalSpeed;
        }
        if(!(Vector2.Distance(transform.position, targetWaypoint.position) < checkDistance))
        {
                noOfTimesOnPlatform = 0;
        }
	}
	// Update is called once per frame
	public void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed*Time.deltaTime);
        if(Vector2.Distance(transform.position, targetWaypoint.position) < checkDistance)
        {
            if(playerCheck())
            {
                targetWaypoint = GetNextWaypoint();
            }
        }
    }
    //Checks the odd no. of times the player jumps on the platform
    public bool playerCheck()
    {
        if(noOfTimesOnPlatform % 2 != 0){
            return true;
        }
        else{
            return false;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.transform.SetParent(this.transform);
            if(Vector2.Distance(transform.position, targetWaypoint.position) < checkDistance){
                noOfTimesOnPlatform ++;
            }
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
        if (collision.gameObject.CompareTag("Player") && moveOnlyIfPlayerIsOn)
        {
			MovePlatform();
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.transform.SetParent(null);
            //noOfTimesOnPlatform --;
		}
	}
}