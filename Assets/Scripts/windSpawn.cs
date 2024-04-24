using System.Collections;
using UnityEngine;

public class WindSpawn : MonoBehaviour
{
    public GameObject bLeft;
    public GameObject bRight;
    public GameObject top;
    public GameObject windTrailPrefab;
    public float spawnDiffTime = 2f;
    public int numberOfPrefabsToSpawn = 5; // Adjust the number of prefabs to spawn

    private int prefabsSpawned = 0;

    void Start()
    {
        StartCoroutine(SpawnPrefabs());
    }

    private IEnumerator SpawnPrefabs()
    {
        while (prefabsSpawned < numberOfPrefabsToSpawn)
        {
            // Randomly determine the position between the bottom GameObjects
            float randomX = Random.Range(bLeft.transform.position.x, bRight.transform.position.x);
            float randomY = Random.Range(bLeft.transform.position.y, bLeft.transform.position.y);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

            // Instantiate the prefab at the random position
            GameObject spawnedPrefab = Instantiate(windTrailPrefab, spawnPosition, Quaternion.identity);

            // Calculate the wind direction
            Vector3 windDirection = (transform.up.y > 0) ? Vector3.up : Vector3.down;

            // Calculate the rotation to face the wind direction
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, windDirection);

            // Apply the rotation to the spawned prefab
            spawnedPrefab.transform.rotation = rotation;

            // Set the parent to this object to keep the hierarchy clean
            spawnedPrefab.transform.SetParent(transform);

            // Get the child object of the spawned prefab
            GameObject child = spawnedPrefab.transform.GetChild(0).gameObject;

            // Check if the child object is above the top GameObject
            if (child.transform.position.y > top.transform.position.y)
            {
                Debug.Log("yeeee");
                Destroy(spawnedPrefab);
                //Destroy(child);
            }

            prefabsSpawned++;

            // Wait for the specified time before spawning the next prefab
            yield return new WaitForSeconds(spawnDiffTime);
        }
    }
}
