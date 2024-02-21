using System.Collections;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float checkDistance = 0.05f;
    public bool moveOnlyIfPlayerIsOn = true;

    private Transform targetWaypoint;
    private int currentWaypointIndex = 0;
    private float originalSpeed;
    private bool playerOnPlatform = false;
    private bool movingToNextWaypoint = false;

    private void Start()
    {
        targetWaypoint = waypoints[currentWaypointIndex];
        originalSpeed = speed;
    }

    private void Update()
    {
        if (movingToNextWaypoint)
        {
            movePlatform();
        }

        if (!movingToNextWaypoint && !moveOnlyIfPlayerIsOn)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
            movePlatform();
        }

        if (Vector2.Distance(transform.position, targetWaypoint.position) < checkDistance + 10)
        {
            speed = 4;
        }
        else
        {
            speed = originalSpeed;
        }
    }

    private void movePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < checkDistance && !movingToNextWaypoint)
        {
            movingToNextWaypoint = true;
            StartCoroutine(WaitForPlayerToLeave());
        }
    }

    private IEnumerator WaitForPlayerToLeave()
    {
        Collider2D playerCollider = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                playerCollider = collider;
            }
        }

        yield return new WaitUntil(() => playerCollider == null || !playerOnPlatform);

        playerOnPlatform = false;
        movingToNextWaypoint = false;
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
        }

        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.5f)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
        }
        else
        {
            movingToNextWaypoint = true;
            StartCoroutine(MoveToNextWaypoint());
        }
    }

    private IEnumerator MoveToNextWaypoint()
    {
        float distToWaypoint = Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position);
        speed = originalSpeed;

        while (distToWaypoint > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);
            distToWaypoint = Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position);
            yield return null;
        }

        movingToNextWaypoint = false;
        targetWaypoint = waypoints[currentWaypointIndex];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
            playerOnPlatform = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && moveOnlyIfPlayerIsOn)
        {
            movingToNextWaypoint = false;
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
