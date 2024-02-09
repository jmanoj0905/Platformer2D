using System.Collections;
using UnityEngine;

public class NorthMagnet : MonoBehaviour
{
    public LayerMask southPole;
    public int numberOfRaycasts = 90;
    private float magneticForce = 0.2f;
    public Rigidbody2D rb;
    public float raycastDelay = 0.05f; // Adjust the delay between each set of raycasts

    private void Start()
    {
        StartCoroutine(CastRays());
    }

    private IEnumerator CastRays()
    {
        while (true)
        {
            for (int i = 0; i < numberOfRaycasts; i++)
            {
                float angle = i * (360f / numberOfRaycasts);
                Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * Vector2.right;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, Mathf.Infinity, southPole);
                Debug.DrawRay(transform.position, rayDirection * 10, Color.green); // Draw the ray in the Scene view
                // Check if the ray hit something
                if (hit.collider != null && hit.collider.CompareTag("SouthPole"))
                {
                    Vector2 southPolePos = hit.collider.gameObject.transform.position;
                    Attract(southPolePos);
                    // Do something with the hit object
                }
            }
            yield return new WaitForSeconds(raycastDelay); // Wait for the specified delay before casting the next set of rays
        }
    }

    public void Attract(Vector2 southPolePos)
    {
        Vector2 attractDirection = southPolePos - (Vector2)transform.position;
        rb.AddForce(attractDirection.normalized * magneticForce, ForceMode2D.Force);
    }
}