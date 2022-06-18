using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MenuUIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI highScoreText; 

    public void RestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateScoreText(int _score)
    {
        scoreText.text = "Score: " + _score;
    }
    public void UpdateHighScoreText(int _highScore)
    {
        highScoreText.text = "HighScore: " + _highScore;
    }
}
