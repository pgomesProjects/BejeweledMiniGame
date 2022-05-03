using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HowToPlayController : MonoBehaviour
{
    [SerializeField] private Image diagram;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private Sprite[] diagrams;
    [SerializeField] private string[] tutorialLines;
    [SerializeField] private Button[] advanceButtons;

    private int currentLine;

    private void OnEnable()
    {
        SetupTutorial();
    }

    private void SetupTutorial()
    {
        //Move to the first diagram / line
        currentLine = 0;
        UpdateUI();
    }

    public void PreviousButton()
    {
        //Move to the previous diagram / line
        currentLine--;
        UpdateUI();
    }

    public void NextButton()
    {
        //Move to the next diagram / line
        currentLine++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        //If the current line is in the length of the array, update the diagram
        if(currentLine < diagrams.Length)
            diagram.sprite = diagrams[currentLine];

        //If the current line is in the length of the array, update the text
        if (currentLine < tutorialLines.Length)
            tutorialText.text = tutorialLines[currentLine];

        //If at the first line, hide the previous button
        if (currentLine == 0)
            advanceButtons[0].gameObject.SetActive(false);
        else
            advanceButtons[0].gameObject.SetActive(true);

        //If at the last line, hide the next button
        if (currentLine == tutorialLines.Length - 1)
            advanceButtons[1].gameObject.SetActive(false);
        else
            advanceButtons[1].gameObject.SetActive(true);
    }
}
