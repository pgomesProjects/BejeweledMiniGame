using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private int minutes, seconds;

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

            if(seconds == 60)
            {
                seconds = 0;
                minutes++;
            }

            timerText.text = "Time: " + ToString();
        }
    }

    public override string ToString()
    {
        if(seconds < 10)
            return minutes + ":0" + seconds;

        return minutes + ":" + seconds;
    }
}
