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

    public void UpdateLevelText(int _level)
    {
        levelText.text = "Level " + _level;
    }
    public void UpdateXPBar(int _xp, int _maxXp)
    {
        xpBar.maxValue = _maxXp;
        xpBar.value = _xp;
    }
    public void UpdateHealthBar(int _health, int _maxHealth)
    {
        healthBar.maxValue = _maxHealth;
        healthBar.value = _health;
    }
}