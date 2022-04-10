using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BackToTitle()
    {
        FindObjectOfType<AudioManager>().Stop("Track" + PlayerController.main.currentTrack);
        SceneManager.LoadScene("Titlescreen");
    }
}
