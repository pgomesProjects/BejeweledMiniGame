using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI personalBestText;
    [SerializeField] private GameObject chainTextObject;
    private TextMeshProUGUI chainText;
    private float defaultChainFontSize;
    private CanvasGroup chainCanvasGroup;
    private IEnumerator chainDisplayCoroutine;

    [SerializeField] private int numberOfTracks = 3;

    [SerializeField] private float fadeInChainSeconds = 0.25f;
    [SerializeField] private float fadeOutChainSeconds = 2;

    private bool canMove;
    private bool menuActive;
    private bool hasHighScore;
    private bool isGameActive;

    private int playerScore;

    internal int currentTrack;

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
        chainText = chainTextObject.GetComponent<TextMeshProUGUI>();
        chainCanvasGroup = chainTextObject.GetComponent<CanvasGroup>();
        chainCanvasGroup.alpha = 0;
        defaultChainFontSize = chainText.fontSize;

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

    public void DisplayChain(int multiplier)
    {
        if(chainDisplayCoroutine != null)
            StopCoroutine(chainDisplayCoroutine);

        chainDisplayCoroutine = ChainAnimation(multiplier);
        StartCoroutine(chainDisplayCoroutine);
    }

    public IEnumerator ChainAnimation(int multiplier)
    {
        chainText.text = "Chain X" + multiplier + "!";
        chainText.fontSize = defaultChainFontSize + (3 * multiplier);

        float currentTimer = 0;

        //Fade in chain animation
        while (currentTimer < fadeInChainSeconds)
        {
            currentTimer += Time.deltaTime;
            chainCanvasGroup.alpha = Mathf.Clamp01(currentTimer / fadeInChainSeconds);
            yield return null;
        }

        chainCanvasGroup.alpha = 1;

        currentTimer = 0;

        //Fade out chain animation
        while (currentTimer < fadeOutChainSeconds)
        {
            currentTimer += Time.deltaTime;
            chainCanvasGroup.alpha = 1 - Mathf.Clamp01(currentTimer / fadeOutChainSeconds);
            yield return null;
        }

        chainCanvasGroup.alpha = 0;
    }

    private void CheckForAchievementScore(int score)
    {
        //Achievement for 25,000 points
        if (score >= 25000 && PlayerPrefs.GetInt("AchievementID1") != 1)
        {
            AchievementListener.Instance.UnlockAchievement(1);
        }

        //Achievement for 100,000 points
        if (score >= 100000 && PlayerPrefs.GetInt("AchievementID3") != 1)
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
