using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem: MonoBehaviour
{
    [SerializeField] private int scoreValue;

    public int GetScoreValue() { return scoreValue; }
    public void SetScoreValue(int value) { scoreValue = value; }
}
