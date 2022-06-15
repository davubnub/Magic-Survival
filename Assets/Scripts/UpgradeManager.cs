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

    const int amountOfOptions = 3;

    int[] options = { 0, 0, 0 };

    public void SelectOptions()
    {
        int[] listOfUpgrades = new int[upgradeStats.GetUpgradeStats().Count];

        for (int i = 0; i < upgradeStats.GetUpgradeStats().Count; i++)
        {
            listOfUpgrades[i] = i;
        }

        for(int i = 0; i < amountOfOptions; i++)
        {
            //pick random upgrade
            int num = listOfUpgrades[Random.Range(0, listOfUpgrades.Length)];
            //remove that upgrade from the list of upgrades to choose from
            listOfUpgrades[System.Array.IndexOf(listOfUpgrades, num)] = listOfUpgrades[listOfUpgrades.Length - 1];
            System.Array.Resize(ref listOfUpgrades, listOfUpgrades.Length - 1);

            options[i] = num;

            upgradeNameText[i].text = upgradeStats.GetUpgradeStats()[options[i]].upgradeName;
            upgradeDescriptionText[i].text = upgradeStats.GetUpgradeStats()[options[i]].upgradeDescription;
        }
    }

    public void OptionButtonPressed(int _number)
    {
        UpgradeStats.upgradeTiers upgradeTier = upgradeStats.GetUpgradeStats()[options[_number]];

        player.Upgrade(upgradeTier.upgrade, upgradeTier.tiers[upgradeTier.tierLevel]);

        int i = Mathf.Clamp(upgradeTier.tierLevel + 1, 0, 5);
        upgradeStats.GetUpgradeStats()[options[_number]].SetUpgradeTier(i);
    }
}
