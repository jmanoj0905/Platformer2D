using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlatform : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float checkDistance = 0.05f;
    public bool moveOnlyIfPlayerIsOn = true;
    private Transform targetWaypoint;
    private int currentWaypointIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        targetWaypoint = waypoints[0];
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
	}
	// Update is called once per frame
	public void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed*Time.deltaTime);
        if(Vector2.Distance(transform.position, targetWaypoint.position) < checkDistance)
        {
            targetWaypoint = GetNextWaypoint();
        }
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.transform.SetParent(this.transform);
            if (!moveOnlyIfPlayerIsOn) { }
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
		}
	}
}
