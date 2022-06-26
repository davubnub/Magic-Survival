using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MenuUIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI highScoreText;
    public PlayerScript player;
    public UIManager uiManager;
    public EnemySpawner enemySpawner;
    public CoinSpawner coinSpawner;
    public GameObject prizePrompt;
    public GameObject purchasePrompt;
    public PrizeScript prizeScript;

    public int coinsToPurchase;

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
    public void StartPressed()
    {
        player.StartPressed();
        enemySpawner.StartPressed();
        coinSpawner.StartPressed();
    }
    public void CustomizePressed()
    {
        uiManager.ShowCustomizeUI(true);
        uiManager.ShowPlayScreen(false);
    }
    public void SettingsPressed()
    {
        uiManager.ShowSettingUI(true);
        uiManager.ShowPlayScreen(false);
    }
    public void BackPressed()
    {
        uiManager.ShowPlayScreen(true);
        uiManager.ShowCustomizeUI(false);
        uiManager.ShowPurchaseUI(false);
        uiManager.ShowSettingUI(false);
    }
    public void PurchasePressed()
    {
        uiManager.ShowPurchaseUI(true);
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - coinsToPurchase);
        purchasePrompt.SetActive(PlayerPrefs.GetInt("Coins") >= coinsToPurchase);
        prizeScript.UnlockPrize();
    }
    public void GameOver(int _coins)
    {
        purchasePrompt.SetActive(_coins >= coinsToPurchase);
    }
}