using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public float gameTime = 10;
    public float timeLeft;
    public Text timer;

	// Use this for initialization
	void Start () 
    {
        timeLeft = 60 * gameTime;
        UpdateLevelTimer(timeLeft);
	}
	
	// Update is called once per frame
	void Update () 
    {
        timeLeft -= Time.deltaTime;
        UpdateLevelTimer(timeLeft);
    }

    public void UpdateLevelTimer(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.RoundToInt(totalSeconds % 60f);

        string formatedSeconds = seconds.ToString();

        if (seconds == 60)
        {
            seconds = 0;
            minutes += 1;
        }

        timer.text = "Time Left: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
