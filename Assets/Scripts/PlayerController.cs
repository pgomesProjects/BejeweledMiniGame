using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI personalBestText;
    [SerializeField] private int numberOfTracks = 3;

    private bool canMove;
    private bool menuActive;
    private bool hasHighScore;
    private bool isGameActive;

    private int playerScore;

    [HideInInspector]
    public int currentTrack;

    public static PlayerController main;

    private void Awake()
    {
        main = this;
        playerScore = 0;
        hasHighScore = false;
        isGameActive = true;
        canMove = false;
        menuActive = false;
    }

    private void Start()
    {
        personalBestText.text = "Personal Best: " + PlayerPrefs.GetInt("PersonalBest");

        //Play a random track from the list of tracks
        if(FindObjectOfType<AudioManager>() != null)
        {
            currentTrack = Random.Range(1, numberOfTracks + 1);

            //5% chance to get easter egg soundtrack
            if(Random.value < 0.05f)
            {
                FindObjectOfType<AudioManager>().Play("TrackAmongUs", PlayerPrefs.GetFloat("BGMVolume", 0.5f));
                EasterEggAchievement();
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("Track" + currentTrack, PlayerPrefs.GetFloat("BGMVolume", 0.5f));
            }
        }
    }

    private void EasterEggAchievement()
    {
        if(PlayerPrefs.GetInt("AchievementID4") != 1 && AchievementListener.Instance != null)
        {
            AchievementListener.Instance.UnlockAchievement(4);
        }
    }

    public void UpdateScore(int score)
    {
        //Update score while the game is active
        if (isGameActive)
        {
            playerScore += score;
            scoreText.text = "Score: " + playerScore;

            if (AchievementListener.Instance != null)
                CheckForAchievementScore(playerScore);

            //If the current score is greater than the personal best, update the personal best score
            if (playerScore > PlayerPrefs.GetInt("PersonalBest"))
            {
                hasHighScore = true;
                PlayerPrefs.SetInt("PersonalBest", playerScore);
                personalBestText.text = "Personal Best: " + PlayerPrefs.GetInt("PersonalBest");
            }
        }
    }

    private void CheckForAchievementScore(int score)
    {
        //Achievement for 2,500 points
        if (score >= 2500 && PlayerPrefs.GetInt("AchievementID1") != 1)
        {
            AchievementListener.Instance.UnlockAchievement(1);
        }

        //Achievement for 10,000 points
        if (score >= 10000 && PlayerPrefs.GetInt("AchievementID3") != 1)
        {
            AchievementListener.Instance.UnlockAchievement(3);
        }
    }

    public bool IsGameActive() { return isGameActive; }
    public void SetGameActive(bool gameActive) { isGameActive = gameActive; }
    public bool HasHighScore() { return hasHighScore; }
    public int GetPlayerScore() { return playerScore; }
    public bool GetCanMove() { return canMove; }
    public void SetCanMove(bool move) { canMove = move; }
    public bool IsMenuActive() { return menuActive; }
    public void SetMenuActive(bool menu) { menuActive = menu; }
}
