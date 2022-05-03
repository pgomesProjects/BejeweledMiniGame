using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitlescreenController : MonoBehaviour
{
    [SerializeField] private GameObject blackoutScreen;
    [SerializeField] private Color endingColor = new Color(1, 1 , 1, 1);
    [SerializeField] private float startingAniSeconds;

    [SerializeField] private GameObject allAchievementsTrophy;
    [SerializeField] private GameObject[] menuStates;

    private enum MenuState { TITLESCREEN, HOWTOPLAY, ACHIEVEMENTS, SETTINGS };
    private MenuState currentMenuState;

    [SerializeField] private Slider[] volumeSliders;

    [SerializeField] private TextMeshProUGUI personalBest;

    private void Awake()
    {
        blackoutScreen.GetComponent<CanvasGroup>().alpha = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Default values on first run
        if(PlayerPrefs.GetInt("FirstRun") == 0)
        {
            PlayerPrefs.SetFloat("BGMVolume", 0.5f);
            PlayerPrefs.SetFloat("SFXVolume", 0.5f);
            PlayerPrefs.SetInt("FirstRun", 1);
        }

        personalBest.text = "Personal Best: " + PlayerPrefs.GetInt("PersonalBest");
        FindObjectOfType<AudioManager>().Play("Titlescreen", PlayerPrefs.GetFloat("BGMVolume"));
        currentMenuState = MenuState.TITLESCREEN;

        Debug.Log("Volumes: " + PlayerPrefs.GetFloat("BGMVolume") + " | " + PlayerPrefs.GetFloat("SFXVolume"));
        volumeSliders[0].value = PlayerPrefs.GetFloat("BGMVolume") * 10;
        volumeSliders[1].value = PlayerPrefs.GetFloat("SFXVolume") * 10;

        allAchievementsTrophy.SetActive(AllAchievementsUnlocked());

        StartCoroutine(StartingAnimation());
    }

    IEnumerator StartingAnimation()
    {
        float currentTimer = 0;
        CanvasGroup blackoutCanvas = blackoutScreen.GetComponent<CanvasGroup>();

        //Fade in blackout screen
        while (currentTimer < startingAniSeconds)
        {
            currentTimer += Time.deltaTime;
            blackoutCanvas.alpha = 1 - Mathf.Clamp01(currentTimer / startingAniSeconds);
            blackoutCanvas.GetComponent<Image>().color = Color.Lerp(blackoutCanvas.GetComponent<Image>().color, endingColor, (startingAniSeconds / 10) * Time.deltaTime);
            yield return null;
        }

        blackoutCanvas.alpha = 0;
    }

    private void CheckForAchievements()
    {
        foreach (var i in AchievementHolder.Instance.achievementItem)
        {
            //If the player prefs say the achievement is unlocked, make sure it is unlocked
            if (PlayerPrefs.GetInt("AchievementID" + i.id) == 1)
            {
                i.isUnlocked = true;
            }
        }
    }

    private bool AllAchievementsUnlocked()
    {
        CheckForAchievements();

        foreach (var i in AchievementHolder.Instance.achievementItem)
            if (i.isUnlocked == false)
                return false;
        return true;
    }

    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Stop("Titlescreen");
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMenu(int menu)
    {
        //Set the current menu state to false
        menuStates[(int)currentMenuState].SetActive(false);
        //Change the state
        currentMenuState = (MenuState)menu;
        //Change the current menu state to true
        menuStates[(int)currentMenuState].SetActive(true);

        //Update achievements menu if going there
        if (currentMenuState == MenuState.ACHIEVEMENTS)
            if (AchievementsController.Instance != null)
                AchievementsController.Instance.CreateAchievementsBoard();
    }

    public void ChangeBGMVolume(float value)
    {
        PlayerPrefs.SetFloat("BGMVolume", value * 0.1f);
        FindObjectOfType<AudioManager>().ChangeVolume("Titlescreen", PlayerPrefs.GetFloat("BGMVolume", 0.5f));
    }

    public void ChangeSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value * 0.1f);
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
