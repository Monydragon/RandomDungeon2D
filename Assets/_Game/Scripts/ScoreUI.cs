using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;
    private int currentScore;
    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        EventManager.onScoreChanged += EventManager_onScoreIncrease;
    }

    private void OnDisable()
    {
        EventManager.onScoreChanged -= EventManager_onScoreIncrease;

    }
    private void EventManager_onScoreIncrease(int score)
    {
        currentScore += score;
        if(scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }
}
