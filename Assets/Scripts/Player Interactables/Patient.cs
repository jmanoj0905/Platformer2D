using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Patient : MonoBehaviour
{
    public float PatientTime = 120.0f;
    private bool playerMoved = false;
    private Vector3 lastPosition;
    public TextMeshProUGUI PatientTimeText;

    // Start is called before the first frame update
    void Start()
    {
        playerMoved = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player moved
        if (transform.position != lastPosition)
        {
            playerMoved = true;
            lastPosition = transform.position;
        }

        // Start decrementing time once the player moves
        if (playerMoved)
        {
            decrementTime();
        }
    }
    void decrementTime(){
        PatientTime -= Time.deltaTime;
        Debug.Log(PatientTime);
        if (PatientTimeText != null)
        {
            PatientTimeText.text = (PatientTime).ToString();
        }
    }
}
