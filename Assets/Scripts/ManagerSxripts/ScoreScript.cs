using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public static int scoreValue = 0;
    private TMP_Text scoreText;

    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        scoreValue += amount;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        scoreValue = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null) // Checks if scoreText is assigned
        {
            scoreText.text = scoreValue.ToString();
        }
    }
}
