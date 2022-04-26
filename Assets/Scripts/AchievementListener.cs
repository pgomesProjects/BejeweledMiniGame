using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementListener : MonoBehaviour
{
    [SerializeField] private GameObject achievementObject;
    [SerializeField] private TextMeshProUGUI achievementText;
    [SerializeField] private float achievementBoxFadeInSeconds = 0.5f;
    [SerializeField] private float achievementBoxShowSeconds = 3;
    [SerializeField] private float achievementBoxFadeOutSeconds = 0.5f;

    public static AchievementListener Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PointsAfterOneMinuteAchievement(60));
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
                UnlockAchievement(0);
                achieved = true;
            }

            yield return null;
        }
    }

    public void UnlockAchievement(int id)
    {
        StartCoroutine(ShowAchievement("Achievement Unlocked: " + AchievementHolder.Instance.achievementItem[id].name));
        PlayerPrefs.SetInt("AchievementID" + id, 1);
    }

    IEnumerator ShowAchievement(string message)
    {
        achievementText.text = message;

        float currentTimer = 0;
        CanvasGroup achievementCanvas = achievementObject.GetComponent<CanvasGroup>();

        //Fade in achievement box
        while (currentTimer < achievementBoxFadeInSeconds)
        {
            currentTimer += Time.deltaTime;
            achievementCanvas.alpha = Mathf.Clamp01(currentTimer / achievementBoxFadeInSeconds);
            yield return null;
        }

        achievementCanvas.alpha = 1;

        yield return new WaitForSeconds(achievementBoxShowSeconds);

        currentTimer = 0;

        //Fade out achievement box
        while (currentTimer < achievementBoxFadeOutSeconds)
        {
            currentTimer += Time.deltaTime;
            achievementCanvas.alpha = 1 - Mathf.Clamp01(currentTimer / achievementBoxFadeInSeconds);
            yield return null;
        }

        achievementCanvas.alpha = 0;
    }
}
