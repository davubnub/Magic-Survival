using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public PlayerScript player;

    public GameObject[] optionButtons;
    public TextMeshProUGUI[] upgradeNameText;
    public TextMeshProUGUI[] upgradeDescriptionText;
    public Image[] upgradeIcons;

    const int amountOfOptions = 3;

    string[] upgradeNames           = { "Bigger benis", "Chonky", "Thicc ass", "Become gay" };
    string[] upgradeDescriptions    = { "very long yes", "chonk", "this is a description", "gay" };

    public void ShowUpgradeUI(bool _active)
    {
        print("active");
        gameObject.SetActive(_active);
    }

    public void SelectOptions()
    {
        int[] options        = { 0, 0, 0, 0 };
        int[] listOfUpgrades = { 0, 1, 2, 3 };

        for(int i = 0; i < amountOfOptions; i++)
        {
            print("listOfUpgrades " + listOfUpgrades.Length);
            //pick random upgrade
            int num = listOfUpgrades[Random.Range(0, listOfUpgrades.Length)];
            //remove that upgrade from the list of upgrades to choose from
            listOfUpgrades[System.Array.IndexOf(listOfUpgrades, num)] = listOfUpgrades[listOfUpgrades.Length - 1];
            System.Array.Resize(ref listOfUpgrades, listOfUpgrades.Length - 1);

            options[i] = num;

            upgradeNameText[i].text = upgradeNames[options[i]];
            upgradeDescriptionText[i].text = upgradeDescriptions[options[i]];
        }
    }

    public void OptionButtonPressed(int _number)
    {
        player.Upgrade();
    }
}
