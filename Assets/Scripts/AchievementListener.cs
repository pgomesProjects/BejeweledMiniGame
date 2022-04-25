using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementListener : MonoBehaviour
{
    [SerializeField] private GameObject achievementObject;
    [SerializeField] private TextMeshProUGUI achievementText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PointsAfterOneMinuteAchievement(60));
        StartCoroutine(AchievementAfterTenMinutes());
        StartCoroutine(AchievementAfterHour());
    }

    IEnumerator PointsAfterOneMinuteAchievement(float secondsUntilExpire)
    {
        float currentTime = 0;
        bool achieved = PlayerPrefs.GetInt("AchievementID0") == 1;

        while(currentTime < secondsUntilExpire && !achieved)
        {
            currentTime += Time.deltaTime;

            if(PlayerController.main.GetPlayerScore() >= 1000)
            {
                StartCoroutine(ShowAchievement("Achievement Unlocked: Speed Matcher"));
                PlayerPrefs.SetInt("AchievementID0", 1);
                achieved = true;
            }

            yield return null;
        }
    }

    IEnumerator AchievementAfterTenMinutes()
    {
        float currentTime = 0;
        bool achieved = PlayerPrefs.GetInt("AchievementID2") == 1;

        while (currentTime < 600 && !achieved)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        if (!achieved)
        {
            StartCoroutine(ShowAchievement("Achievement Unlocked: A Nice Cup Of Coffee"));
            PlayerPrefs.SetInt("AchievementID2", 1);
        }
    }


    IEnumerator AchievementAfterHour()
    {
        float currentTime = 0;
        bool achieved = PlayerPrefs.GetInt("AchievementID5") == 1;

        while (currentTime < 3600 && !achieved)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        if (!achieved)
        {
            StartCoroutine(ShowAchievement("Achievement Unlocked: Mindless Addict"));
            PlayerPrefs.SetInt("AchievementID5", 1);
        }
    }

    IEnumerator ShowAchievement(string message)
    {
        achievementText.text = message;
        achievementObject.SetActive(true);
        
        yield return new WaitForSeconds(3);

        achievementObject.SetActive(false);
    }
}
