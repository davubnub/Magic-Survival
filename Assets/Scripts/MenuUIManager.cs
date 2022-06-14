using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    public void ShowGameOverScreen(bool _active)
    {
        gameOverScreen.SetActive(_active);
    }

    public void RestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
