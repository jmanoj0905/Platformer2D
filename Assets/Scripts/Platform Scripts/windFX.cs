using UnityEngine;
using System.Collections;

public class windFX : MonoBehaviour
{
    public float speed = 5f;
    public float amplitude = 1f;
    public float frequency = 1f;
    [HideInInspector]public float phase;

    private float initialX;
    private float initialY;
    private float horizontalMovement = 0f;
    private TrailRenderer windTrailRenderer;

    void Start()
    {
        initialX = transform.localPosition.x;
        initialY = transform.localPosition.y;
        windTrailRenderer = GetComponent<TrailRenderer>();
    }

    void FixedUpdate()
    {
        horizontalMovement += speed * Time.deltaTime;
        float verticalMovement = amplitude * Mathf.Sin((Time.time * frequency * 2 * Mathf.PI)+phase);
        Vector3 newPosition = new Vector3(initialX + verticalMovement , initialY + horizontalMovement, 0f);
        transform.localPosition = newPosition;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("windTrailEnd"))
        {
            StartCoroutine(DelObj());
        }
    }

    public IEnumerator DelObj()
    {
        yield return new WaitForSeconds(windTrailRenderer.time);
        // Destroy the windTrailObject
        Destroy(gameObject);
    }
}
