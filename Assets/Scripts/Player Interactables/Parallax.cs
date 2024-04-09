using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float parallaxAmountX = 0.5f; // Adjust these values as needed for the desired effect
    public float parallaxAmountY = 0.5f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the parallax effect based on the camera's movement
        float parallaxX = (Camera.main.transform.position.x - initialPosition.x) * parallaxAmountX;
        float parallaxY = (Camera.main.transform.position.y - initialPosition.y) * parallaxAmountY;

        // Apply the parallax effect to the object's position
        transform.position = new Vector3(initialPosition.x + parallaxX, initialPosition.y + parallaxY, transform.position.z);
    }
}
