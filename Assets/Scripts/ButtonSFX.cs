using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private string hoverSFXName, clickSFXName;

    AudioManager audioManager;
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioManager != null)
            audioManager.PlayOneShot(hoverSFXName, PlayerPrefs.GetFloat("SFXVolume", 0.5f));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioManager != null)
            audioManager.PlayOneShot(clickSFXName, PlayerPrefs.GetFloat("SFXVolume", 0.5f));
    }
}
