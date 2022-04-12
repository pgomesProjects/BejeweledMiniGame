using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitlescreenController : MonoBehaviour
{
    [SerializeField] private GameObject[] menuStates;

    private enum MenuState { TITLESCREEN, HOWTOPLAY, SETTINGS };
    private MenuState currentMenuState;

    [SerializeField] private Slider[] volumeSliders;

    [SerializeField] private TextMeshProUGUI personalBest;
    // Start is called before the first frame update
    void Start()
    {
        personalBest.text = "Personal Best: " + PlayerPrefs.GetInt("PersonalBest");
        FindObjectOfType<AudioManager>().Play("Titlescreen", PlayerPrefs.GetFloat("BGMVolume", 0.5f));
        currentMenuState = MenuState.TITLESCREEN;

        volumeSliders[0].value = PlayerPrefs.GetFloat("BGMVolume", 0.5f) * 10;
        volumeSliders[1].value = PlayerPrefs.GetFloat("SFXVolume", 0.5f) * 10;
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
