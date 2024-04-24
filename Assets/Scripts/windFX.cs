using UnityEngine;

public class WindFX : MonoBehaviour
{
    public float speed = 5f;
    public float amplitude = 1f;
    public float frequency = 1f;
    private float phase;

    private float initialX;
    private float initialY;
    private float horizontalMovement = 0f;

    void Start()
    {
        initialX = transform.localPosition.x;
        initialY = transform.localPosition.y;
        phase = Random.Range(-90, 90);
    }

    void FixedUpdate()
    {
        horizontalMovement += speed * Time.deltaTime;
        float verticalMovement = amplitude * Mathf.Sin(Time.time * frequency * 2 * Mathf.PI + phase);
        Vector3 newPosition = new Vector3(initialX + horizontalMovement, initialY+verticalMovement, 0f);
        transform.localPosition = newPosition;
    }
}
