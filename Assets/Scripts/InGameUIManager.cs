using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InGameUIManager : MonoBehaviour
{
    public Slider xpBar;
    public Slider healthBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreText;

    public void UpdateLevelText(int _level)
    {
        levelText.text = "Level " + _level;
    }
    public void UpdateScoreText(int _score)
    {
        scoreText.text = "Score: " + _score;
    }
    public void UpdateXPBar(int _xp, int _maxXp)
    {
        xpBar.maxValue = _maxXp;
        xpBar.value = _xp;
    }
    public void UpdateHealthBar(float _health, float _maxHealth)
    {
        healthBar.maxValue = _maxHealth;
        healthBar.value = _health;
        healthBar.transform.localScale = new Vector3((float)_maxHealth/800, healthBar.transform.localScale.y, 1);
    }

    public void UpdateCoinText(int _coins)
    {
        coinText.text = "Coins $" + _coins;
    }
}