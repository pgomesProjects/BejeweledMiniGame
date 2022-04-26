using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private int hours = 0, minutes = 0, seconds = 0;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        //Display beginning time
        timerText.text = "Time: " + ToString();

        StartCoroutine(ActiveTimer());
    }

    IEnumerator ActiveTimer()
    {
        //While the game is active, iterate the timer
        while (PlayerController.main.IsGameActive())
        {
            yield return new WaitForSeconds(1);

            seconds++;

            if (seconds == 60)
            {
                seconds = 0;
                minutes++;
            }

            if (minutes == 60)
            {
                minutes = 0;
                hours++;
            }

            //Check for achievements related to session time
            CheckTimerAchievements();

            //Display time
            timerText.text = "Time: " + ToString();
        }
    }

    private void CheckTimerAchievements()
    {
        //Achievement for 10 Minutes
        if (PlayerPrefs.GetInt("AchievementID2") != 1 && minutes >= 10)
        {
            if (AchievementListener.Instance != null)
                AchievementListener.Instance.UnlockAchievement(2);
        }

        //Achievement for 1 hour
        if (PlayerPrefs.GetInt("AchievementID5") != 1 && hours >= 1)
        {
            if (AchievementListener.Instance != null)
                AchievementListener.Instance.UnlockAchievement(5);
        }
    }

    public override string ToString()
    {
        if (hours > 0)
        {
            if(minutes < 10)
                return hours + ":0" + minutes + ":0" + seconds;
            else
                return hours + ":" + minutes + ":0" + seconds;
        }

        if (seconds < 10)
            return minutes + ":0" + seconds;

        return minutes + ":" + seconds;
    }
}
