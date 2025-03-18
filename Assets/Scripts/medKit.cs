using UnityEngine;

public class MedKit : MonoBehaviour
{
    public float timeBonus = 10f; // Adds 10 seconds
    public Patient patient;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if player collects it
        {
            if (patient != null)
            {
                patient.AddTime(timeBonus); // Add time
            }
            Destroy(gameObject); // Remove medkit after collection
        }
    }
}