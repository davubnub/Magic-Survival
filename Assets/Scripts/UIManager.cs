using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject gameScreen;
    public GameObject upgradeScreen;

    public void ShowGameOverScreen(bool _active)
    {
        gameOverScreen.SetActive(_active);
    }
    public void ShowInGameUI(bool _active)
    {
        gameScreen.SetActive(_active);
    }
    public void ShowUpgradeUI(bool _active)
    {
        upgradeScreen.SetActive(_active);
    }
}
