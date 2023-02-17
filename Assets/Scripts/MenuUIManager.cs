using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI highScoreText;
    public PlayerScript player;
    public UIManager uiManager;
    public EnemySpawner enemySpawner;
    public ObstacleSpawner obstacleSpawner;
    public CoinSpawner coinSpawner;
    public GameObject prizePrompt;
    public GameObject purchasePrompt;
    public GameObject watchAdPrompt;
    public PurchaseScript purchaseScript;
    public PrizeScript prizeScript;
    public PoolingManager poolingManager;
    public TextMeshProUGUI[] coinTexts;
    public Animator cameraAnimator;
    public Animator fadeAnimator;
    public AdvertisingScript advertisingScript;

    public int coinsToPurchase;

    [Header("Audio")]
    public GameObject BGM;
    public bool hasSound = true;
    public GameObject MusicButton;
    public GameObject SoundButton;

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
        StartCoroutine(StartDelay());
    }
    IEnumerator StartDelay()
    {
        cameraAnimator.SetTrigger("PlayPosition");
        poolingManager.SpawnIntialObjects();
        uiManager.ShowPlayScreen(false);

        yield return new WaitForSeconds(1.5f);

        uiManager.ShowInGameUI(true);
        player.StartPressed();
        enemySpawner.StartPressed();
        coinSpawner.StartPressed();
        obstacleSpawner.StartPressed();
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
    public void AcceptPressed()
    {
        uiManager.ShowPurchaseUI(false);
        uiManager.ShowPrizeUI(false);
    }
    public void PurchasePressed()
    {
        uiManager.ShowPurchaseUI(true);
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - coinsToPurchase);
        purchasePrompt.SetActive(PlayerPrefs.GetInt("Coins") >= coinsToPurchase);
        purchaseScript.UnlockPrize();
        UpdateCoinTexts();
    }
    public void PrizePressed()
    {
        uiManager.ShowPrizeUI(true);
        prizePrompt.SetActive(false);
        prizeScript.GivePrize();
        PlayerPrefs.SetInt("Day", System.DateTime.Today.Day);
        UpdateCoinTexts();
    }

    public void GameOver(int _coins)
    {
        purchasePrompt.SetActive(_coins >= coinsToPurchase);
        watchAdPrompt.SetActive(Random.Range(0, 100) < advertisingScript.chanceForAd);
        prizePrompt.SetActive(PlayerPrefs.GetInt("Day", 0) != System.DateTime.Today.Day);
        UpdateCoinTexts();
        fadeAnimator.SetTrigger("Fade");

        if (Random.Range(0, 100) < advertisingScript.chanceForAd)
        {
            advertisingScript.ShowAd();
        }
    }

    void UpdateCoinTexts()
    {
        for(int i = 0; i < coinTexts.Length; i++)
        {
            coinTexts[i].text = "Volts: " + PlayerPrefs.GetInt("Coins");
        }
    }

    public void ToggleMusic()
    {
        if (BGM.activeSelf)
        {
            Color Red = new Color(0.78f, 0.36f, 0.38f, 1.0f);
            MusicButton.GetComponent<Image>().color = Red;

            BGM.SetActive(false);
        }
        else
        {
            Color Green = new Color(0.56f, 0.74f, 0.45f, 1.0f);
            MusicButton.GetComponent<Image>().color = Green;

            BGM.SetActive(true);
        }
    }
    public void ToggleSound()
    {
        if (hasSound)
        {
            Color Red = new Color(0.78f, 0.36f, 0.38f, 1.0f);
            SoundButton.GetComponent<Image>().color = Red;

            hasSound = false;
        }
        else
        {
            Color Green = new Color(0.56f, 0.74f, 0.45f, 1.0f);
            SoundButton.GetComponent<Image>().color = Green;

            hasSound = true;
        }
    }
}