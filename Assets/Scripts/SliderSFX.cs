using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderSFX : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private string sliderSFX;
    [SerializeField] private float repeatTime = 0.5f;

    private AudioManager audioManager;
    private IEnumerator sliderSFXRepeat;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        sliderSFXRepeat = RepeatSFX();
        StartCoroutine(sliderSFXRepeat);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(sliderSFXRepeat);
    }

    IEnumerator RepeatSFX()
    {
        while (true)
        {
            if (audioManager != null)
                audioManager.Play(sliderSFX, PlayerPrefs.GetFloat("SFXVolume", 0.5f));
            yield return new WaitForSeconds(repeatTime);
        }
    }
}
