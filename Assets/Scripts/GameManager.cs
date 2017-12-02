using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public float gameTime = 10;
    public float timeLeft;
    public Text timerText;
    public Text winText;
    public bool gameEnded;

    [Range(2, 4)]
    public int playerAmount = 2;

    public List<Player> playerList = new List<Player>();

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

        if (timeLeft <= 0 && !gameEnded)
        {
            GameEnd();
        }
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

        timerText.text = "Time Left: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void GameEnd()
    {
      Player topPlayer = playerList[0];
      foreach (Player player in playerList)
      {
          if (player.playerMoney > topPlayer.playerMoney)
          {
              topPlayer = player;
          }
      }

      timerText.gameObject.SetActive(false);
      winText.gameObject.SetActive(true);
      winText.text = "Player " + topPlayer.playerID + " wins \n With " + topPlayer.playerMoney + " moneys!";

      foreach (Player player in playerList)

      gameEnded = true;
    }


}
