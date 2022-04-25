using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private int hours, minutes, seconds;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(ActiveTimer());
    }

    IEnumerator ActiveTimer()
    {
        //While the game is active, iterate the timer
        while (PlayerController.main.IsGameActive())
        {
            yield return new WaitForSeconds(1);

            seconds++;

            if (minutes == 60)
            {
                minutes = 0;
                hours++;
            }

            if (seconds == 60)
            {
                seconds = 0;
                minutes++;
            }

            timerText.text = "Time: " + ToString();
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
