using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementsController : MonoBehaviour
{
    [SerializeField] private GameObject achievementHolder;
    [SerializeField] private GameObject achievementObject;
    [SerializeField] private AchievementItem[] achievementItem;
    [SerializeField] private Color lockedImageColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color lockedTextColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color unlockedImageColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color unlockedTextColor = new Color(1, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        CheckForAchievements();
        CreateAchievementsBoard();
    }

    private void CheckForAchievements()
    {
        foreach(var i in achievementItem)
        {
            //If the player prefs say the achievement is unlocked, make sure it is unlocked
            if(PlayerPrefs.GetInt("AchievementID" + i.id) == 1 || i.isUnlocked)
            {
                i.isUnlocked = true;
            }
        }
    }

    private void CreateAchievementsBoard()
    {
        int counter = 0;
        foreach (var i in achievementItem)
        {
            //If the achievement is not hidden or is unlocked, display it
            if (!i.isHidden || i.isUnlocked)
            {
                GameObject currentAchievement = Instantiate(achievementObject);
                currentAchievement.transform.SetParent(achievementHolder.transform);
                currentAchievement.GetComponentInChildren<TextMeshProUGUI>().text = i.name + " - " + i.description;

                //If the achievement is locked, grey it out
                if (!i.isUnlocked)
                {
                    currentAchievement.transform.Find("AchievementImage").GetComponent<Image>().color = lockedImageColor;
                    currentAchievement.GetComponentInChildren<TextMeshProUGUI>().color = lockedTextColor;
                }
                //If unlocked, show the full colors
                else
                {
                    currentAchievement.transform.Find("AchievementImage").GetComponent<Image>().color = unlockedImageColor;
                    currentAchievement.GetComponentInChildren<TextMeshProUGUI>().color = unlockedTextColor;
                }
            }
            counter++;
        }
    }
}
