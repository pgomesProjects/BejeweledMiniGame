using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int playerScore;

    public static PlayerController main;
    PlayerControlSystem playerControls;
    private void Awake()
    {
        main = this;
        playerControls = new PlayerControlSystem();
        playerControls.Player.Quit.performed += _ => QuitGame();
        playerScore = 0;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void UpdateScore(int score)
    {
        playerScore += score;
        scoreText.text = "Score: " + playerScore;
    }

    private void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
