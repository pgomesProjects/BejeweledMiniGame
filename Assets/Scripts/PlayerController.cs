using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI personalBestText;
    [SerializeField] private int numberOfTracks = 3;

    private int playerScore;
    [HideInInspector]
    public int currentTrack;

    public static PlayerController main;
    PlayerControlSystem playerControls;

    private bool canMove;

    private void Awake()
    {
        main = this;
        playerControls = new PlayerControlSystem();
        playerControls.Player.Quit.performed += _ => QuitGame();
        playerScore = 0;
        canMove = false;
    }

    private void Start()
    {
        personalBestText.text = "Personal Best: " + PlayerPrefs.GetInt("PersonalBest");

        //Play a random track from the list of tracks
        if(FindObjectOfType<AudioManager>() != null)
        {
            currentTrack = Random.Range(1, numberOfTracks + 1);
            FindObjectOfType<AudioManager>().Play("Track" + currentTrack, PlayerPrefs.GetFloat("BGMVolume"));
        }
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

        //If the current score is greater than or equal to the personal best, update the personal best score
        if(playerScore >= PlayerPrefs.GetInt("PersonalBest"))
        {
            PlayerPrefs.SetInt("PersonalBest", playerScore);
            personalBestText.text = "Personal Best: " + PlayerPrefs.GetInt("PersonalBest");
        }
    }

    public bool GetCanMove() { return canMove; }
    public void SetCanMove(bool move) { canMove = move; }

    private void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
