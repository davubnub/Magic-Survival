using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public PlayerScript player;
    public UpgradeStats upgradeStats;

    public GameObject[] optionButtons;
    public TextMeshProUGUI[] upgradeNameText;
    public TextMeshProUGUI[] upgradeDescriptionText;
    public Image[] upgradeIcons;
    public TextMeshProUGUI skipText;

    public int skipCoin;

    const int amountOfOptions = 3;
    const int maxTiers = 5;

    int[] options = { 0, 0, 0 };

    public void SelectOptions()
    {
        skipText.text = "Skip +$" + skipCoin;

        List<int> listOfUpgrades = new List<int>();

        for (int i = 0; i < upgradeStats.GetUpgradeStats().Count; i++)
        {
            if (upgradeStats.GetUpgradeStats()[i].tierLevel <= maxTiers)
            {
                listOfUpgrades.Add(i);
            }
        }

        int[] arrayOfUpgrades = listOfUpgrades.ToArray();

        for (int i = 0; i < amountOfOptions; i++)
        {
            //pick random upgrade
            int num = arrayOfUpgrades[Random.Range(0, arrayOfUpgrades.Length)];
            //remove that upgrade from the list of upgrades to choose from
            arrayOfUpgrades[System.Array.IndexOf(arrayOfUpgrades, num)] = arrayOfUpgrades[arrayOfUpgrades.Length - 1];
            System.Array.Resize(ref arrayOfUpgrades, arrayOfUpgrades.Length - 1);

            options[i] = num;

            UpgradeStats.upgradeTiers upgradeTier = upgradeStats.GetUpgradeStats()[options[i]];

            upgradeNameText[i].text = upgradeTier.upgradeName + " " + Mathf.Clamp(upgradeTier.tierLevel + 1, 0, 5);
            upgradeDescriptionText[i].text = upgradeTier.upgradeDescription;
        }
    }

    public void OptionButtonPressed(int _number)
    {
        UpgradeStats.upgradeTiers upgradeTier = upgradeStats.GetUpgradeStats()[options[_number]];

        player.Upgrade(upgradeTier.upgrade, upgradeTier.positiveUpgrade, upgradeTier.negativeUpgrade);

        int i = Mathf.Clamp(upgradeTier.tierLevel + 1, 0, 5);
        upgradeStats.GetUpgradeStats()[options[_number]].SetUpgradeTier(i);
    }

    public void SkipPressed()
    {
        player.IncreaseCoins(skipCoin);
        player.Upgrade(PlayerScript.UPGRADES.none, 0, 0);
    }
}
