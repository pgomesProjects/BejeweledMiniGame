using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject endSessionMenu;
    [SerializeField] private Button endSessionButton;

    [SerializeField] private GameObject gameStatsMenu;
    [SerializeField] private TextMeshProUGUI scoreStats;
    [SerializeField] private TextMeshProUGUI timeStats;
    [SerializeField] private GameTimer timer;

    public void PromptEndSession()
    {
        endSessionButton.gameObject.SetActive(false);
        endSessionMenu.SetActive(true);
        PlayerController.main.SetMenuActive(true);
    }

    public void EndSession()
    {
        endSessionMenu.SetActive(false);
        PlayerController.main.SetGameActive(false);

        //Sets menu information
        scoreStats.text = "Final Score: " + PlayerController.main.GetPlayerScore();

        if (PlayerController.main.HasHighScore())
        {
            scoreStats.text += "<br><br><size=55>New High Score!</size>";
        }

        timeStats.text = "Time Spent: " + timer.ToString();

        CheckForEndSessionAchievements();

        gameStatsMenu.SetActive(true);
    }

    private void CheckForEndSessionAchievements()
    {
        //Achievement for 0 points
        if (PlayerController.main.GetPlayerScore() == 0 && PlayerPrefs.GetInt("AchievementID6") != 1)
        {
            AchievementListener.Instance.UnlockAchievement(6);
        }
    }

    public void CancelEndSession()
    {
        endSessionButton.gameObject.SetActive(true);
        endSessionMenu.SetActive(false);
        PlayerController.main.SetMenuActive(false);
    }

    public void BackToTitle()
    {
        if(FindObjectOfType<AudioManager>() != null)
        {
            if(FindObjectOfType<AudioManager>().IsPlaying("TrackAmongUs"))
                FindObjectOfType<AudioManager>().Stop("TrackAmongUs");
            else
                FindObjectOfType<AudioManager>().Stop("Track" + PlayerController.main.currentTrack);
        }
        SceneManager.LoadScene("Titlescreen");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
