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
    }

    IEnumerator PointsAfterOneMinuteAchievement(float secondsUntilExpire)
    {
        float currentTime = 0;
        bool achieved = false;
        while(currentTime < secondsUntilExpire && !achieved)
        {
            currentTime += Time.deltaTime;

            if(PlayerController.main.GetPlayerScore() >= 1000)
            {
                StartCoroutine(ShowAchievement("Achievement Got: Speed Matcher"));
                achieved = true;
            }

            yield return null;
        }
    }

    IEnumerator ShowAchievement(string message)
    {
        achievementText.text = message;
        achievementObject.SetActive(true);
        
        yield return new WaitForSeconds(3);

        achievementObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
