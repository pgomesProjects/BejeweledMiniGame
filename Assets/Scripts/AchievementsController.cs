using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementsController : MonoBehaviour
{
    [SerializeField] private GameObject achievementHolder;
    [SerializeField] private GameObject achievementObject;
    [SerializeField] private Sprite lockedImage;
    [SerializeField] private Color lockedImageColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color lockedTextColor = new Color(1, 1, 1, 1);
    [SerializeField] private Sprite unlockedImage;
    [SerializeField] private Color unlockedImageColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color unlockedTextColor = new Color(1, 1, 1, 1);

    public static AchievementsController Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateAchievementsBoard();
    }

    public void CreateAchievementsBoard()
    {
        ClearAchievementsBoard();

        int counter = 0;
        foreach (var i in AchievementHolder.Instance.achievementItem)
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
                    currentAchievement.transform.Find("AchievementImage").GetComponent<Image>().sprite = lockedImage;
                    currentAchievement.transform.Find("AchievementImage").GetComponent<Image>().color = lockedImageColor;
                    currentAchievement.GetComponentInChildren<TextMeshProUGUI>().color = lockedTextColor;
                }
                //If unlocked, show the full colors
                else
                {
                    currentAchievement.transform.Find("AchievementImage").GetComponent<Image>().sprite = unlockedImage;
                    currentAchievement.transform.Find("AchievementImage").GetComponent<Image>().color = unlockedImageColor;
                    currentAchievement.GetComponentInChildren<TextMeshProUGUI>().color = unlockedTextColor;
                }
            }
            counter++;
        }
    }

    public void ClearAchievementsBoard()
    {
        foreach (Transform child in achievementHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
