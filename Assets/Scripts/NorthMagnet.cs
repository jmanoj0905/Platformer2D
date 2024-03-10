using UnityEngine;

public class NorthMagnet : MonoBehaviour
{
    public bool isItAMag;
    public bool canMove = true; // Variable to control if the magnet can move
    public LayerMask oppositePoleLayer; // LayerMask to detect south pole magnets
    public float raycastDistance = 5f; // Distance to cast raycasts
    public float raycastAngleGap = 6f; // Angle gap between raycasts
    public float magneticForce = 5f; // Strength of the magnetic force
    public Rigidbody2D rb; // Rigidbody2D component of the magnet

    private void Update()
    {
        if (isItAMag && canMove) // Check if it's a magnet and if it's allowed to move
        {
            // Cast raycasts in both directions
            CastRaycasts(Vector2.right);
            CastRaycasts(Vector2.left);
        }
    }

    private void CastRaycasts(Vector2 direction)
    {
        // Calculate the number of raycasts based on the angle gap
        int numberOfRaycasts = Mathf.CeilToInt(360f / raycastAngleGap);
        Vector2 totalForce = Vector2.zero;
        int hitCount = 0;

        for (int i = 0; i < numberOfRaycasts; i++)
        {
            // Calculate the angle for the current raycast
            float angle = i * raycastAngleGap;
            // Rotate the direction vector by the angle
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction;

            // Cast a raycast in the current direction
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, raycastDistance, oppositePoleLayer);

            // Draw debug raycast lines in the Scene view
            Debug.DrawRay(transform.position, rayDirection * raycastDistance, Color.green);

            // Check if the raycast hits a south pole magnet
            if (hit.collider != null && hit.collider.CompareTag("SouthPole"))
            {
                // Calculate the attract direction towards the south pole magnet
                Vector2 attractDirection = hit.collider.transform.position - transform.position;
                // Calculate the inverse square of the distance
                float distanceSquared = attractDirection.sqrMagnitude;
                // Calculate the force based on the inverse square of the distance
                float forceMagnitude = magneticForce / distanceSquared;

                // Add force to the total force vector
                totalForce += attractDirection.normalized * forceMagnitude;
                hitCount++;
            }
            if (hit.collider != null && hit.collider.CompareTag("NorthPole"))
            {
                // Calculate the attract direction towards the south pole magnet
                Vector2 repelDirection = -(hit.collider.transform.position - transform.position);
                // Calculate the inverse square of the distance
                float distanceSquared = repelDirection.sqrMagnitude;
                // Calculate the force based on the inverse square of the distance
                float forceMagnitude = magneticForce / distanceSquared;

                // Add force to the total force vector
                totalForce += repelDirection.normalized * -forceMagnitude;
                hitCount++;
            }
        }

        // Distribute the total force among the hit raycasts
        if (hitCount > 0)
        {
            Vector2 averageForce = totalForce / hitCount;
            // Apply the average force to the rigidbody
            rb.AddForce(averageForce, ForceMode2D.Force);
        }
    }
}
