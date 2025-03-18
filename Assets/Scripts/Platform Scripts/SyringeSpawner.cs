using System.Collections;
using UnityEngine;

public class SyringeSpawner : MonoBehaviour
{
    public GameObject prefab; // Assign prefab in Inspector
    public float spawnInterval = 2f; // Time between spawns
    public Vector2 moveDirection = Vector2.down; // Default movement direction (south)
    public float spawnDelay = 0.0f; // Delay before first spawn

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        // Apply initial spawn delay before starting the loop
        yield return new WaitForSeconds(spawnDelay);

        while (true)
        {
            GameObject spawnedObject = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, 270));


            Rigidbody2D rb = spawnedObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = moveDirection * 5f; // Adjust speed as needed
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
