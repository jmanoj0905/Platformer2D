using System.Collections;
using UnityEngine;

public class windSpawn : MonoBehaviour
{
    public GameObject windTrailPrefab;
    public Transform bottomLeft;
    public Transform bottomRight;
    public Transform top;

    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;

    //private TrailRenderer windTrailRenderer;
    public Collider2D windTrailCollider2D;
    public windFX windFXScript;

    private void Start()
    {
        StartCoroutine(SpawnWindTrail());
        windTrailCollider2D = windTrailPrefab.GetComponent<Collider2D>();
    }
    private IEnumerator SpawnWindTrail()
    {
        while (true)
        {
            // Get a random position along the x-axis within the bounds of the bottom GameObject
            float randomX = Random.Range(bottomLeft.position.x , bottomRight.position.x);
            // Set the spawn position to be at the y-level of the bottom GameObject
            Vector3 spawnPosition = new Vector3(randomX, bottomLeft.position.y, 0f);
            windFXScript.phase = Random.Range(-90,90);
            // Spawn the windTrailPrefab as a child of this object
            GameObject windTrailObject = Instantiate(windTrailPrefab, spawnPosition, Quaternion.identity);

            // Wait for a random time before spawning the next windTrail
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
}