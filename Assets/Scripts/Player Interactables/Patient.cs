using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Patient : MonoBehaviour
{
    public float PatientTime = 120.0f;
    private bool playerMoved = false;
    [HideInInspector] public bool canWeStart = false;
    private Vector3 lastPosition;
    public TextMeshProUGUI PatientTimeText;
    public Slider HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        playerMoved = false;
        if(HealthBar){
            HealthBar.maxValue = PatientTime;
            HealthBar.value = PatientTime;
        }
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
        if (playerMoved && canWeStart)
        {
            decrementTime();
        }
    }
    void decrementTime(){
        PatientTime -= Time.deltaTime;
        //Debug.Log(PatientTime);
        if (PatientTimeText != null)
        {
            PatientTimeText.text = (PatientTime).ToString();
        }
        if(HealthBar){
            HealthBar.value = PatientTime;
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player")){
            //GAME FINISHED STORE THE TIMER IN LEADER~BOARD or something
            SceneManager.LoadScene("HospitalEnv");
        }
    }
    public void AddTime(float extraTime)
    {
        PatientTime += extraTime; // Assuming you have a timer variable
    }

}
