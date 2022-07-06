using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PrizeScript : MonoBehaviour
{
    public int minCoins;
    public int maxCoins;
    public TextMeshProUGUI coinText;

    public void GivePrize()
    {
        int coinPrize = Random.Range(minCoins, maxCoins);
        coinPrize = (int)Mathf.Ceil((coinPrize / 10)) * 10;
        coinText.text = "" + coinPrize;
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + coinPrize);
    }
}
