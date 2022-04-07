using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitlescreenController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI personalBest;
    // Start is called before the first frame update
    void Start()
    {
        personalBest.text = "Personal Best: " + PlayerPrefs.GetInt("PersonalBest");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
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
