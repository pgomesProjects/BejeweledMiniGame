using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearDataController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject confirmClearDataBox;
    [SerializeField] private GameObject clearConfirmedBox;

    [SerializeField] private TextMeshProUGUI personalBest;
    [SerializeField] private GameObject allAchievements;

    public void OpenConfirmClearData()
    {
        confirmClearDataBox.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ClearData()
    {
        //Clear all PlayerPrefs
        PlayerPrefs.DeleteAll();

        UpdatePersonalBestText();
        RefreshAchievementsPage();
    }

    private void UpdatePersonalBestText()
    {
        personalBest.text = "Personal Best: " + PlayerPrefs.GetInt("PersonalBest");
    }

    private void RefreshAchievementsPage()
    {
        //Get rid of trophy
        allAchievements.SetActive(false);

        //Lock all achievements
        foreach (var i in AchievementHolder.Instance.achievementItem)
            i.isUnlocked = false;

        //Show successful clear box
        confirmClearDataBox.SetActive(false);
        clearConfirmedBox.SetActive(true);
    }

    public void CloseClearConfirmed()
    {
        clearConfirmedBox.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void CancelClear()
    {
        confirmClearDataBox.SetActive(false);
        mainMenu.SetActive(true);
    }

}
