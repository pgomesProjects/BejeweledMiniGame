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
        while (true)
        {
            yield return new WaitForSeconds(1);

            seconds++;

            if(seconds == 60)
            {
                seconds = 0;
                minutes++;
            }

            if(seconds < 10)
                timerText.text = "Time: " + minutes + ":0" + seconds;
            else
                timerText.text = "Time: " + minutes + ":" + seconds;
        }
    }
}
